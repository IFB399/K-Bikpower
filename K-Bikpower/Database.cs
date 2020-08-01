﻿using SQLite;
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
        }

        public Task<List<Assets>> GetPeopleAsync()
        {
            return _database.Table<Assets>().ToListAsync();
        }

        public Task<List<Substation_Codes>> GetSubAsync()
        {
            return _database.Table<Substation_Codes>().ToListAsync();
        }

        public Task<int> SaveStudentAsync(Assets Asset)
        {
            return _database.InsertAsync(Asset);
        }

        public Task<int> UpdateStudentAsync(Assets Asset)
        {
            return _database.InsertOrReplaceAsync(Asset);
        }

        public List<Assets> Scangen(string scan)
        {
            List<Assets> aset = conn.Table<Assets>().Where(a => a.Id == 1).ToList();
            return aset;
        }
    }
}
