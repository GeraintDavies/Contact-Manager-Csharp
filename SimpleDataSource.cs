using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

class SimpleDataSource : IDisposable
{
    MySqlConnection Conn;
    string Server, Database, User, Password;
    int Port;

    /// <summary>
    /// Constructor, calls Connect method with params
    /// </summary>
    /// <param name="Server">Server IP or hostname</param>
    /// <param name="Database">Database/schema name</param>
    /// <param name="Port">Port number</param>
    /// <param name="User">Username</param>
    /// <param name="Password">Password</param>
    public SimpleDataSource(string Server, string Database, int Port,
        string User, string Password)
    {
        Connect(Server, Database, Port, User, Password);
    }


    /// <summary>
    /// Intialises MySqlConnection object with parameters provided.
    /// </summary>
    /// <param name="Server">Server IP or hostname</param>
    /// <param name="Database">Database/schema name</param>
    /// <param name="Port">Port number</param>
    /// <param name="User">Username</param>
    /// <param name="Password">Password</param>
    public void Connect(string Server, string Database, int Port, string User, string Password)
    {
        this.Server = Server;
        this.Database = Database;
        this.Port = Port;
        this.User = User;
        this.Password = Password;

        Conn = new MySqlConnection(string.Format("server={0};user={1};database={2};port={3};password={4};",
                Server, User, Database, Port, Password));
        try
        {
            Conn.Open();
        }
        catch (MySqlException E)
        {
            Console.WriteLine(E.Message);
            Conn = null;
        }
    }

    /// <summary>
    /// Creates an SQL query from the provided string and
    /// executes it.
    /// </summary>
    /// <param name="QueryString">A string containing an SQL query</param>
    /// <returns>A MySqlDataReader object with the results 
    /// of the query or null if an exception occurs.</returns>


    public MySqlDataReader Query(string QueryString)
    { 
        MySqlDataReader Reader = null;
        if (Conn != null)
        {
            lock (Conn)
            {
                try
                {
                    MySqlCommand Command = new MySqlCommand(QueryString, Conn);
                    Reader = Command.ExecuteReader();
                }
                catch (MySqlException E)
                {
                    Console.WriteLine(E.Message);
                }
            }
        }

        return Reader;
    }

    /// <summary>
    /// creates the datatable to be used within the project.
    /// </summary>
    /// <param name="QueryString"></param>
    /// <param name="Params"></param>
    /// <returns></returns>
    public DataTable QueryDataTable(string QueryString, Dictionary<string, string> Params)
    {
        DataTable Table = null;
        MySqlCommand Command = null;

        if (Conn != null)
        {
            lock (Conn)
            {
                try
                {
                    Command = new MySqlCommand(QueryString, Conn);
                    MySqlDataAdapter Adapter = new MySqlDataAdapter(Command);
                    Table = new DataTable();
                    Adapter.Fill(Table);
                }
                catch (MySqlException E)
                {
                    Console.WriteLine(E.Message);
                }
            }
        }

        return Table;
    }

    /// <summary>
    /// Creates an SQL statement from the provided string and
    /// executes it. 
    /// </summary>
    /// <param name="updateString">A string containing an SQL non-query</param>
    public void Update(string UpdateString)
    {
        if (Conn != null)
        {
            lock (Conn)
            {
                try
                {
                    MySqlCommand Command = new MySqlCommand(UpdateString, Conn);
                    Command.ExecuteNonQuery();
                }
                catch (MySqlException E)
                {
                    Console.WriteLine(E.Message);
                }
            }
        }
    }

    /// <summary>
    /// Garbage collection method called by Garbage Collector.
    /// SimpleDataSource implements IDisposable, so the GC will
    /// know to call this method when the object is no longer
    /// needed.
    /// </summary>
    public void Dispose()
    {
        if (Conn != null)
            Conn.Dispose();
    }
}

