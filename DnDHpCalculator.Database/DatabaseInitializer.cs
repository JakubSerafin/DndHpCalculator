using Microsoft.Data.Sqlite;

public class SqlLiteDatabase
{
    public static void Initialize()
    {
        if (!File.Exists("DnD.db"))
            using (var connection = new SqliteConnection("Data Source=DnD.db"))
            {
                connection.Open();
                var createTableCommand = connection.CreateCommand();
                //IT Will be non-relational database, so only ID and JSON string will be stored
                createTableCommand.CommandText = """
                                                 CREATE TABLE CharacterSheets (
                                                     Id INTEGER PRIMARY KEY,
                                                     Data TEXT NOT NULL
                                                 );
                                                 """;
                createTableCommand.ExecuteNonQuery();
            }
    }

    public static SqliteConnection GetConnection()
    {
        var connection = new SqliteConnection("Data Source=DnD.db");
        connection.Open();
        return connection;
    }
}