using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K_Bikpower
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        //private readonly SQLiteConnection datbaseQ;

        readonly SQLiteConnection conn;
        public Database(string dbPath)
        {
            conn = new SQLiteConnection(dbPath);
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Assets>().Wait();
            _database.CreateTableAsync<Substation_Codes>().Wait();
            _database.CreateTableAsync<User>().Wait();
        }

        public Task<List<Assets>> GetPeopleAsync()
        {
            return _database.Table<Assets>().ToListAsync();
        }

        public List<User> GetUserAsync()
        {
            return conn.Table<User>().ToList();

        }

        public Task<List<Substation_Codes>> GetSubAsync()
        {
            return _database.Table<Substation_Codes>().ToListAsync();
        }

        public Task<List<Substation_Codes>> GetSubAssetsAsync(string sub)
        {
            return _database.Table<Substation_Codes>().Where(a => a.Substation_Code == sub).ToListAsync();
        }

        public Task<int> SaveStudentAsync(Assets Asset)
        {
            return _database.InsertAsync(Asset);
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<int> SaveSubAsync(Substation_Codes Asset)
        {
            return _database.InsertAsync(Asset);
        }

        public Task<int> UpdateStudentAsync(Assets Asset)
        {
            return _database.InsertOrReplaceAsync(Asset);
        }

        public Task<int> UpdateSubAsync(Substation_Codes Asset)
        {
            return _database.InsertOrReplaceAsync(Asset);
        }

        public List<Assets> Scangen(int scan)
        {
            List<Assets> aset = conn.Table<Assets>().Where(a => a.Id == scan).ToList();
            return aset;
        }
    }
}
