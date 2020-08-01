using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K_Bikpower
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;
        //private readonly SQLiteConnection datbaseQ;


        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Assets>().Wait();
           // datbaseQ = new SQLiteConnection(dbPath);
        }

        public Task<List<Assets>> GetPeopleAsync()
        {
            return _database.Table<Assets>().ToListAsync();
        }

        public Task<int> SaveStudentAsync(Assets Asset)
        {
            return _database.InsertAsync(Asset);
        }

        public Task<int> UpdateStudentAsync(Assets Asset)
        {
            return _database.InsertOrReplaceAsync(Asset);
        }


        public async Task<IEnumerable<Assets>> QueryValuationsAsync(string QRcode)
        {
           //var test = _database.QueryAsync<Assets>("select * from" + QRcode);
           // var result = await query.ToListAsync();

            var query = _database.Table<Assets>().Where(Assets.Id = "0");

            var result2 = await query.ToListAsync();


            return result2;
        }

    }
}
