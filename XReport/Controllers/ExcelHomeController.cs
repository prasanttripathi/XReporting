using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
//using OfficeOpenXml;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.Runtime.InteropServices;
using GridMvc;
using System.Collections;
using System.Diagnostics;


namespace XReport.Controllers
{
    public class ExcelHomeController : Controller
    {
       public static string csvFilePath, xlsFilePath, excelFilePath = string.Empty;
      
        // GET: ExcelHome
        public ActionResult UploadExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {

            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);
               
               
                if (fileExtension == ".csv")
                {
                    csvFilePath =    System.IO.Path.GetFullPath(Request.Files["file"].FileName);
                    excelFilePath = @"D:\" + (Path.GetFileName(Request.Files["file"].FileName)).Replace(fileExtension, ".xlsx");

                   Application app = new Application();
                   Workbook wb = app.Workbooks.Open(csvFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                   wb.SaveAs(excelFilePath, XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                   wb.Close();
                   Marshal.ReleaseComObject(wb);
                   app.Quit();

                   Marshal.ReleaseComObject(app);
                   KillExcel();
                   ViewBag.data = "Converted from .csv to .xslx !! find at " + excelFilePath;
                   ViewBag.Status = "Yes";
                   return View();
                  
                }

                else if (fileExtension == ".xls" || fileExtension == ".xlsx" )
                {
                    if (fileExtension == ".xls")
                    {
                        xlsFilePath = System.IO.Path.GetFullPath(Request.Files["file"].FileName);
                        excelFilePath = @"D:\" + (Path.GetFileName(Request.Files["file"].FileName)).Replace(fileExtension, ".xlsx");
                        Application app = new Application();
                        Workbook wb = app.Workbooks.Open(xlsFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                       
                        wb.SaveAs(excelFilePath, XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                       

                        ViewBag.data = "Converted from .xls to .xlsx !! find at " + excelFilePath;
                        ViewBag.Status = "Yes";
                        wb.Close();
                        Marshal.ReleaseComObject(wb);
                        app.Quit();
                       
                        Marshal.ReleaseComObject(app);
                        KillExcel();
                        return View();

                    }
                   
                }
                else
                ViewBag.Data = "some error,make sure you are uploading a .xls or .xlsx format file only";
                ViewBag.Status = "No";
            }
                              
            return View();
           
        }

        public void KillExcel()
        {
           var process = System.Diagnostics.Process.GetProcessesByName("Excel");
           foreach (var p in process)
           {
               if (!string.IsNullOrEmpty(p.ProcessName))
               {
                   try
                   {
                       p.Kill();
                   }
                   catch { }
               }
           }

        }
 
        [HttpGet]
       /* public ActionResult GenerateReport()
        {
           
                Application xlApp = new Application();
                Workbook xlWorkbook = xlApp.Workbooks.Open(excelFilePath);
                _Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Range xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;
                int MaincolCount = xlRange.Columns.Count;

                System.Data.DataTable tableReport = new System.Data.DataTable();


                int suf = 0;
                try
                {

                    for (int i = 1; i <= rowCount; i++)
                    {

                        colCount = MaincolCount;
                        if (colCount!=0)
                        { 
                       // colCount = MaincolCount;
                        foreach (object columnName in xlRange.Rows[i].Value)
                        {
                            string temp = Convert.ToString(columnName);
                           
                                if (!tableReport.Columns.Contains(temp))
                                {
                                    tableReport.Columns.Add(temp);
                                }
                                else
                                {
                                    tableReport.Columns.Add(temp + suf);
                                    suf++;
                                }

                                colCount = colCount - 1;

                                                     
                        }
                        }
                        else 
                        {
                            if (i == 1)
                                break;
                                else
                            { 
                               DataRow row = tableReport.NewRow();
                            row["id"] = i;
                            row["item"] = "item " + i.ToString();
                            tableReport.Rows.Add(row);
                               // tableReport.Rows.Add(xlRange.Rows[i].Value);
                            }
                        }
                       
                    }

                    Session["table"] = tableReport;
            }
            finally
                {
          
            Marshal.ReleaseComObject(xlRange);
           Marshal.ReleaseComObject(xlWorksheet);
           xlWorkbook.Close();
           Marshal.ReleaseComObject(xlWorkbook);
            xlApp.Quit();
           Marshal.ReleaseComObject(xlApp);
                }
               Response.Redirect("http://localhost:64761/Report.aspx");
                return View(tableReport);

        }*/

        private List<IDictionary> ConvertToDictionary(System.Data.DataTable dtObject)
        {
            var columns = dtObject.Columns.Cast<DataColumn>();

            var dictionaryList = dtObject.AsEnumerable()
                .Select(dataRow => columns
                    .Select(column =>
                        new { Column = column.ColumnName, Value = dataRow[column] })
                             .ToDictionary(data => data.Column, data => data.Value)).ToList().ToArray();

            return dictionaryList.ToList<IDictionary>();
        }
 
        public ActionResult GenerateReport()
        {

            Application xlApp = new Application();
            Workbook xlWorkbook = xlApp.Workbooks.Open(excelFilePath);
            _Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
            int MaincolCount = xlRange.Columns.Count;
            string[] columnsofTable = new string[colCount];
            System.Data.DataTable tableReport = new System.Data.DataTable();
            DataRow row;
         
           
            int suf = 0;
            int r=0;
            string cl=string.Empty;
            try
            {
                for (int i = 1; i <= rowCount; i++)
                {
                  
                    row = tableReport.NewRow();
                    colCount = MaincolCount;

                    foreach (object value1  in xlRange.Rows[i].Value)
                    {
                        string temp = Convert.ToString(value1);

                        if (i == 1)
                        { 
                                 if (colCount != 0 )
                                {
                                        if (!tableReport.Columns.Contains(temp))
                                        {
                                            if(temp.Contains("Date"))
                                            {
                                                tableReport.Columns.Add(temp, typeof(DateTime));
                                                columnsofTable[colCount - 1] = temp;
                                            }
                                            else
                                            { 
                                            tableReport.Columns.Add(temp);
                                            columnsofTable[colCount-1] = temp;
                                            }
                                        }
                                        else
                                        {
                                            cl = temp + suf;
                                            tableReport.Columns.Add(cl);
                                            columnsofTable[colCount-1] = cl;
                                            suf++;  
                                        }                           
                                    colCount = colCount - 1;                                 
                                }
                        }
                        else{

                            if (columnsofTable[colCount - 1].Contains("Date"))
                            {

                                DateTime dateTime;
                                if (DateTime.TryParse(temp, out dateTime))
                                {
                                    row[columnsofTable[colCount - 1]] = dateTime;
                                    colCount--;
                                    r = 1;
                                }
                                else
                                {
                                   DateTime dateTime2  = new DateTime(2015, 1, 1, 1, 1, 1);
                                   row[columnsofTable[colCount - 1]] = dateTime2;
                                    colCount--;
                                    r = 1;
                                }
                            }
                            else
                            {
                                row[columnsofTable[colCount - 1]] = temp;
                                colCount--;
                                r = 1;
                            }
                           
                        }
                    }
                    if (r == 1)
                    {
                        tableReport.Rows.Add(row);
                    }

                }
                
               Session.Add("datatable",tableReport);
               Session.Add("columns",columnsofTable);
                
            }
            finally
            {

                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
                KillExcel();
            }

            ViewBag.Status = "Yes";
            ViewBag.panel = "Visible";
            var dictionaryList = ConvertToDictionary(tableReport);
            ViewBag.data = "Report has been generated ";
           // ViewBag.report1 = dictionaryList;
           TempData["report"] = dictionaryList;
            return View("UploadExcel");

        }

        public ActionResult ViewReport()
        {
            return View("GenerateReport", TempData["report"]);
        }

        public ActionResult CreateReport()
        {


            System.Data.DataTable dtReport = new System.Data.DataTable();
            dtReport =  (System.Data.DataTable)Session["datatable"];
            DateTime input = new DateTime(2015, 8, 1, 7, 10, 24); 
            //2/8/2016  6:33:00 PM
           IEnumerable<DataRow> query =
                                        from dtrow in dtReport.AsEnumerable()
                                        where dtrow.Field<DateTime>("Reported Date+") > input
                                        select dtrow;
           System.Data.DataTable tempDt = new System.Data.DataTable();
            tempDt = dtReport.Clone();

          tempDt.Rows.Clear();

           foreach (DataRow dr in query)
           {
               DataRow row = tempDt.NewRow();
               row = dr;              
              // tempDt.Rows.Add(row.ItemArray);
               tempDt.ImportRow(row);
              // tempDt.Rows.Add(drRow);  //This row already belongs to another Table error       
           }

           var idic = ConvertToDictionary(tempDt);
           return View("CreateReport", idic);
        }


        public DateTime input { get; set; }
    }
}