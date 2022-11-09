using crm.portal.Interfaces.MyTab2;
using crm.portal.OscaModel;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.ui.Controllers.Api;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Novacode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalTab2Api")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PortalTab2ApiController : ControllerBase
    {
        private readonly IMyTab2Repository _myTab2Repository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMailService _emailSender;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<PortalTab2ApiController> _log;
        LogException LogException;
        public PortalTab2ApiController(ILogger<PortalTab2ApiController> log, IConfigurationRepository iConfig, IConfigurationRepository configurationRepository,IMailService emailSender, IEmailTemplateRepository emailTemplateRepository,IHostingEnvironment hostingEnvironment, IMyTab2Repository myTab2Repository)
        {

            _myTab2Repository = myTab2Repository;
            _hostingEnvironment = hostingEnvironment;
            _emailTemplateRepository = emailTemplateRepository;
            _emailSender = emailSender;
            _configurationRepository = configurationRepository;
        }
        private int UserId
        {
            get
            {
                var securityUserId = 0;

                if (HttpContext.Session.Keys.Any(a => a.Equals("UserId")))
                {
                    securityUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                }

                return securityUserId;
            }
            set {; }
        }
        private string UserName
        {
            get
            {
                var securityUserName = "";

                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("UserName")))
                {
                    securityUserName = HttpContext.Session.GetString("UserName");
                }

                return securityUserName;
            }
            set {; }
        }
        [HttpGet]
        [Route("GetPortalTab2")]
        public IActionResult GetPortalTab2()
        {
            try
            {
                return Ok(_myTab2Repository.TrainingAssessmentPlanGet(Convert.ToInt32(UserName)));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("DownlaodDoc")]
        [HistoryFilter]
        public IActionResult DownlaodDoc(int tabId)
        {
            try
            {
                var data = _myTab2Repository.TrainingAssessmentPlanGetBy_Id(tabId);


                // return File(data.TAP_FinalFile, "application/pdf", "test");
               // return File(data.TAP_FinalFile, "application/pdf", data.TAP_Id.ToString());

                return File(data.TAP_FinalFile, System.Net.Mime.MediaTypeNames.Application.Octet, data.TAP_FileName);

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("DownlaodPDF")]
        [HistoryFilter]
        public IActionResult DownlaodPDF(int tabId)
        {
            try
            {
                var data = _myTab2Repository.TrainingAssessmentPlanGetBy_Id(tabId);
                var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("LearnerUpload");

                string Pathstr = "\\" + data.TAP_Id_Learner + "\\";
                string new_Filename_on_Server = dbPath + "\\" + Pathstr + data.TAP_Id_Learner + ".pdf";

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + dbPath + Pathstr))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + dbPath + Pathstr);
                }
                if (System.IO.File.Exists(_hostingEnvironment.WebRootPath + new_Filename_on_Server)) { System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server); }

                if (data.TAP_FinalFile != null)
                {
                    using (var imageFile = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                    {
                        imageFile.Write(data.TAP_FinalFile, 0, data.TAP_FinalFile.Length);
                        imageFile.Flush();
                        imageFile.Close();
                    }
                    // ReplacedInWordDoc(sPath);
                }
                string filename = Path.GetFileName(_hostingEnvironment.WebRootPath + new_Filename_on_Server);
                byte[] fileBytes = GetFile(_hostingEnvironment.WebRootPath + new_Filename_on_Server);
                return File(
                    fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                // return File(data.TAP_FinalFile, "application/pdf", "test");
                // return File(data.TAP_FinalFile, "application/pdf");

                //return File(data.TAP_FinalFile, System.Net.Mime.MediaTypeNames.Application.Octet, data.TAP_FileName);

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
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
        [Route("SaveSign")]
        [HistoryFilter]
        public IActionResult SaveSign(Tab tab)
        {
            byte[] bytes = Convert.FromBase64String(tab.TAP_FinalFileStr.Split(',')[1]);
            string sPath = "";
            string sMainTempPath = "C:\\HostingSpaces\\oscahost\\portal.osca.hostedapps.mmsuk.net\\data\\docs\\";

            sPath = sMainTempPath + "/signature_.png";
            string stempPath = sMainTempPath + "/tempsignature_.png";
            if (!Directory.Exists(sMainTempPath))
            {
                System.IO.Directory.CreateDirectory(sMainTempPath);
            }
            if (System.IO.File.Exists(sPath)) { System.IO.File.Delete(sPath); }
            System.IO.File.Copy(stempPath, sPath);
            if (bytes != null)
            {
                using (var imageFile = new FileStream(sPath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                    imageFile.Close();
                }
               // ReplacedInWordDoc(sPath);
            }
            var data = _myTab2Repository.TrainingAssessmentPlanGetBy_Id(tab.TAP_Id);
            Byte[] bytes1 = data.TAP_FinalFile;
            string sMainTempPath1 = "C:\\HostingSpaces\\oscahost\\portal.osca.hostedapps.mmsuk.net\\data\\temp\\";
            if (!Directory.Exists(sMainTempPath1))
            {
                System.IO.Directory.CreateDirectory(sMainTempPath1);
            }
            using (var stream = new FileStream(sMainTempPath + tab.TAP_Id+ ".docx", FileMode.Create, FileAccess.Write, FileShare.Write, 4096, useAsync: true))
            {
                //var bytes = Encoding.UTF8.GetBytes(content);
                 stream.WriteAsync(bytes1, 0, bytes1.Length);
            }
            // System.IO.File.WriteAllBytes(sMainTempPath + "/test" + tab.TAP_Id + ".docx", bytes);
            //using (var stream = new FileStream(sMainTempPath, FileMode.Append))
            //{
            //    stream.Write(bytes, 0, bytes.Length);
            //}
            using (DocX document = DocX.Load(sMainTempPath  + tab.TAP_Id + ".docx"))
            {
                document.ReplaceText("[LEARNERCOMMENT]", tab.TAP_LearnerComments, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                document.ReplaceText("[LERSIGNDATE]", DateTime.Now.ToShortDateString(), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                using (var ms = new MemoryStream())
                {
                    using (var fileStream = new FileStream(sPath, FileMode.Open))
                    {
                        var logo = System.Drawing.Image.FromStream(fileStream);
                        logo.Save(ms, logo.RawFormat);
                        ms.Seek(0, SeekOrigin.Begin);
                    }
                    var myimg = document.AddImage(ms);
                    var pic1 = myimg.CreatePicture();
                    var p = document.InsertParagraph();

                    Novacode.Table placeholderTable = document.Tables[5];
                    placeholderTable.Rows[0].Cells[1].Paragraphs.First().InsertPicture(pic1);
                }
                // Save changes made to this document.
                document.Save();
            }
            if (System.IO.File.Exists(sPath)) { System.IO.File.Delete(sPath); }
            byte[] fileBytes;

            string strPath = sMainTempPath + tab.TAP_Id + ".docx";
            using (FileStream fileStream = new FileStream(strPath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    fileBytes = reader.ReadBytes((int)fileStream.Length);
                }
            }
            Hashtable ht = new Hashtable();
            ht.Add("[LEARNERNAME]", UserName);
            ht.Add("[TAPID]",tab.TAP_Id);
            ht.Add("[COURSEID]", data.TAP_Id_LearnerCourse);
            Tuple<string, string> tuple = _emailTemplateRepository.GetSubjectAndEmailTemplateBodyReplacedByCode("LERTAP001", ht);
           // appInvite.api_emailbody = tuple.Item2;
            tab.TAP_FinalFile = fileBytes;
            _myTab2Repository.SaveSignatur(tab);
           // _emailSender.SendEmail()
            return Ok(true);
        }
    }
}
