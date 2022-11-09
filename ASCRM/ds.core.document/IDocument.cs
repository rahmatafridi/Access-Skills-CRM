using ds.core.document.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ds.core.document
{
    public interface IDocument
    {
        List<documentModel> Upload(dynamic document, int Id, HttpRequest Request, out long uploaded_size, out int iCounter, out string sFiles_uploaded);
        documentModel DownloadFile(int documentId, int moduleType);
        List<documentModel> UploadCourseContent(dynamic document, int Id, HttpRequest Request, out long uploaded_size, out int iCounter, out string sFiles_uploaded);
        bool DeleteFile(string path);
        bool DeleteFileCourseContent(string path);

        bool DeleteFileUpload(string path);
        List<documentModel> UploadResource(dynamic document, int Id, HttpRequest Request, out long uploaded_size, out int iCounter, out string sFiles_uploaded);

        string DownlaodFileFromPath(string path);
    }
}
