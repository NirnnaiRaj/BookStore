using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business
{
    public class DataUtilities
    {
        public static DataTable CreateDataTable(string name, Dictionary<string, string> columns)
        {

            if (columns == null || columns.Count <= 0)
            {
                throw new AppException("Columns parameter cannot be null", AppErrorCode.DataTableColumnsCannotbeNull);
            }

            DataColumn[] dataColumns = new DataColumn[columns.Count];
            for (var i = 0; i < columns.Count; i++)
            {

                var keyVal = columns.ElementAt(i);
                var dataColumn = new DataColumn(keyVal.Key, Type.GetType(keyVal.Value));
                dataColumns[i] = dataColumn;
            }

            var dt = new DataTable(name);
            dt.Columns.AddRange(dataColumns);

            return dt;

        }
    }
}
