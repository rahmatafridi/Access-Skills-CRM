using Dapper;
using ds.core.configuration;
using ds.core.document.Models;
using ds.core.uow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ds.core.document
{
    public class Document : IDocument
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfigurationRepository _configurationRepository;
        public Document(IConfigurationRepository configurationRepository, IUOW unitOfWork, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _connString = unitOfWork.GetConnectionString();
            _configurationRepository = configurationRepository;
        }
        public List<documentModel> Upload(dynamic document, int Id, HttpRequest Request, out long uploaded_size, out int iCounter, out string sFiles_uploaded)
        {
            List<documentModel> documentModels = new List<documentModel>();
            uploaded_size = 0;
            string path_for_Uploaded_Files = "\\Assets\\Temp\\" + Id + "\\";
            var uploaded_files = Request.Form.Files;
            iCounter = 0;
            sFiles_uploaded = "";
            foreach (var uploaded_file in uploaded_files)
            {
                iCounter++;
                uploaded_size += uploaded_file.Length;
                sFiles_uploaded += "\n" + uploaded_file.FileName;

                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string uploaded_Filename = unixTimestamp + "_" + uploaded_file.FileName.Replace(" ", "").Trim();
                string new_Filename_on_Server = path_for_Uploaded_Files + "\\" + uploaded_Filename;

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files);
                }

                using (FileStream stream = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                {
                    uploaded_file.CopyTo(stream);
                    stream.Flush();
                }

                byte[] fileData = null;
                using (FileStream fs = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    var binaryReader = new BinaryReader(fs);
                    fileData = binaryReader.ReadBytes((int)fs.Length);

                    documentModels.Add(new documentModel
                    {
                        document_file_name = uploaded_file.FileName,
                        document_file_extension = Path.GetExtension(uploaded_file.FileName),
                        document_mime_type = uploaded_file.ContentType,
                        document_object = fileData,
                        document_size = uploaded_file.Length,
                    });
                }
                System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server);

            }

            return documentModels;
        }
        public List<documentModel> UploadCourseContent(dynamic document, int Id, HttpRequest Request, out long uploaded_size, out int iCounter, out string sFiles_uploaded)
        {
            List<documentModel> documentModels = new List<documentModel>();
            uploaded_size = 0;
            var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("CourseContent");
            string path_for_Uploaded_Files = dbPath ;
            var uploaded_files = Request.Form.Files;
            iCounter = 0;
            sFiles_uploaded = "";
            foreach (var uploaded_file in uploaded_files)
            {
                iCounter++;
                uploaded_size += uploaded_file.Length;
                sFiles_uploaded += "\n" + uploaded_file.FileName;

                //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string uploaded_Filename = uploaded_file.FileName;
                string Pathstr = "\\"  + Id + "\\";
                string dbstr = Pathstr + uploaded_Filename;
                string new_Filename_on_Server = path_for_Uploaded_Files + "\\" + Pathstr + uploaded_Filename;

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files + Pathstr))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files+ Pathstr);
                }

               
                if (System.IO.File.Exists(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server);
                }
                if (!System.IO.File.Exists(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    using (FileStream stream = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                    {
                        uploaded_file.CopyTo(stream);
                        stream.Flush();
                    }
                }
                byte[] fileData = null;
                using (FileStream fs = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    var binaryReader = new BinaryReader(fs);
                    fileData = binaryReader.ReadBytes((int)fs.Length);

                    documentModels.Add(new documentModel
                    {
                        document_file_name = uploaded_file.FileName,
                        document_file_extension = Path.GetExtension(uploaded_file.FileName),
                        document_mime_type = uploaded_file.ContentType,
                        document_object = fileData,
                        document_size = uploaded_file.Length,
                        file_path = dbstr
                    });
                }
              //  System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server);

            }

            return documentModels;
        }

        public documentModel DownloadFile(int documentId, int moduleType)
        {
            documentModel document = new documentModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Document_GetDocumentByID]";
                    /// document module 1 for lead document.
                    object dynamicParams = new { @document_Id = documentId, @document_module = moduleType };
                    document = conn.QuerySingleOrDefault<documentModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return document;
        }

        public bool DeleteFile(string path)
        {
            bool result = false;
            if (System.IO.File.Exists(_hostingEnvironment.WebRootPath + path))
            {
                System.IO.File.Delete(_hostingEnvironment.WebRootPath + path);
                result = true;
            }
            return result;
        }
        public bool DeleteFileCourseContent(string path)
        {
            bool result = false;
            if (System.IO.File.Exists(_hostingEnvironment.WebRootPath + path))
            {
                System.IO.File.Delete(_hostingEnvironment.WebRootPath + path);
                result = true;
            }
            return result;
        }
        public bool DeleteFileUpload(string path)
        {
            bool result = false;
            if (System.IO.File.Exists(_hostingEnvironment.WebRootPath + path))
            {
                System.IO.File.Delete(_hostingEnvironment.WebRootPath + path);
                result = true;
            }
            return result;
        }
        public string DownlaodFileFromPath(string path)
        {
            return _hostingEnvironment.WebRootPath + path;
        }

        public List<documentModel> UploadResource(dynamic document, int Id, HttpRequest Request, out long uploaded_size, out int iCounter, out string sFiles_uploaded)
        {
            List<documentModel> documentModels = new List<documentModel>();
            uploaded_size = 0;
            var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("LearnerUpload");
            string path_for_Uploaded_Files = dbPath;
            var uploaded_files = Request.Form.Files;
            iCounter = 0;
            sFiles_uploaded = "";
            foreach (var uploaded_file in uploaded_files)
            {
                iCounter++;
                uploaded_size += uploaded_file.Length;
                sFiles_uploaded += "\n" + uploaded_file.FileName;

                //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string uploaded_Filename = uploaded_file.FileName;
                string Pathstr = "\\" + Id + "\\";
                string dbstr = Pathstr + uploaded_Filename;
                string new_Filename_on_Server = path_for_Uploaded_Files + "\\" + Pathstr + uploaded_Filename;

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files + Pathstr))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files + Pathstr);
                }


                if (System.IO.File.Exists(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server);
                }
                if (!System.IO.File.Exists(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    using (FileStream stream = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                    {
                        uploaded_file.CopyTo(stream);
                        stream.Flush();
                    }
                }
                byte[] fileData = null;
                using (FileStream fs = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    var binaryReader = new BinaryReader(fs);
                    fileData = binaryReader.ReadBytes((int)fs.Length);

                    documentModels.Add(new documentModel
                    {
                        document_file_name = uploaded_file.FileName,
                        document_file_extension = Path.GetExtension(uploaded_file.FileName),
                        document_mime_type = uploaded_file.ContentType,
                        document_object = fileData,
                        document_size = uploaded_file.Length,
                        file_path = dbstr
                    });
                }
                //  System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server);

            }

            return documentModels;
        }
    }
}
