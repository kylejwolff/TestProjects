using System;
using System.Data.SqlClient;
using System.Linq;

namespace ValidConnectionStringTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var connex = @"Data Source={0}\{1};Initial Catalog=Master;Integrated Security=True;";


            var conn = new SqlConnection(string.Format(connex, "(localdb)", "Projects"));
            conn.Open();
            conn.Dispose();
            // this proves the trailing slash is fine in connex strings, makes it easy to handle server\instance stuff in installers!
            conn = new SqlConnection(string.Format(connex, ".", ""));
            conn.Open();
            conn.Dispose();
        }
    }
}
