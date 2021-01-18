using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WPF_ProductLabel.Tools
{
    class ExcelFileHandler
    {
        //private DataTable _data;

        public DataTable FileDataTableSchema { get; set; }
        public string FilePath { get; set; }
        public int CurrentTableIndex { get; private set; }
        //public DataTable Data
        //{ 
        //    get
        //    {
        //        if (_data != null)
        //        {
        //            return _data;
        //        }
        //        return null;
        //    }
        //    private set
        //    {
        //        _data = value;
        //    } 
        //}

        public DataTable Load(string filePath)
        {
            FilePath = filePath;
            string connectionString = string.Format(
                "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';Extended Properties=\"Excel 12.0;HDR=YES;\"",
                FilePath.Substring(FilePath.LastIndexOf("\'") + 1)
            );

            OLEDBHelper oledbHelper = new OLEDBHelper(connectionString);

            FileDataTableSchema = oledbHelper.GetOleDbSchemaTable();

            if (FileDataTableSchema.Rows.Count < 1)
            {
                throw new Exception("File does not contain any row of data!");
            }

            if (CurrentTableIndex == -1)
            {
                CurrentTableIndex = 0;
            }

            DataTable data = oledbHelper.Query("select * from [" + FileDataTableSchema.Rows[CurrentTableIndex]["TABLE_NAME"].ToString() + "]");
            return data;
        }

        public async Task<DataTable> LoadAsync(string filePath)
        {
            var myTask = Task.Run(() => Load(filePath));
            DataTable result = await myTask;
            return result;
        }
    }
}
