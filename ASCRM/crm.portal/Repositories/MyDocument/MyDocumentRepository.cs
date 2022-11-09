using crm.portal.Interfaces.MyDocument;
using crm.portal.OscaModel;
using Dapper;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace crm.portal.Repositories.MyDocument
{
    public class MyDocumentRepository : IMyDocumentRepository
    {
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;

        public MyDocumentRepository(IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();

        }
        public IEnumerable<LearnerDoc> LoadLearnerDoc(int Id)
        {
            List<LearnerDoc> learnerDoc = new List<LearnerDoc>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Portal_GetLearnerMyDocs]";
                    object dynamicParams = new
                    {
                        LearnerId = Id
                    };

                    learnerDoc = (List<LearnerDoc>)conn.Query<LearnerDoc>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return learnerDoc;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
    }
}
