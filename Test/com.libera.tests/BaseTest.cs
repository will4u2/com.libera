using System;
using System.Data;
using System.Data.SQLite;

namespace com.libera.tests
{
    public abstract class BaseTest
    {

        internal void BuildDatabaseAndSeed(SQLiteConnection connection)
        {
            string sqlString = @"CREATE TABLE CoinTypes (
		                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Type varchar(200) NOT NULL,
                                            Value decimal(18,2) NOT NULL,
	                                        Active bit NOT NULL,
	                                        InUser bigint NOT NULL,
	                                        InDate datetime NOT NULL,
	                                        InApplication bigint NULL,
	                                        ModificationUser bigint NULL,
	                                        ModificationDate datetime NULL,
	                                        ModificationApplication bigint NULL,
	                                        DeleteUser bigint NULL,
	                                        DeleteDate datetime NULL,
	                                        DeleteApplication bigint NULL                                            
                                        );
                                    CREATE TABLE Coins
                                        (
		                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Quantity int NOT NULL,
                                            CoinTypeId int NOT NULL,
	                                        Active bit NOT NULL,
	                                        InUser bigint NOT NULL,
	                                        InDate datetime NOT NULL,
	                                        InApplication bigint NULL,
	                                        ModificationUser bigint NULL,
	                                        ModificationDate datetime NULL,
	                                        ModificationApplication bigint NULL,
	                                        DeleteUser bigint NULL,
	                                        DeleteDate datetime NULL,
	                                        DeleteApplication bigint NULL                                            
                                        );
									INSERT INTO CoinTypes (Type, Value, Active, InDate, InUser, InApplication) VALUES ('quarter', 0.25, 1, date('now'), 1, 1);
									INSERT INTO CoinTypes (Type, Value, Active, InDate, InUser, InApplication) VALUES ('dime', 0.1, 1, date('now'), 1, 1);
									INSERT INTO CoinTypes (Type, Value, Active, InDate, InUser, InApplication) VALUES ('nickel', 0.05, 1, date('now'), 1, 1);
									INSERT INTO CoinTypes (Type, Value, Active, InDate, InUser, InApplication) VALUES ('penny', 0.01, 1, date('now'), 1, 1);
                                ";
            try
            {
                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sqlString.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("  ", " ");
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.Connection.Open();
                        var results = cmd.ExecuteNonQuery();
                    }
                    catch(Exception exc)
                    {
                        string message = exc.Message.ToString();
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.Message.ToString();
            }
        }
    }
}
