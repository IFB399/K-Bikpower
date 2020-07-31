using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K_Bikpower
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;


        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Assets>().Wait();
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
            System.Console.WriteLine("test");
            return _database.InsertOrReplaceAsync(Asset);
        }


       // public IEnumerable<Assets> QueryValuations(Assets Asset)
       // {
        //    var test = _database.QueryAsync<Assets>("select * from Assets where Id = ?", Asset);
        //    return test;
       // }

    }
}
