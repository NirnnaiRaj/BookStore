using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axobis.Restaurant.Core.Constants
{
    public class SqlTableTypeColumns
    {
        public static Dictionary<string, string> Orders_Columns = new Dictionary<string, string>{
            { "Order_ID","System.Int32" },
            { "Book_ID","System.Int32" },
        };
    }
}
