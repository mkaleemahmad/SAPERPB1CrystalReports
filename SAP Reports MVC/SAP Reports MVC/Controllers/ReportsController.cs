using SAP_Reports_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAP_Reports_MVC.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        UserTrainingxDBEntities db = new UserTrainingxDBEntities();

        // GET: Reports
        public ActionResult Index()
        {
            ViewBag.CashAccounts = (from o in db.OACTs
                                    where o.AcctName.StartsWith("Cash")
                                    select new
                                    {
                                        AcctCode = o.AcctCode,
                                        DisplayValue = o.AcctCode + " - " + o.AcctName
                                    }).ToList();

            return View();
        }
        // GET: Cash Flow Reports
        public ActionResult CashFlow()
        {
            try
            {

                ViewBag.CashAccounts = (from o in db.OACTs
                                        where o.AcctName.StartsWith("Cash")
                                        select new
                                        {
                                            AcctCode = o.AcctCode,
                                            DisplayValue = o.AcctCode + " - " + o.AcctName
                                        }).ToList();

                ViewBag.BPs = (from o in db.OCRDs
                               select new
                               {
                                   CardCode = o.CardCode,
                                   DisplayValue = o.CardCode + " - " + o.CardName
                               }).ToList();

            }
            catch (Exception ex)
            {
            }
            return View();
        }

        [HttpPost]
        // POST: Cash Flow Reports
        public FileStreamResult CashFlow(string cashAccount, string dateFrom, string dateTo, string BPFrom, string BPTo, int? rptNo)
        {
            decimal? OB = db.spCashAccountOB(cashAccount, dateFrom).FirstOrDefault();
            rptNo = 1;
            CashFlowReport rpt = new CashFlowReport();
            rpt.SetDatabaseLogon("sa", "nsfinmanapp47865", "localhost", "UserTrainingxDB", true);
            rpt.SetParameterValue("@CashAccount", cashAccount);
            rpt.SetParameterValue("@FromDate", dateFrom);
            rpt.SetParameterValue("@ToDate", dateTo);
            rpt.SetParameterValue("@FromAccount", BPFrom);
            rpt.SetParameterValue("@ToAccount", BPTo);
            rpt.SetParameterValue("pmCompany", "Al-meraj Bakers & Sweets");
            rpt.SetParameterValue("pmOpeningBalance", OB);
            //rpt.SaveAs("abc.pdf", true);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            //Response.AppendHeader("Content-Disposition", "inline; filename=" + "CashFlowReport.pdf");

            //return File(stream, "application/pdf", "CashFlowReport.pdf");
            return File(stream, "application/pdf");

            //          }
            //            return Redirect("/Reports.aspx?cashAccount=" + cashAccount+"&dateFrom="+dateFrom+"&dateTo="+dateTo+"&BPFrom="+BPFrom+"&BPTo="+BPTo+"&rptNo="+rptNo+"");
            //            return RedirectToRoute("~/Reports.aspx");
        }


        // GET: Cash Flow Reports
        public ActionResult CashFlowDetail()
        {
            try
            {

                ViewBag.CashAccounts = (from o in db.OACTs
                                        where o.AcctName.StartsWith("Cash")
                                        select new
                                        {
                                            AcctCode = o.AcctCode,
                                            DisplayValue = o.AcctCode + " - " + o.AcctName
                                        }).ToList();

                ViewBag.BPs = (from o in db.OCRDs
                               select new
                               {
                                   CardCode = o.CardCode,
                                   DisplayValue = o.CardCode + " - " + o.CardName
                               }).ToList();

            }
            catch (Exception ex)
            {
            }
            return View();
        }


        //public FileStreamResult ShowReport(string cashAccount, string dateFrom, string dateTo, string BPFrom, string BPTo, int? rptNo)
        //{
        //    Stream stream;
        //    try
        //    {
        //        decimal? OB = db.spCashAccountOB(cashAccount, dateFrom).FirstOrDefault();

        //        if (rptNo == 1)
        //        {
        //            CashFlowReport rpt = new CashFlowReport();
        //            rpt.SetDataSource(db.spCashFlow(cashAccount, " ", " ", dateFrom, dateTo));
        //            //rpt.SetDatabaseLogon("sa", "nsfinmanapp47865", "localhost", "UserTrainingxDB", true);
        //            rpt.SetParameterValue("@CashAccount", cashAccount);
        //            rpt.SetParameterValue("@FromDate", dateFrom);
        //            rpt.SetParameterValue("@ToDate", dateTo);
        //            rpt.SetParameterValue("@FromAccount", BPFrom);
        //            rpt.SetParameterValue("@ToAccount", BPTo);
        //            rpt.SetParameterValue("pmCompany", "Al-meraj Bakers & Sweets");
        //            rpt.SetParameterValue("pmOpeningBalance", OB);
        //            //rpt.SaveAs("abc.pdf", true);
        //            Response.Buffer = false;
        //            Response.ClearContent();
        //            Response.ClearHeaders();
        //            stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //            stream.Seek(0, SeekOrigin.Begin);
        //            return File(stream, "application/pdf");
        //        }
        //        else if (rptNo == 2)
        //        {
        //            CashFlowDetailReport rpt = new CashFlowDetailReport();
        //            rpt.SetDatabaseLogon("sa", "nsfinmanapp47865", ".", "UserTrainingxDB", true);
        //            rpt.SetParameterValue("@CashAccount", cashAccount);
        //            rpt.SetParameterValue("@FromDate", dateFrom);
        //            rpt.SetParameterValue("@ToDate", dateTo);
        //            rpt.SetParameterValue("@FromAccount", BPFrom);
        //            rpt.SetParameterValue("@ToAccount", BPTo);
        //            rpt.SetParameterValue("pmCompany", "Al-meraj Bakers & Sweets");
        //            rpt.SetParameterValue("pmOpeningBalance", OB);
        //            //rpt.SaveAs("abc.pdf", true);
        //            Response.Buffer = false;
        //            Response.ClearContent();
        //            Response.ClearHeaders();
        //            stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        //            stream.Seek(0, SeekOrigin.Begin);
        //            return File(stream, "application/pdf");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    stream = Stream.Null;
        //    return File(stream, "application/pdf");
        //}


        public FileStreamResult ShowReport(string cashAccount, string dateFrom, string dateTo, string BPFrom, string BPTo, int? rptNo)
        {
            DataTable table = new DataTable();

            string connString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

            //var con = new SqlConnection("data source=localhost;initial catalog=UserTrainingxDB;persist security info=True;user id=sa;password=nsfinmanapp47865");
            var con = new SqlConnection(connString);
            Stream stream;
            try
            {
                decimal? OB = db.spCashAccountOB(cashAccount, dateFrom).FirstOrDefault();
                if (rptNo == 1)
                {
                    var cmd = new SqlCommand("spCashFlow", con);
                    cmd.Parameters.AddWithValue("@CashAccount", cashAccount);
                    cmd.Parameters.AddWithValue("@FromDate", dateFrom);
                    cmd.Parameters.AddWithValue("@ToDate", dateTo);
                    cmd.Parameters.AddWithValue("@FromAccount", BPFrom);
                    cmd.Parameters.AddWithValue("@ToAccount", BPTo);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(table);
                    }

                    if (BPFrom == "")
                    {
                        BPFrom = "ALL";
                    }
                    if (BPTo == "")
                    {
                        BPTo = "ALL";
                    }

                    CashFlowReport rpt = new CashFlowReport();
                    //rpt.SetDatabaseLogon(UserID, password, server, database, true);
                    //                    rpt.SetDatabaseLogon("sa", "nsfinmanapp47865", "localhost", "UserTrainingxDB", true);
                    rpt.SetDataSource(table);
                    rpt.SetParameterValue("@CashAccount", cashAccount);
                    rpt.SetParameterValue("CashAcc", cashAccount);
                    rpt.SetParameterValue("@FromDate", dateFrom);
                    rpt.SetParameterValue("FDate", dateFrom);
                    rpt.SetParameterValue("@ToDate", dateTo);
                    rpt.SetParameterValue("TDate", dateTo);
                    rpt.SetParameterValue("@FromAccount", BPFrom);
                    rpt.SetParameterValue("FromAcc", BPFrom);
                    rpt.SetParameterValue("@ToAccount", BPTo);
                    rpt.SetParameterValue("ToAcc", BPTo);
                    rpt.SetParameterValue("pmCompany", "Al-meraj Bakers & Sweets");
                    rpt.SetParameterValue("pmOpeningBalance", OB);
                    //rpt.SaveAs("abc.pdf", true);
                    //rpt.Refresh();
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf");
                }
                else if (rptNo == 2)
                {
                    var cmd = new SqlCommand("spCashFlowTrans", con);
                    cmd.Parameters.AddWithValue("@CashAccount", cashAccount);
                    cmd.Parameters.AddWithValue("@FromDate", dateFrom);
                    cmd.Parameters.AddWithValue("@ToDate", dateTo);
                    cmd.Parameters.AddWithValue("@FromAccount", BPFrom);
                    cmd.Parameters.AddWithValue("@ToAccount", BPTo);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(table);
                    }

                    if (BPFrom == "")
                    {
                        BPFrom = "ALL";
                    }
                    if (BPTo == "")
                    {
                        BPTo = "ALL";
                    }

                    CashFlowDetailReport rpt = new CashFlowDetailReport();
                    //rpt.SetDatabaseLogon(UserID, password, server, database, true);
                    //rpt.SetDatabaseLogon("sa", "nsfinmanapp47865", ".", "UserTrainingxDB", true);
                    rpt.SetDataSource(table);
                    rpt.SetParameterValue("@CashAccount", cashAccount);
                    rpt.SetParameterValue("CashAcc", cashAccount);
                    rpt.SetParameterValue("@FromDate", dateFrom);
                    rpt.SetParameterValue("FDate", dateFrom);
                    rpt.SetParameterValue("@ToDate", dateTo);
                    rpt.SetParameterValue("TDate", dateTo);
                    rpt.SetParameterValue("@FromAccount", BPFrom);
                    rpt.SetParameterValue("FromAcc", BPFrom);
                    rpt.SetParameterValue("@ToAccount", BPTo);
                    rpt.SetParameterValue("ToAcc", BPTo);
                    rpt.SetParameterValue("pmCompany", "Al-meraj Bakers & Sweets");
                    rpt.SetParameterValue("pmOpeningBalance", OB);
                    //rpt.SaveAs("abc.pdf", true);
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            stream = Stream.Null;
            return File(stream, "application/pdf");
        }

    }
}