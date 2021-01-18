using System;
using System.Data;
using System.Data.OleDb;

namespace WPF_ProductLabel.Tools
{
    public class OLEDBHelper
    {
        private OleDbConnection connection = null;
        public OLEDBHelper(string connectionString)
        {
            connection = new OleDbConnection(connectionString);
        }

        private void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        private void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public DataTable GetOleDbSchemaTable()
        {
            DataTable dataTable = new DataTable();
            try
            {
                OpenConnection();
                dataTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            }
            catch { return null; }
            finally
            {
                CloseConnection();
            }
            return dataTable;
        }

        public DataTable Query(string query)
        {
            DataTable dataTable = new DataTable();

            using (connection)
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection))
                {
                    using (dataTable)
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
    }
}
