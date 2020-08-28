using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace StockManager.Core.Extensions
{
    public static class DataExtensions
    {
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase)) return true;
            }
            return false;
        }
    }
}
