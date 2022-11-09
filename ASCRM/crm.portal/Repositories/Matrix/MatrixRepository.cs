using crm.portal.Interfaces.Matrix;
using crm.portal.OscaModel;
using Dapper;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace crm.portal.Repositories.Matrix
{
    public class MatrixRepository : IMatrixRepository
    {
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;

        public MatrixRepository(IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();

        }

        public List<MatrixLabel> GetMatrixLabels(int LearnerCourseId)
        {


            List<MatrixLabel> course = new List<MatrixLabel>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_SS_GetListLearnerAssignedMatrixes]";
                    object dynamicParams = new
                    {
                        LearnerCourseId = LearnerCourseId
                    };

                    course = (List<MatrixLabel>)conn.Query<MatrixLabel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    
                }
                catch (Exception ex)
                {

                    throw;
                }
                return course;
            }
        }

        public CourseLevel LoadLearnerCourseForOsca(int id)
        {
            CourseLevel  course = new CourseLevel();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_SS_GetLearnerCourses]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    course = conn.QueryFirstOrDefault<CourseLevel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return course;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public CourseLevel LoadLearnerCourse(int id)
        {
            CourseLevel  course = new CourseLevel();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Portal_GetLearnerCourses]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    course = conn.QueryFirstOrDefault<CourseLevel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return course;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<Matrixes> LoadMatrix(int userId, int matrixLabelId)
        {
            List<Matrixes> matrixLabel = new List<Matrixes>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {

                try
                {
                    var data = LoadLearnerCourse(userId);
                   // var data = LoadLearnerCourse();
                    string storedProc = "[dbo].[SP_SS_GetListLearnerMatrixLabelsById]";
                    object dynamicParams = new
                    {
                        LearnerCourseId = data.LearnerCourses_id,
                        MatrixLabelId= matrixLabelId
                    };

                    matrixLabel = (List<Matrixes>)conn.Query<Matrixes>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return matrixLabel;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<MatrixSummary> LoadMatrixFullSummary(int topic_Id,int usreid, int matrixLabelId)
        {
            List<MatrixSummary> matrixLabel = new List<MatrixSummary>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {

                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_MATRIC_SHOW_FULL_SUMMARY]";
                    var data = LoadLearnerCourse(usreid);
                    object dynamicParams = new
                    {
                        Id_LearnerCourse = data.LearnerCourses_id,
                        Id_MatrixLabel = matrixLabelId,
                        Id_Topic =topic_Id
                    };

                    matrixLabel = (List<MatrixSummary>)conn.Query<MatrixSummary>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return matrixLabel;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<MatrixLabel> LoadMatrixLables(int id)
        {
            List<MatrixLabel> matrixLabel = new List<MatrixLabel>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                
                try
                {
                    var data = LoadLearnerCourse(id);
                    string storedProc = "[dbo].[SP_SS_GetListLearnerMatrixLabels]";
                    object dynamicParams = new
                    {
                        LearnerCourseId = data.LearnerCourses_id
                    };

                    matrixLabel = (List<MatrixLabel>)conn.Query<MatrixLabel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return matrixLabel;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<MatrixSummary> LoadMatrixSummary(int userid, int matrixLabelId)
        {
            List<MatrixSummary> matrixLabel = new List<MatrixSummary>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {

                try
                {
                    var data = LoadLearnerCourse(userid);
                    string storedProc = "[dbo].[SP_PORTAL_MATRIC_SHOW_SUMMARY]";
                    object dynamicParams = new
                    {
                        Id_LearnerCourse = data.LearnerCourses_id,
                        Id_MatrixLabel = matrixLabelId
                    };

                    matrixLabel = (List<MatrixSummary>)conn.Query<MatrixSummary>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return matrixLabel;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<MatrixAssignedLearners> MatrixDetail(int learnercours, int topicId, int lableId)
        {
            List<MatrixAssignedLearners> matrixLabel = new List<MatrixAssignedLearners>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {

                try
                {
                    string storedProc = "[dbo].[PORTAL_GET_DETAIL_FORMATRIX]";
                    object dynamicParams = new
                    {
                        learnercours = learnercours,
                        topicId = topicId,
                        lableId= lableId
                    };

                    matrixLabel = (List<MatrixAssignedLearners>)conn.Query<MatrixAssignedLearners>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return matrixLabel;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
    }
}
