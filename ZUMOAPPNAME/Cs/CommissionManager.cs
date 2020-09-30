/*
 * To add Offline Sync Support:
 *  1) Add the NuGet package Microsoft.Azure.Mobile.Client.SQLiteStore (and dependencies) to all client projects
 *  2) Uncomment the #define OFFLINE_SYNC_ENABLED
 *
 * For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342
 */
//#define OFFLINE_SYNC_ENABLED

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
    public partial class CommissionManager
    {
        static CommissionManager defaultInstance = new CommissionManager();
        MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<CommissionData> todoTable;
#else
        IMobileServiceTable<CommissionData> todoTable;
#endif

        const string offlineDbPath = @"localstore.db";

        private CommissionManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<CommissionData>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.todoTable = client.GetSyncTable<CommissionData>();
#else
            this.todoTable = client.GetTable<CommissionData>();
#endif
        }

        public static CommissionManager DefaultManager
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
            get { return todoTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<CommissionData>; }
        }

        public async Task<ObservableCollection<CommissionData>> GetCFormsAsync(bool syncItems = false, string submittedBy = null, string status = null, ObservableCollection<string> formIds = null)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<CommissionData> items = await todoTable.ToEnumerableAsync();
                if (formIds != null)
                {
                    items = items.Where(asset => formIds.Contains(asset.Id)); //the user is viewing forms of a particular asset
                }
                if (submittedBy != null)
                {
                    items = items.Where(asset => asset.SubmittedBy == submittedBy);
                }

                if (status != null)
                {
                    items = items.Where(asset => asset.Status == status);
                    /*
                    if (status == "Rejected")
                    {
                        items = items.Where(asset => asset.Status == "Rejected");
                    }
                    else if (status == "Approved")
                    {
                        items = items.Where(asset => asset.Status == "Approved");
                    }
                    else
                    {
                        items = items.Where(asset => asset.Status == status);
                    }
                    */
                }


                return new ObservableCollection<CommissionData>(items.OrderByDescending(item => item.DateCommissioned));
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
        public async Task DeleteFormAsync(CommissionData d)
        {
            await todoTable.DeleteAsync(d);
        }

        public async Task<ObservableCollection<string>> GetSubmittedByNames()
        {
            IEnumerable<string> items = await todoTable.Where(form => (form.SubmittedBy != "" && form.SubmittedBy != null)).Select(form => form.SubmittedBy).ToEnumerableAsync();
            items = items.Distinct();
            return new ObservableCollection<string>(items);
        }

        public async Task SaveTaskAsync(CommissionData item)
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
