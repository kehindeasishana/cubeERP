
using Data;
using System.Web.Mvc;
using Web.Models;
using System.Linq;
using System;
using System.Web;
using System.IO;
using System.Data;
//using LumenWorks.Framework.IO.Csv;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Web;
using Core.Domain;
using System.Collections.Generic;
using Core.Domain.Financials;
using CsvHelper;
//using Web.Models.ViewModels;
//using System.Web.UI.WebControls;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        //private readonly Services.Administration.IAdministrationService _administrationService;
        //public HomeController(Services.Administration.IAdministrationService administrationService)
        //{
        //    _administrationService = administrationService;
        //}
        public HomeController()
        {

        }

        public ActionResult Category()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Category(string Countries, string States)
        {

            int stateID = -1;
            if (!int.TryParse(States, out stateID))
            {
                ViewBag.YouSelected = "You must select a Country and State";
                return View();
            }
            var state = from s in State.GetStates()
                      where s.StateID == stateID
                      select s.StateName;
            //var state = from s in State.GetStates()
            //            where s.StateID == stateID
            //            select s.StateName;

            var country = from s in Country.GetCountries()
                          where s.Code == Countries
                          select s.Name;


            ViewBag.YouSelected = "You selected " + country.SingleOrDefault()
                                 + " And " + state.SingleOrDefault();
            return View("Info");
        }
        public SelectList GetCountrySelectList()
        {

            var countries = Country.GetCountries();
            return new SelectList(countries.ToArray(),
                                "Code",
                                "Name");

        }

        public ActionResult IndexDDL()
        {

            ViewBag.Country = GetCountrySelectList();
            return View();
        }

        [HttpPost]
        public ActionResult IndexDDL(string Countries, string States)
        {

            ViewBag.Countries = GetCountrySelectList();

            int stateID = -1;
            if (!int.TryParse(States, out stateID))
            {
                ViewBag.YouSelected = "You must select a Country and State";
                return View();
            }

            var state = from s in State.GetStates()
                        where s.StateID == stateID
                        select s.StateName;

            var country = from s in Country.GetCountries()
                          where s.Code == Countries
                          select s.Name;


            ViewBag.YouSelected = "You selected " + country.SingleOrDefault()
                                 + " And " + state.SingleOrDefault();
            return View("Info");

        }
        public ActionResult CountryList()
        {
            var countries = Country.GetCountries();

            if (HttpContext.Request.IsAjaxRequest())
                return Json(GetCountrySelectList(), JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");
        }

        public ActionResult StateList(string ID)
        {
            string Code = ID;
            var states = from s in State.GetStates()
                         where s.Code == Code
                         select s;

            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(
                                states.ToArray(),
                                "StateID",
                                "StateName")
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");
        }
        private static DataTable ProcessCSV(string fileName)
        {
            //Set up our variables
            string Feedback = string.Empty;
            string line = string.Empty;
            string[] strArray;
            DataTable dt = new DataTable();
            DataRow row;
            // work out where we should split on comma, but not in a sentence
            Regex r = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //Set the filename in to our stream
            StreamReader sr = new StreamReader(fileName);

            //Read the first line and split the string at , with our regular expression in to an array
            line = sr.ReadLine();
            strArray = r.Split(line);

            //For each item in the new split array, dynamically builds our Data columns. Save us having to worry about it.
            Array.ForEach(strArray, s => dt.Columns.Add(new DataColumn()));

            //Read each line in the CVS file until it’s empty
            while ((line = sr.ReadLine()) != null)
            {
                row = dt.NewRow();

                //add our current value to our data row
                row.ItemArray = r.Split(line);
                dt.Rows.Add(row);
            }

            //Tidy Streameader up
            sr.Dispose();

            //return a the new DataTable
            return dt;

        }
        private static String ProcessBulkCopy(DataTable dt)
        {
            string Feedback = string.Empty;
            string connString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            //make our connection and dispose at the end
            using (SqlConnection conn = new SqlConnection(connString))
            {
                //make our command and dispose at the end
                using (var copy = new SqlBulkCopy(conn))
                {

                    //Open our connection
                    conn.Open();

                    ///Set target table and tell the number of rows
                    copy.DestinationTableName = "JournalEntryLine";
                    copy.BatchSize = dt.Rows.Count;
                    try
                    {
                        //Send it to the server
                        copy.WriteToServer(dt);
                        Feedback = "Upload complete";
                    }
                    catch (Exception ex)
                    {
                        Feedback = ex.Message;
                    }
                }
            }

            return Feedback;
        }

        [Audit]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            //ViewBag.Message = "Your application description page.";

            return View();
        }
        [Audit]
        public ActionResult HomePage()
        {
            //if (!User.Identity.IsAuthenticated)
               // return RedirectToAction("Login", "Account");

            //if (_administrationService.GetDefaultCompany() == null)
            //    Data.DbInitializerHelper.Initialize();

            return View();

        }
      
        public ActionResult AddBatchUpload()
        {
            
            return View();
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddBatchUpload(HttpPostedFileBase upload)
        //public ActionResult AddBatchUpload(HttpPostedFileBase FileUpload)
        //{

        //    // Set up DataTable place holder
        //    DataTable dt = new DataTable();

        //    //check we have a file
        //    if (FileUpload.ContentLength > 0)
        //    {
        //        //Workout our file path
        //        string fileName = Path.GetFileName(FileUpload.FileName);
        //        string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);

        //        //Try and upload
        //        try
        //        {
        //            FileUpload.SaveAs(path);
        //            //Process the CSV file and capture the results to our DataTable place holder
        //            dt = ProcessCSV(path);

        //            //Process the DataTable and capture the results to our SQL Bulk copy
        //            ViewData["Feedback"] = ProcessBulkCopy(dt);
        //        }
        //        catch (Exception ex)
        //        {
        //            //Catch errors
        //            ViewData["Feedback"] = ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        //Catch errors
        //        ViewData["Feedback"] = "Please select a file";
        //    }

        //    //Tidy up
        //    dt.Dispose();

        //    return View("Index", ViewData["Feedback"]);
        {
            List<FileUploadViewModel> fileUpload = new List<FileUploadViewModel>();
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {

                    if (upload.FileName.EndsWith(".csv"))
                    {
                        // Set up DataTable place holder
                        // DataTable dt = new DataTable();
                        var fileName = Path.GetFileName(upload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                       
                        upload.SaveAs(path);
                        var csv = new CsvReader(new StreamReader(path));
                        var journalEntry = csv.GetRecords<JournalEntryLine>();
                        foreach (var journal in journalEntry) // Each record will be fetched and printed on the screen
                        {
                            FileUploadViewModel uploadFile = new FileUploadViewModel();
                            uploadFile.AccountId = journal.AccountId;
                            uploadFile.SubCategoryId = journal.SubCategoryId;
                            uploadFile.Amount = journal.Amount;
                            uploadFile.Date = journal.Date;
                            uploadFile.Description = journal.Description;
                            uploadFile.ReferenceNo = journal.ReferenceNo;
                            uploadFile.DrCr = journal.DrCr;
                            uploadFile.Id = journal.Id;
                            fileUpload.Add(uploadFile);
                        }
                        //return View();
                        //Stream reader will read test.csv file in current folder
                        //StreamReader sr = new StreamReader(Server.MapPath("~/Content/Upload"));
                        //Stream stream = upload.InputStream;
                        //DataTable csvTable = new DataTable();
                        //CsvReader csvRead = new CsvReader(new StreamReader(stream));
                        //IEnumerable<JournalEntryLine> record = csvRead.GetRecords<JournalEntryLine>();
                        //Process the CSV file and capture the results to our DataTable place holder
                         //dt = ProcessCSV(path);
                        //Try and upload
                        //        try
                        //        {
                        //            FileUpload.SaveAs(path);
                        //            

                        //            //Process the DataTable and capture the results to our SQL Bulk copy
                        //            ViewData["Feedback"] = ProcessBulkCopy(dt);
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            //Catch errors
                        //            ViewData["Feedback"] = ex.Message;
                        //        }
                        ModelState.Clear();
                        //Stream reader will read test.csv file in current folder
                        //StreamReader sr = new StreamReader(Server.MapPath(@"test.csv"));
                        //Csv reader reads the stream
                        //CsvReader csvReader = new CsvReader(sr);
                        //csvread will fetch all record in one go to the IEnumerable object record
                        
                       
                        
                         //csvTable.Load(csvRead);

                        
                        //foreach (var rec in record) // Each record will be fetched and printed on the screen
                        //{
                        //    decimal amount = rec.Amount;
                        //    DateTime account = rec.Date;
                        //    string memo = rec.Memo;
                        //    string referenceNo = rec.ReferenceNo;
                        //    int accountId = rec.AccountId;
                           
                        //}

                        //using (CsvReader csvRead =
                        //    new CsvReader(new StreamReader(stream), true))
                        //{
                        //    csvTable.Load(csvRead);
                            

                        //    //Process the DataTable and capture the results to our SQL Bulk copy
                        //    //ViewData["Feedback"] = ProcessBulkCopy(csvTable);

                        //}
                        //var connectionString = "ApplicationContext";
                        //var adapter = new SqlDataAdapter("Select * From[Sheet1$]", connectionString);
                        //var ds = new DataSet();
                        //adapter.Fill(ds, "Results");
                        //DataTable data = ds.Tables["Results"];
                        //foreach (DataRow row in data.Rows)
                        //{
                        //    Decimal Amount = Convert.ToDecimal(row["Amount"]);
                        //    DateTime Date = Convert.ToDateTime(row["Date"]);
                        //    string Description = row["Memo"].ToString();
                        //    string ReferenceNo = row["ReferenceNo"].ToString();
                        //    int AccountId = Convert.ToInt32(row["AccountId"]);
                        //    int SubCategoryId = Convert.ToInt32(row["SubCategoryId"]);

                        //    UploadService.SaveFileDetails(AccountId, SubCategoryId, Amount, Date, Description, ReferenceNo);
                        //}
                        //return View(csvTable);
                        
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return View(fileUpload);
        }
        //public ActionResult AddBatchUpload()
        //{
        //    return View();
        //}
        //[Audit]
        //[HttpPost]
        //public ActionResult AddBatchUpload(HttpPostedFileBase file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (file == null)
        //        {
        //            ModelState.AddModelError("File", "Please Upload Your file");
        //        }
        //        else if (file.ContentLength > 0)
        //        {
        //            int MaxContentLength = 1024 * 1024 * 3; //3 MB
        //            string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf",".csv",".xls",".xlsx",".docx" };

        //            if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
        //            {
        //                ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
        //            }

        //            else if (file.ContentLength > MaxContentLength)
        //            {
        //                ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
        //            }
        //            else
        //            {
        //                //TO:DO
        //                var fileName = Path.GetFileName(file.FileName);
        //                var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
        //                file.SaveAs(path);
        //                ModelState.Clear();
        //                ViewBag.Message = "File uploaded successfully";
        //            }
        //        }
        //    }
        //    return View();
        //}

        [Audit]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PopupWIndowTest()
        {
            return PartialView("_PopupWindowTest");
        }
    }

   
}
