using crm.portal.CrmModel;
using crm.portal.Interfaces.CourseContent;
using crm.portal.Interfaces.Matrix;
using crm.portal.OscaModel;
using Dapper;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace crm.portal.Repositories.CourseContent
{
    public class CourseContentRepository : ICourseContentRepository
    {
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;
        private readonly IMatrixRepository _matrixRepository;
        public CourseContentRepository(IMatrixRepository matrixRepository,IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();
            _matrixRepository = matrixRepository;

        }
        public IEnumerable<CourseContent_Portal> GetCourseContents(int userId,int id ,bool isAdmin)
        {
            IEnumerable<CourseContent_Portal> courseConent = new List<CourseContent_Portal>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (isAdmin == true)
                    {
                        var data = _matrixRepository.LoadLearnerCourse(userId);
                        string storedProc = "[dbo].[SP_PORTAL_GET_COURSE_CONTENT_ForAdmin]";
                        object dynamicParams = new
                        {
                            id = id
                        };

                        courseConent = conn.Query<CourseContent_Portal>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        var data = _matrixRepository.LoadLearnerCourse(userId);
                        string storedProc = "[dbo].[SP_PORTAL_GET_COURSE_CONTENT]";
                        object dynamicParams = new
                        {
                            CC_Id_Course_Level = data.Course_Id
                        };
                   
                    courseConent = conn.Query<CourseContent_Portal>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    return courseConent;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }



        public IEnumerable<Courses> LoadCourses()
        {
            IEnumerable<Courses> course = new List<Courses>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_SS_GetListCourses]";
                    object dynamicParams = new
                    {
                        
                    };

                    course  = conn.Query<Courses>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return course;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public bool InsertUpdateDocumentRecord(CourseContent_Portal document)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@CC_Id_Course_Level", document.CC_Id_Course_Level);
                    dynamicParams.Add("@CC_Name", document.CC_Name);
                    dynamicParams.Add("@CC_Description", document.CC_Description);
                    dynamicParams.Add("@CC_Type", document.CC_Type);
                    dynamicParams.Add("@CC_FilePath", document.CC_FilePath);
                    dynamicParams.Add("@CC_CreatedByUserName", document.CC_CreatedByUserName);
                    dynamicParams.Add("@CC_CreatedDate", document.CC_CreatedDate);
                    dynamicParams.Add("@CC_File_Size", document.CC_File_Size);
                    dynamicParams.Add("@CC_SortOrder", document.CC_SortOrder);


                    if (document.CC_Id > 0)
                    {
                        dynamicParams.Add("@CC_Id", document.CC_Id);
                        string storedProc = "[dbo].[SP_PORTAL_UPDATE_COURSE_CONTENT]";
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_PORTAL_CREATE_COURSE_CONTENT]";
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

        public bool DeleteCourseContentById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_DELETE_COURSE_CONTENT]";
                    object dynamicParams = new { Id = id};

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return true;
        }

        public CourseContent_Portal GetCourseContentById(int Id)
        {
            var data = new CourseContent_Portal(); 
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_COURSE_CONTENT_BY_ID]";
                    object dynamicParams = new { Id = Id };

                  data =  conn.QueryFirstOrDefault<CourseContent_Portal>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return data;
        }
    }
}
