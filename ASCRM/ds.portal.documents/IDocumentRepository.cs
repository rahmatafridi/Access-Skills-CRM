using ds.portal.documents.Models;
using System;
using System.Collections.Generic;

namespace ds.portal.documents
{
    public interface IDocumentRepository
    {
        IEnumerable<DefaultDocument> GetAllDefaultDocuments();
        bool InsertUpdateDefaultDocumentRecord(DefaultDocument document);
        DefaultDocument GetDefaultDocumentById(int id);
        IEnumerable<DefaultDocument> GetDocumentsByCategoryId(int id);
        bool DeleteDefaultDocumentById(int id, int userId, string userName);
    }
}
