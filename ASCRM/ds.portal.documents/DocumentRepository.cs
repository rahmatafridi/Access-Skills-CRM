using Dapper;
using ds.core.uow;
using ds.portal.documents.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ds.portal.documents
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        //private readonly string _connStringOSCA;
        //private readonly IUOW _unitOfWork_OSCA;
        public DocumentRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }
        #region Default Documents

        public IEnumerable<DefaultDocument> GetAllDefaultDocuments()
        {
            IEnumerable<DefaultDocument> documents = new List<DefaultDocument>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdCore_DefaultDocument_GetAll]";                    

                    documents = conn.Query<DefaultDocument>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return documents;
        }

        public bool InsertUpdateDefaultDocumentRecord(DefaultDocument document)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (document.DefaultDoc_Id > 0)
                    {
                        string storedProc = "[dbo].[SP_mdCore_DefaultDocument_Update]";
                        object dynamicParams = document;
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_mdCore_DefaultDocument_Insert]";
                        object dynamicParams = document;
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        
        public DefaultDocument GetDefaultDocumentById(int id)
        {
            DefaultDocument document = new DefaultDocument();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdCore_DefaultDocument_GetById]";
                    object dynamicParams = new { DefaultDoc_Id = id };

                    document = conn.QuerySingleOrDefault<DefaultDocument>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return document;
        }

        public IEnumerable<DefaultDocument> GetDocumentsByCategoryId(int id)
        {
            IEnumerable<DefaultDocument> documents = new List<DefaultDocument>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdCore_DefaultDocument_GetByCategoryId]";
                    object dynamicParams = new { DefaultDoc_Id_Category = id };

                    documents = conn.Query<DefaultDocument>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return documents;
        }
        public bool DeleteDefaultDocumentById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdCore_DefaultDocument_DeleteById]";
                    object dynamicParams = new { DefaultDoc_Id = id, DefaultDoc_DeletedByUserId = userId, DefaultDoc_DeletedByUserName = userName };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        #endregion
    }
}
