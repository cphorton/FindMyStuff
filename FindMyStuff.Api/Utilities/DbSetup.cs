using Dapper;
using FindMyStuff.Api.Shared;
using Microsoft.Data.Sqlite;
using Serilog;

namespace FindMyStuff.Api.Utilities
{
    public static class DbSetup
    {
        public static async Task Setup()
        {
            SqliteConnection conn;
            conn = new SqliteConnection(Constants.SQL_CONN);

            var tableScripts = new List<string> {

                //Create Item table
                @"create table if not exists Item (ItemId varchar(36) primary key, Name varchar(100), 
                    Description TEXT)",

                //Create Item Search table
                @"CREATE VIRTUAL TABLE IF NOT EXISTS ItemSearch 
                    USING fts4(ItemId varchar(36) PRIMARY KEY, NameDescription TEXT)",

                //Create Item Search update triggers
                @"CREATE TRIGGER IF NOT EXISTS ItemSearchInsert
                    AFTER INSERT ON Item
                    BEGIN
                        INSERT INTO ItemSearch (ItemId, NameDescription) VALUES (NEW.ItemId, NEW.Name || ' - ' || NEW.Description);
                    END;",
                
                //Create Image table
                @"CREATE TABLE IF NOT EXISTS Image (
                    ImageId varchar(36) PRIMARY KEY,
                    ParentId varchar(36),
                    Path text,
                    Description text
                        )"
                };

            foreach (var tableScript in tableScripts)
            {
                try
                {
                    await conn.ExecuteAsync(tableScript);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error creating table" );
                }
            }
        }
    }
}
