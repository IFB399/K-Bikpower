/*
 * To add Offline Sync Support:
 *  1) Add the NuGet package Microsoft.Azure.Mobile.Client.SQLiteStore (and dependencies) to all client projects
 *  2) Uncomment the #define OFFLINE_SYNC_ENABLED
 *
 * For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342
 */
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
    public partial class AssetManager
    {
        static AssetManager defaultInstance = new AssetManager();
        MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Asset> todoTable;
#else
        IMobileServiceTable<Asset> todoTable;
#endif

        const string offlineDbPath = @"localstore.db";

        private AssetManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<Asset>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.todoTable = client.GetSyncTable<Asset>();
#else
            this.todoTable = client.GetTable<Asset>();
#endif
        }

        public static AssetManager DefaultManager
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
            get { return todoTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<Asset>; }
        }

        public async Task<ObservableCollection<Asset>> GetTodoItemsAsync(bool syncItems = false, string substation = null, string equipmentClass = null, string manufacturer = null)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Asset> items = await todoTable.ToEnumerableAsync();
                if (manufacturer != null)
                {
                    items = items.Where(asset => asset.ManufacturerName == manufacturer);
                }
                if (substation != null)
                {
                    items = items.Where(asset => asset.SubstationCode == substation);
                }
                if (equipmentClass != null)
                {
                    items = items.Where(asset => asset.EquipmentClassDescription == equipmentClass);
                }

                return new ObservableCollection<Asset>(items);
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
        public async Task<ObservableCollection<string>> GetManufacturerNames()
        {
            IEnumerable<string> items = await todoTable.Select(asset => asset.ManufacturerName).ToEnumerableAsync();
            items = items.Distinct();
            return new ObservableCollection<string>(items);
        }
        public async Task<ObservableCollection<string>> GetSubstationCodes()
        {
            IEnumerable<string> items = await todoTable.Select(asset => asset.SubstationCode).ToEnumerableAsync();
            items = items.Distinct();
            return new ObservableCollection<string>(items);
        }
        public async Task<ObservableCollection<string>> GetEquipmentClass()
        {
            IEnumerable<string> items = await todoTable.Select(asset => asset.EquipmentClassDescription).ToEnumerableAsync();
            items = items.Distinct();
            return new ObservableCollection<string>(items);
        }

        public async Task<ObservableCollection<Asset>> GetScan(string id)
        {
            IEnumerable<Asset> items = await todoTable.Where(asset => asset.Id == id).ToEnumerableAsync();
            items = items.Distinct();
            return new ObservableCollection<Asset>(items);
        }
        public async Task SaveTaskAsync(Asset item)
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
