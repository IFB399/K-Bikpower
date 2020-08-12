#define OFFLINE_SYNC_ENABLED

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
#endif

namespace K_Bikpower
{

        public partial class TableManager
        {
            static TableManager defaultInstance = new TableManager();
            MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Assets> todoTable;
#else
            IMobileServiceTable<Assets> todoTable;
#endif

            const string offlineDbPath = @"localstore.db";

            private TableManager()
            {
                this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<Assets>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.todoTable = client.GetSyncTable<Assets>();
#else
                this.todoTable = client.GetTable<Assets>();
#endif
            }

            public static TableManager DefaultManager
            {
                get
                {
                    return defaultInstance;
                }
                private set
                {
                    defaultInstance = value;
                }
            }

            public MobileServiceClient CurrentClient
            {
                get { return client; }
            }

            public bool IsOfflineEnabled
            {
                get { return todoTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Assets>; }
            }

            public async Task<ObservableCollection<Assets>> GetTodoItemsAsync(bool syncItems = false)
            {
                try
                {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                    IEnumerable<Assets> items = await todoTable
                        .Where(Assets => !Assets.Done)
                        .ToEnumerableAsync();

                    return new ObservableCollection<Assets>(items);
                }
                catch (MobileServiceInvalidOperationException msioe)
                {
                    Debug.WriteLine("Invalid sync operation: {0}", new[] { msioe.Message });
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Sync error: {0}", new[] { e.Message });
                }
                return null;
            }

            public async Task SaveTaskAsync(Assets item)
            {
                try
                {
                    if (item.Id == null)
                    {
                        await todoTable.InsertAsync(item);
                    }
                    else
                    {
                        await todoTable.UpdateAsync(item);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Save error: {0}", new[] { e.Message });
                }
            }

#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.todoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allTodoItems",
                    this.todoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
#endif
        }
    }

