using ds.core.configuration;
using Microsoft.AspNetCore.Mvc;
using portal.data.repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ds.core.document;
using crm.portal.Interfaces.Matrix;
using System;
using crm.portal.Interfaces.User;
using CoreHtmlToImage;
using System.Threading;
using System.Drawing;
using Spire.Pdf.HtmlConverter;
using Spire.Pdf;
using IronPdf;
using ds.portal.ui.Models;

namespace ds.portal.ui.Controllers
{
    public class DocumentController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMemoryCache _cache;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDocument _document;
        private readonly IMatrixRepository _matrixRepository;
        private readonly IPortalUserRepository _portalUserRepository;

        public DocumentController(IPortalUserRepository portalUserRepository, IMatrixRepository matrixRepository,IDocument document,IHostingEnvironment hostingEnvironment,IRoleAdminRepository _roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(_roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _configurationRepository = configurationRepository;
            _hostingEnvironment = hostingEnvironment;
            _document = document;
            _matrixRepository = matrixRepository;
            _portalUserRepository = portalUserRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Feature = "Document Management";
            ViewBag.Page = "Manage";

            SetProductDetailToViewBag();

            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");

            return View();
        }
        public IActionResult MultipleUploadWithMetaData()
        {
            ViewBag.Feature = "Document Management";
            ViewBag.Page = "Multiple File Upload";
            return View();
        }
        public ActionResult DownloadCourseContentFile(string filePath)
        {
            var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("CourseContent");
            var newpath = dbPath + filePath;
            string path = _document.DownlaodFileFromPath(newpath);
            string filename = Path.GetFileName(path);
            byte[] fileBytes = GetFile(path);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        public ActionResult DownloadLearnerUploadFile(string filePath)
        {
            var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("LearnerUpload");
            var newpath = dbPath + filePath;
            string path = _document.DownlaodFileFromPath(newpath);
            string filename = Path.GetFileName(path);
            byte[] fileBytes = GetFile(path);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        public ActionResult DownloadLocalFile(string filePath)
        {
            string filename = Path.GetFileName(filePath);
            byte[] fileBytes = GetFile(filePath);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        public ActionResult DownloadFile(string filePath)
        {
            string filename = Path.GetFileName(filePath);
            byte[] fileBytes = GetFile(filePath);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        [HttpGet]
        public string DownloadCompleteSummary()
        {
            byte[] bytes;
            var data1 = _matrixRepository.LoadLearnerCourse(Convert.ToInt32(GetSecurityUserName));

            var learnerId = Convert.ToInt32(GetSecurityUserName);
            var learnerName = _portalUserRepository.GetLearnerNameSurname(learnerId.ToString());
            var courseId = data1.LearnerCourses_id;
            var course = data1.Course_Title;
            var data = _matrixRepository.GetMatrixLabels(courseId);
            string sBody = "";
            if (data != null)
            {
                 sBody += "<body style='font-family:arial;font-size:15px;'>";
                string strRow = " <table border=0 style='font-family:arial;padding: 5px 5px 5px 5px;background-color: #E0EEEE;font-size:14px;border:1px solid #c3c3c3' cellspacing=0 cellpadding=2 width=\"100%\" align=left>";
                strRow += " <tbody>";
                strRow += "<tr><td><h2 style='text-align:center;font-size: 41px;margin-bottom: 0;'>Learner Matrix</h2></td></tr>";
                strRow += "<tr><td style='height: 20px;'></td></tr>";
                strRow += "<tr><td style='font-weight: bold;font-size: 20px;'>Learner Name: " + learnerName + " </td></tr>";
                strRow += "<tr><td style='font-weight: bold;font-size: 20px;'>Learner Course: " + course + " </td></tr>";
                strRow += "<tr><td style='height: 20px;'></td></tr>";
                foreach (var item in data)
                {
                    strRow += "<tr>";
                    strRow += "<td><h2 class='units' style='background: lightblue;color: #000;padding: 10px;margin: 0px;margin-top: 15px;'>" + item.SSMB_Title + "</h2></td></tr><tr><td>";
                    var listItem = _matrixRepository.LoadMatrix(courseId, item.SSMB_Id);
                    strRow += "<tr><td>";
                    if (listItem != null)
                    {
                        strRow += " <table id=matrix_tbl_" + item.SSMB_Id.ToString() + " border=0 style='font-family:arial;font-size:14px;background: #fff;' cellspacing=0 cellpadding=2 width=\"100%\" align=left>";
                        strRow += " <tbody>";
                        foreach (var item1 in listItem)
                        {
                            strRow += "<tr><td>";
                            strRow += "<Label class='topics' style='font-size: 16px;padding: 0 10px;display: inline-block;margin-top: 16px;' ><b> " + item1.SSM_Name + "</b></Label></td></tr>";
                            // var listFullSummary = _matrixRepository.LoadMatrixFullSummary(topicid, Convert.ToInt32(UserName), matrixId));
                            var aa = _matrixRepository.MatrixDetail(courseId, item1.SSM_Id, item.SSMB_Id);
                            strRow += "<tr><td width=''>";
                            foreach (var a in aa)
                            {
                                strRow += "<span class='section' style='display:inline-block; margin:5px;>' ";
                                strRow += "<label>" + a.SSML_Id_LevelKey.ToString() + "." + a.SSML_Id_LevelValue.ToString() + "<label>";
                                if (a.SSML_IsCompleted.ToString() == "1")
                                {

                                    strRow += " <input type='checkbox' checked='checked' /> ";
                                }
                                else
                                {
                                    strRow += "<input type='checkbox' /> ";
                                }

                                strRow += "</span>";
                            }
                            strRow += "</td></tr>";
                        }
                        strRow += "</tbody></table>";
                    }
                    strRow += "</td></tr>";
                }
                strRow += "</tbody></table>";
                sBody += strRow + "</body>";

                var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("LearnerUpload");

                string Pathstr = "\\" + learnerId + "\\";
                string new_Filename_on_Server = dbPath + "\\" + Pathstr + "-Matrix.pdf";

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + dbPath + Pathstr))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + dbPath + Pathstr);
                }

                //var Renderer = new ChromePdfRenderer();
                //var PDF = Renderer.RenderHtmlAsPdf(sBody, _hostingEnvironment.WebRootPath + dbPath + Pathstr);
                //var Renderer = new IronPdf.ChromePdfRenderer();
                //Renderer.RenderingOptions.FitToPaper = true;
                //Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;
                //Renderer.RenderingOptions.PrintHtmlBackgrounds = true;
                //Renderer.RenderingOptions.CreatePdfFormsFromHtml = true;
                //var PDF = Renderer.RenderHtmlAsPdf("<h1>Hello IronPdf</h1>");
                //var Renderer = new IronPdf.HtmlToPdf();
                //var PDF = Renderer.RenderHtmlAsPdf(sBody);
                //var OutputPath = "HtmlToPDF.pdf";
                //PDF.SaveAs(OutputPath);
               // var OutputPath = "pixel-perfect.pdf";
                //PDF.SaveAs(OutputPath);
                //var Render_htm = new IronPdf.HtmlTpPdf();
                //var Rendered_pdf = Render.RenderHtmAsPdf(sBody);
                //var Output_Path = “My_First_Html.pdf”;
                //PDF.SaveAs("TestPDF"); 
                // PdfDocument pdf = new PdfDocument();
                //Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                //PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();

                // Spire.Pdf.HtmlConverter.PdfHtmlLayoutFormat htmlLayoutFormat = new Spire.Pdf.HtmlConverter.PdfHtmlLayoutFormat();
                //webBrowser load html whether Waiting
                // htmlLayoutFormat.IsWaiting = false;
                //htmlLayoutFormat.FitToPage = Spire.Pdf.HtmlConverter.Clip.Width;            
                //page setting
                //Spire.Pdf.PdfPageSettings setting = new Spire.Pdf.PdfPageSettings();
                //setting.Size = Spire.Pdf.PdfPageSize.A4;
                //setting.Orientation = Spire.Pdf.PdfPageOrientation.Portrait;

                //Thread thread = new Thread(() =>
                //{ pdf.LoadFromHTML(sBody, false, setting, htmlLayoutFormat); });
                //thread.SetApartmentState(ApartmentState.STA);
                //thread.Start();
                //thread.Join();
                //string outputFile = "ToPDF.pdf";

               


                //  string htmlCode = invFinalInvoiceInitial.Body;

                //if (System.IO.File.Exists(dbPath + new_Filename_on_Server))
                //{
                //    System.IO.File.Delete(dbPath + new_Filename_on_Server);
                //}
                //if (!System.IO.File.Exists(dbPath + new_Filename_on_Server))
                //{
                //  if (System.IO.File.Exists(_hostingEnvironment.WebRootPath +dbPath + Pathstr)) { System.IO.File.Delete(dbPath + Pathstr); }
                //  System.IO.File.Copy(stempPath, sPath);
                // var converter = new HtmlConverter();
                //byte[] pdfBytes = converter.FromHtmlString(sBody);
                // var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(sBody);

                //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                //if (pdfBytes != null)
                //{
                //    using (var imageFile = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                //    {
                //        imageFile.Write(pdfBytes, 0, pdfBytes.Length);
                //        imageFile.Flush();
                //        imageFile.Close();
                //    }
                //    // ReplacedInWordDoc(sPath);
                //}
                //using (FileStream stream = new FileStream(dbPath + new_Filename_on_Server, FileMode.Create))
                //{
                //    uploaded_file.CopyTo(stream);
                //    stream.Flush();
                //}
                // }
            }

            return sBody;
        }  
        [HttpGet]
        public string DownloadCompleteSummaryForAccount(string id)
        {
            string sBody = "";
            //string learnerId = "0";
            //if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            //{
            //learnerId1 = HttpContext.Request.Query["id"];
            var ids = Encryption.Decrypt(id);
                if (ids == "")
                {
                    return sBody="Please Provide Learner Id";
                }
                //learnerId = id;
           // }
            byte[] bytes;
            var data1 = _matrixRepository.LoadLearnerCourse(Convert.ToInt32(ids));

            var learnerId = Convert.ToInt32(Convert.ToInt32(ids));
            var learnerName = _portalUserRepository.GetLearnerNameSurname(learnerId.ToString());
            var courseId = data1.LearnerCourses_id;
            var course = data1.Course_Title;
            var data = _matrixRepository.GetMatrixLabels(courseId);
          
            if (data != null)
            {
                 sBody += "<body style='font-family:arial;font-size:15px;'>";
                string strRow = " <table border=0 style='font-family:arial;padding: 5px 5px 5px 5px;background-color: #E0EEEE;font-size:14px;border:1px solid #c3c3c3' cellspacing=0 cellpadding=2 width=\"100%\" align=left>";
                strRow += " <tbody>";
                strRow += "<tr><td><h2 style='text-align:center;font-size: 41px;margin-bottom: 0;'>Learner Matrix</h2></td></tr>";
                strRow += "<tr><td style='height: 20px;'></td></tr>";
                strRow += "<tr><td style='font-weight: bold;font-size: 20px;'>Learner Name: " + learnerName + " </td></tr>";
                strRow += "<tr><td style='font-weight: bold;font-size: 20px;'>Learner Course: " + course + " </td></tr>";
                strRow += "<tr><td style='height: 20px;'></td></tr>";
                foreach (var item in data)
                {
                    strRow += "<tr>";
                    strRow += "<td><h2 class='units' style='background: lightblue;color: #000;padding: 10px;margin: 0px;margin-top: 15px;'>" + item.SSMB_Title + "</h2></td></tr><tr><td>";
                    var listItem = _matrixRepository.LoadMatrix(courseId, item.SSMB_Id);
                    strRow += "<tr><td>";
                    if (listItem != null)
                    {
                        strRow += " <table id=matrix_tbl_" + item.SSMB_Id.ToString() + " border=0 style='font-family:arial;font-size:14px;background: #fff;' cellspacing=0 cellpadding=2 width=\"100%\" align=left>";
                        strRow += " <tbody>";
                        foreach (var item1 in listItem)
                        {
                            strRow += "<tr><td>";
                            strRow += "<Label class='topics' style='font-size: 16px;padding: 0 10px;display: inline-block;margin-top: 16px;' ><b> " + item1.SSM_Name + "</b></Label></td></tr>";
                            // var listFullSummary = _matrixRepository.LoadMatrixFullSummary(topicid, Convert.ToInt32(UserName), matrixId));
                            var aa = _matrixRepository.MatrixDetail(courseId, item1.SSM_Id, item.SSMB_Id);
                            strRow += "<tr><td width=''>";
                            foreach (var a in aa)
                            {
                                strRow += "<span class='section' style='display:inline-block; margin:5px;>' ";
                                strRow += "<label>" + a.SSML_Id_LevelKey.ToString() + "." + a.SSML_Id_LevelValue.ToString() + "<label>";
                                if (a.SSML_IsCompleted.ToString() == "1")
                                {

                                    strRow += " <input type='checkbox' checked='checked' /> ";
                                }
                                else
                                {
                                    strRow += "<input type='checkbox' /> ";
                                }

                                strRow += "</span>";
                            }
                            strRow += "</td></tr>";
                        }
                        strRow += "</tbody></table>";
                    }
                    strRow += "</td></tr>";
                }
                strRow += "</tbody></table>";
                sBody += strRow + "</body>";

                var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("LearnerUpload");

                string Pathstr = "\\" + learnerId + "\\";
                string new_Filename_on_Server = dbPath + "\\" + Pathstr + "-Matrix.pdf";

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + dbPath + Pathstr))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + dbPath + Pathstr);
                }

                //var Renderer = new ChromePdfRenderer();
                //var PDF = Renderer.RenderHtmlAsPdf(sBody, _hostingEnvironment.WebRootPath + dbPath + Pathstr);
                //var Renderer = new IronPdf.ChromePdfRenderer();
                //Renderer.RenderingOptions.FitToPaper = true;
                //Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;
                //Renderer.RenderingOptions.PrintHtmlBackgrounds = true;
                //Renderer.RenderingOptions.CreatePdfFormsFromHtml = true;
                //var PDF = Renderer.RenderHtmlAsPdf("<h1>Hello IronPdf</h1>");
                //var Renderer = new IronPdf.HtmlToPdf();
                //var PDF = Renderer.RenderHtmlAsPdf(sBody);
                //var OutputPath = "HtmlToPDF.pdf";
                //PDF.SaveAs(OutputPath);
               // var OutputPath = "pixel-perfect.pdf";
                //PDF.SaveAs(OutputPath);
                //var Render_htm = new IronPdf.HtmlTpPdf();
                //var Rendered_pdf = Render.RenderHtmAsPdf(sBody);
                //var Output_Path = “My_First_Html.pdf”;
                //PDF.SaveAs("TestPDF"); 
                // PdfDocument pdf = new PdfDocument();
                //Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                //PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();

                // Spire.Pdf.HtmlConverter.PdfHtmlLayoutFormat htmlLayoutFormat = new Spire.Pdf.HtmlConverter.PdfHtmlLayoutFormat();
                //webBrowser load html whether Waiting
                // htmlLayoutFormat.IsWaiting = false;
                //htmlLayoutFormat.FitToPage = Spire.Pdf.HtmlConverter.Clip.Width;            
                //page setting
                //Spire.Pdf.PdfPageSettings setting = new Spire.Pdf.PdfPageSettings();
                //setting.Size = Spire.Pdf.PdfPageSize.A4;
                //setting.Orientation = Spire.Pdf.PdfPageOrientation.Portrait;

                //Thread thread = new Thread(() =>
                //{ pdf.LoadFromHTML(sBody, false, setting, htmlLayoutFormat); });
                //thread.SetApartmentState(ApartmentState.STA);
                //thread.Start();
                //thread.Join();
                //string outputFile = "ToPDF.pdf";

               


                //  string htmlCode = invFinalInvoiceInitial.Body;

                //if (System.IO.File.Exists(dbPath + new_Filename_on_Server))
                //{
                //    System.IO.File.Delete(dbPath + new_Filename_on_Server);
                //}
                //if (!System.IO.File.Exists(dbPath + new_Filename_on_Server))
                //{
                //  if (System.IO.File.Exists(_hostingEnvironment.WebRootPath +dbPath + Pathstr)) { System.IO.File.Delete(dbPath + Pathstr); }
                //  System.IO.File.Copy(stempPath, sPath);
                // var converter = new HtmlConverter();
                //byte[] pdfBytes = converter.FromHtmlString(sBody);
                // var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(sBody);

                //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                //if (pdfBytes != null)
                //{
                //    using (var imageFile = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                //    {
                //        imageFile.Write(pdfBytes, 0, pdfBytes.Length);
                //        imageFile.Flush();
                //        imageFile.Close();
                //    }
                //    // ReplacedInWordDoc(sPath);
                //}
                //using (FileStream stream = new FileStream(dbPath + new_Filename_on_Server, FileMode.Create))
                //{
                //    uploaded_file.CopyTo(stream);
                //    stream.Flush();
                //}
                // }
            }

            return sBody;
        }
        [HttpGet]
        public string ViewCompleteSummary()
        {
            byte[] bytes;
            var data1 = _matrixRepository.LoadLearnerCourse(Convert.ToInt32(GetSecurityUserName));

            var learnerId = Convert.ToInt32(GetSecurityUserName);
            var learnerName = _portalUserRepository.GetLearnerNameSurname(learnerId.ToString());
            var courseId = data1.LearnerCourses_id;
            var course = data1.Course_Title;
            var data = _matrixRepository.GetMatrixLabels(courseId);
            string sBody = "";
            if (data != null)
            {
                sBody += "<body style='font-family:arial;font-size:15px;'>";
                string strRow = " <table border=0 style='font-family:arial;padding: 5px 5px 5px 5px;background-color: #E0EEEE;font-size:14px;border:1px solid #c3c3c3' cellspacing=0 cellpadding=2 width=\"100%\" align=left>";
                strRow += " <tbody>";
                strRow += "<tr><td><h2 style='text-align:center;font-size: 41px;margin-bottom: 0;'>Learner Matrix</h2></td></tr>";
                strRow += "<tr><td style='height: 20px;'></td></tr>";
                strRow += "<tr><td style='font-weight: bold;font-size: 20px;'>Learner Name: " + learnerName + " </td></tr>";
                strRow += "<tr><td style='font-weight: bold;font-size: 20px;'>Learner Course: " + course + " </td></tr>";
                strRow += "<tr><td style='height: 20px;'></td></tr>";
                foreach (var item in data)
                {
                    strRow += "<tr>";
                    strRow += "<td><h2 class='units' style='background: lightblue;color: #000;padding: 10px;margin: 0px;margin-top: 15px;'>" + item.SSMB_Title + "</h2></td></tr><tr><td>";
                    var listItem = _matrixRepository.LoadMatrix(courseId, item.SSMB_Id);
                    strRow += "<tr><td>";
                    if (listItem != null)
                    {
                        strRow += " <table id=matrix_tbl_" + item.SSMB_Id.ToString() + " border=0 style='font-family:arial;font-size:14px;background: #fff;' cellspacing=0 cellpadding=2 width=\"100%\" align=left>";
                        strRow += " <tbody>";
                        foreach (var item1 in listItem)
                        {
                            strRow += "<tr><td>";
                            strRow += "<Label class='topics' style='font-size: 16px;padding: 0 10px;display: inline-block;margin-top: 16px;' ><b> " + item1.SSM_Name + "</b></Label></td></tr>";
                            // var listFullSummary = _matrixRepository.LoadMatrixFullSummary(topicid, Convert.ToInt32(UserName), matrixId));
                            var aa = _matrixRepository.MatrixDetail(courseId, item1.SSM_Id, item.SSMB_Id);
                            strRow += "<tr><td width=''>";
                            foreach (var a in aa)
                            {
                                strRow += "<span class='section' style='display:inline-block; margin:5px;>' ";
                                strRow += "<label>" + a.SSML_Id_LevelKey.ToString() + "." + a.SSML_Id_LevelValue.ToString() + "<label>";
                                if (a.SSML_IsCompleted.ToString() == "1")
                                {

                                    strRow += " <input type='checkbox' checked='checked' /> ";
                                }
                                else
                                {
                                    strRow += "<input type='checkbox' /> ";
                                }

                                strRow += "</span>";
                            }
                            strRow += "</td></tr>";
                        }
                        strRow += "</tbody></table>";
                    }
                    strRow += "</td></tr>";
                }
                strRow += "</tbody></table>";
                sBody += strRow + "</body>";

                var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("LearnerUpload");

                string Pathstr = "\\" + learnerId + "\\";
                string new_Filename_on_Server = dbPath + "\\" + Pathstr + "-Matrix.pdf";

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + dbPath + Pathstr))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + dbPath + Pathstr);
                }

                //var Renderer = new ChromePdfRenderer();
                //var PDF = Renderer.RenderHtmlAsPdf(sBody, _hostingEnvironment.WebRootPath + dbPath + Pathstr);
                //var Renderer = new IronPdf.ChromePdfRenderer();
                //Renderer.RenderingOptions.FitToPaper = true;
                //Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;
                //Renderer.RenderingOptions.PrintHtmlBackgrounds = true;
                //Renderer.RenderingOptions.CreatePdfFormsFromHtml = true;
                //var PDF = Renderer.RenderHtmlAsPdf("<h1>Hello IronPdf</h1>");
                //var Renderer = new IronPdf.HtmlToPdf();
                //var PDF = Renderer.RenderHtmlAsPdf(sBody);
                //var OutputPath = "HtmlToPDF.pdf";
                //PDF.SaveAs(OutputPath);
                // var OutputPath = "pixel-perfect.pdf";
                //PDF.SaveAs(OutputPath);
                //var Render_htm = new IronPdf.HtmlTpPdf();
                //var Rendered_pdf = Render.RenderHtmAsPdf(sBody);
                //var Output_Path = “My_First_Html.pdf”;
                //PDF.SaveAs("TestPDF"); 
                // PdfDocument pdf = new PdfDocument();
                //Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                //PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();

                // Spire.Pdf.HtmlConverter.PdfHtmlLayoutFormat htmlLayoutFormat = new Spire.Pdf.HtmlConverter.PdfHtmlLayoutFormat();
                //webBrowser load html whether Waiting
                // htmlLayoutFormat.IsWaiting = false;
                //htmlLayoutFormat.FitToPage = Spire.Pdf.HtmlConverter.Clip.Width;            
                //page setting
                //Spire.Pdf.PdfPageSettings setting = new Spire.Pdf.PdfPageSettings();
                //setting.Size = Spire.Pdf.PdfPageSize.A4;
                //setting.Orientation = Spire.Pdf.PdfPageOrientation.Portrait;

                //Thread thread = new Thread(() =>
                //{ pdf.LoadFromHTML(sBody, false, setting, htmlLayoutFormat); });
                //thread.SetApartmentState(ApartmentState.STA);
                //thread.Start();
                //thread.Join();
                //string outputFile = "ToPDF.pdf";




                //  string htmlCode = invFinalInvoiceInitial.Body;

                //if (System.IO.File.Exists(dbPath + new_Filename_on_Server))
                //{
                //    System.IO.File.Delete(dbPath + new_Filename_on_Server);
                //}
                //if (!System.IO.File.Exists(dbPath + new_Filename_on_Server))
                //{
                //  if (System.IO.File.Exists(_hostingEnvironment.WebRootPath +dbPath + Pathstr)) { System.IO.File.Delete(dbPath + Pathstr); }
                //  System.IO.File.Copy(stempPath, sPath);
                // var converter = new HtmlConverter();
                //byte[] pdfBytes = converter.FromHtmlString(sBody);
                // var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(sBody);

                //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                //if (pdfBytes != null)
                //{
                //    using (var imageFile = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                //    {
                //        imageFile.Write(pdfBytes, 0, pdfBytes.Length);
                //        imageFile.Flush();
                //        imageFile.Close();
                //    }
                //    // ReplacedInWordDoc(sPath);
                //}
                //using (FileStream stream = new FileStream(dbPath + new_Filename_on_Server, FileMode.Create))
                //{
                //    uploaded_file.CopyTo(stream);
                //    stream.Flush();
                //}
                // }
            }

            return sBody;
        }   
        [HttpGet]
        public string ViewCompleteSummaryForAccount(int id)
        {
            byte[] bytes;
            var data1 = _matrixRepository.LoadLearnerCourse(id);

            var learnerId = Convert.ToInt32(id);
            var learnerName = _portalUserRepository.GetLearnerNameSurname(learnerId.ToString());
            var courseId = data1.LearnerCourses_id;
            var course = data1.Course_Title;
            var data = _matrixRepository.GetMatrixLabels(courseId);
            string sBody = "";
            if (data != null)
            {
                sBody += "<body style='font-family:arial;font-size:15px;'>";
                string strRow = " <table border=0 style='font-family:arial;padding: 5px 5px 5px 5px;background-color: #E0EEEE;font-size:14px;border:1px solid #c3c3c3' cellspacing=0 cellpadding=2 width=\"100%\" align=left>";
                strRow += " <tbody>";
                strRow += "<tr><td><h2 style='text-align:center;font-size: 41px;margin-bottom: 0;'>Learner Matrix</h2></td></tr>";
                strRow += "<tr><td style='height: 20px;'></td></tr>";
                strRow += "<tr><td style='font-weight: bold;font-size: 20px;'>Learner Name: " + learnerName + " </td></tr>";
                strRow += "<tr><td style='font-weight: bold;font-size: 20px;'>Learner Course: " + course + " </td></tr>";
                strRow += "<tr><td style='height: 20px;'></td></tr>";
                foreach (var item in data)
                {
                    strRow += "<tr>";
                    strRow += "<td><h2 class='units' style='background: lightblue;color: #000;padding: 10px;margin: 0px;margin-top: 15px;'>" + item.SSMB_Title + "</h2></td></tr><tr><td>";
                    var listItem = _matrixRepository.LoadMatrix(courseId, item.SSMB_Id);
                    strRow += "<tr><td>";
                    if (listItem != null)
                    {
                        strRow += " <table id=matrix_tbl_" + item.SSMB_Id.ToString() + " border=0 style='font-family:arial;font-size:14px;background: #fff;' cellspacing=0 cellpadding=2 width=\"100%\" align=left>";
                        strRow += " <tbody>";
                        foreach (var item1 in listItem)
                        {
                            strRow += "<tr><td>";
                            strRow += "<Label class='topics' style='font-size: 16px;padding: 0 10px;display: inline-block;margin-top: 16px;' ><b> " + item1.SSM_Name + "</b></Label></td></tr>";
                            // var listFullSummary = _matrixRepository.LoadMatrixFullSummary(topicid, Convert.ToInt32(UserName), matrixId));
                            var aa = _matrixRepository.MatrixDetail(courseId, item1.SSM_Id, item.SSMB_Id);
                            strRow += "<tr><td width=''>";
                            foreach (var a in aa)
                            {
                                strRow += "<span class='section' style='display:inline-block; margin:5px;>' ";
                                strRow += "<label>" + a.SSML_Id_LevelKey.ToString() + "." + a.SSML_Id_LevelValue.ToString() + "<label>";
                                if (a.SSML_IsCompleted.ToString() == "1")
                                {

                                    strRow += " <input type='checkbox' checked='checked' /> ";
                                }
                                else
                                {
                                    strRow += "<input type='checkbox' /> ";
                                }

                                strRow += "</span>";
                            }
                            strRow += "</td></tr>";
                        }
                        strRow += "</tbody></table>";
                    }
                    strRow += "</td></tr>";
                }
                strRow += "</tbody></table>";
                sBody += strRow + "</body>";

                var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("LearnerUpload");

                string Pathstr = "\\" + learnerId + "\\";
                string new_Filename_on_Server = dbPath + "\\" + Pathstr + "-Matrix.pdf";

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + dbPath + Pathstr))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + dbPath + Pathstr);
                }

                //var Renderer = new ChromePdfRenderer();
                //var PDF = Renderer.RenderHtmlAsPdf(sBody, _hostingEnvironment.WebRootPath + dbPath + Pathstr);
                //var Renderer = new IronPdf.ChromePdfRenderer();
                //Renderer.RenderingOptions.FitToPaper = true;
                //Renderer.RenderingOptions.CssMediaType = IronPdf.Rendering.PdfCssMediaType.Screen;
                //Renderer.RenderingOptions.PrintHtmlBackgrounds = true;
                //Renderer.RenderingOptions.CreatePdfFormsFromHtml = true;
                //var PDF = Renderer.RenderHtmlAsPdf("<h1>Hello IronPdf</h1>");
                //var Renderer = new IronPdf.HtmlToPdf();
                //var PDF = Renderer.RenderHtmlAsPdf(sBody);
                //var OutputPath = "HtmlToPDF.pdf";
                //PDF.SaveAs(OutputPath);
                // var OutputPath = "pixel-perfect.pdf";
                //PDF.SaveAs(OutputPath);
                //var Render_htm = new IronPdf.HtmlTpPdf();
                //var Rendered_pdf = Render.RenderHtmAsPdf(sBody);
                //var Output_Path = “My_First_Html.pdf”;
                //PDF.SaveAs("TestPDF"); 
                // PdfDocument pdf = new PdfDocument();
                //Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                //PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();

                // Spire.Pdf.HtmlConverter.PdfHtmlLayoutFormat htmlLayoutFormat = new Spire.Pdf.HtmlConverter.PdfHtmlLayoutFormat();
                //webBrowser load html whether Waiting
                // htmlLayoutFormat.IsWaiting = false;
                //htmlLayoutFormat.FitToPage = Spire.Pdf.HtmlConverter.Clip.Width;            
                //page setting
                //Spire.Pdf.PdfPageSettings setting = new Spire.Pdf.PdfPageSettings();
                //setting.Size = Spire.Pdf.PdfPageSize.A4;
                //setting.Orientation = Spire.Pdf.PdfPageOrientation.Portrait;

                //Thread thread = new Thread(() =>
                //{ pdf.LoadFromHTML(sBody, false, setting, htmlLayoutFormat); });
                //thread.SetApartmentState(ApartmentState.STA);
                //thread.Start();
                //thread.Join();
                //string outputFile = "ToPDF.pdf";




                //  string htmlCode = invFinalInvoiceInitial.Body;

                //if (System.IO.File.Exists(dbPath + new_Filename_on_Server))
                //{
                //    System.IO.File.Delete(dbPath + new_Filename_on_Server);
                //}
                //if (!System.IO.File.Exists(dbPath + new_Filename_on_Server))
                //{
                //  if (System.IO.File.Exists(_hostingEnvironment.WebRootPath +dbPath + Pathstr)) { System.IO.File.Delete(dbPath + Pathstr); }
                //  System.IO.File.Copy(stempPath, sPath);
                // var converter = new HtmlConverter();
                //byte[] pdfBytes = converter.FromHtmlString(sBody);
                // var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(sBody);

                //var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();

                //if (pdfBytes != null)
                //{
                //    using (var imageFile = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                //    {
                //        imageFile.Write(pdfBytes, 0, pdfBytes.Length);
                //        imageFile.Flush();
                //        imageFile.Close();
                //    }
                //    // ReplacedInWordDoc(sPath);
                //}
                //using (FileStream stream = new FileStream(dbPath + new_Filename_on_Server, FileMode.Create))
                //{
                //    uploaded_file.CopyTo(stream);
                //    stream.Flush();
                //}
                // }
            }

            return sBody;
        }
    }
}