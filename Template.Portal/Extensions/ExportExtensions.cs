//using ClosedXML.Excel;
using System.Data;
using System.Reflection;
using Template.Library.Attributes;

namespace Template.Portal.Extensions
{
    public static class ExportExtensions
    {
        //public static MemoryStream GenerateItemsExport(this IEnumerable<ClientViewModel> clients)
        //{
        //    var response = clients.ToList().ToDataTable();

        //    return ExportToExcel(response);
        //}

        //public static MemoryStream GenerateItemsExport(this IEnumerable<LoanApplicationViewModel> clients)
        //{
        //    var response = clients.ToList().ToDataTable();

        //    return ExportToExcel(response);
        //}

        //public static DataTable ToDataTable<T>(this List<T> items)
        //{
        //    DataTable dataTable = new DataTable(typeof(T).Name);

        //    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(pi => pi.GetCustomAttributes(typeof(ExportAttribute), true).Length == 1).ToArray();

        //    foreach (PropertyInfo prop in Props)
        //    {
        //        var name = typeof(T).GetProperty(prop.Name).GetCustomAttribute<ExportAttribute>().Name;

        //        dataTable.Columns.Add(name);
        //    }
        //    foreach (T item in items)
        //    {
        //        var values = new object[Props.Length];
        //        for (int i = 0; i < Props.Length; i++)
        //        {
        //            //inserting property values to datatable rows
        //            values[i] = Props[i].GetValue(item, null);
        //        }
        //        dataTable.Rows.Add(values);
        //    }

        //    return dataTable;
        //}

        //public static MemoryStream ExportToExcel(System.Data.DataTable exportData)
        //{
        //    MemoryStream stream = new();

        //    var workbook = new XLWorkbook();
        //    workbook.AddWorksheet(exportData);
        //    workbook.SaveAs(stream);
        //    stream.Position = 0;
        //    return stream;
        //}
    }
}
