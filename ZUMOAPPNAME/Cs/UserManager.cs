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
    public partial class UserManager
    {
        static UserManager defaultInstance = new UserManager();
        MobileServiceClient client;
        string authenicationstring;
        string SavedUsername; 

#if OFFLINE_SYNC_ENABLED
        
#else
        IMobileServiceTable<User> todoTable;
#endif

        const string offlineDbPath = @"localstore.db";

        private UserManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<User>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.todoTable = client.GetSyncTable<User>();
#else
            this.todoTable = client.GetTable<User>();
#endif
        }

        public static UserManager DefaultManager
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
            get { return todoTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<User>; }
        }

        public async Task<ObservableCollection<User>> GetTodoItemsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<User> items = await todoTable
                    .ToEnumerableAsync();

                return new ObservableCollection<User>(items);
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

        
        //public async Task<User> GetUser(string username)
        public async Task<ObservableCollection<User>> GetUser(string email)
        {

            IEnumerable<User> items = await todoTable.Where(user => user.Email == email) //ideally just want to return a user not a collection?
                .ToEnumerableAsync();

            //User u = (User)await todoTable.Where(user => user.Username == username) //ideally just want to return a user not a collection?
                //.ToEnumerableAsync();

            return new ObservableCollection<User>(items);
            //return u;
        }

        public async Task GetUserAuth(string email)
        {

            var items = await todoTable.Where(user => user.Email == email).Select(user => user.Permission).ToEnumerableAsync();
            authenicationstring = items.First();
        }

        public  string Authentication()
        {
            return authenicationstring;
        }

        public void SetUsername(string email)
        {
            SavedUsername = email;
        }
        public string ReturnUser()
        {
            return SavedUsername;
        }
        public async Task DeleteUserAsync(User a)
        {
            await todoTable.DeleteAsync(a);
        }

        public async Task SaveTaskAsync(User item)
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
