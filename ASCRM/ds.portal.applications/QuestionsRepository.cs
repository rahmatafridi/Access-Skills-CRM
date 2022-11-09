using Dapper;
using ds.core.uow;
using ds.portal.applications.Models;
using ds.portal.leads.Models;
using ds.portal.leads.Models.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ds.portal.applications
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;

        public QuestionsRepository(IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
            _unitOfWork_OSCA = unitOfWork_OSCA;
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
        }

        public bool DeleteQuestionById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Question_DeleteById]";
                    object dynamicParams = new { id = id };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool DeleteHardQuestionById(int id)
        {
            IEnumerable<ApplicationQuestions> data = new List<ApplicationQuestions>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_APP_APPLICATION_QUESTIONS]";
                    object dynamicParams = new { id = id };

                    data = conn.Query<ApplicationQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    foreach (var item in data)
                    {
                        string storedProc2 = "[dbo].[SP_APP_GET_APPlICATION_BYID]";
                        object dynamicParams2 = new { id = item.app_id };
                        var user = conn.QueryFirstOrDefault<ApplicationUser>(storedProc2, param: dynamicParams2, commandType: CommandType.StoredProcedure);
                        if(user.appuser_issubmitted == 0)
                        {
                            string storedProc3 = "[dbo].[SP_APP_DELETE_APP_QUESTION]";
                            object dynamicParams3 = new { id = item.a_id };
                            var app = conn.Query(storedProc3, param: dynamicParams3, commandType: CommandType.StoredProcedure);
                        }
                    }
                    string storedProc1 = "[dbo].[SP_APP_DELETE_QUESTION_HARD]";
                    object dynamicParams1 = new { id = id };

                      conn.Query(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool CheckQuestionExist(int levelId, int number)
        {
            bool isExist = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Get_Question_By_Level_Number]";
                    object dynamicParams = new { levelId = levelId, number = number };

                    var data = conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    if (data.Any())
                    {
                        isExist = true;
                    }
                    else
                    {
                        isExist = false;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return isExist;
        }

        public IEnumerable<ApplicationQuestions> GetAllQuestions()
        {
            IEnumerable<ApplicationQuestions> section = new List<ApplicationQuestions>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Question_GetAll]";

                    section = conn.Query<ApplicationQuestions>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return section;
        }

        public IEnumerable<ApplicationQuestions> GetApplicationDepend(int setpId, int sectionId, int courseLeveId)
        {
            IEnumerable<ApplicationQuestions> section = new List<ApplicationQuestions>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Question_GetDepend]";
                    object dynamicParams = new { setpId = setpId, sectionId = sectionId, courselevelId = courseLeveId };

                    section = conn.Query<ApplicationQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return section;
        }

        public IEnumerable<ApplicationSection> GetApplicationSection(int id)
        {
            IEnumerable<ApplicationSection> section = new List<ApplicationSection>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Section_GetById]";
                    object dynamicParams = new { id = id };

                    section = conn.Query<ApplicationSection>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return section;
        }

        public IEnumerable<ApplicationSteps> GetApplicationStep()
        {
            IEnumerable<ApplicationSteps> step = new List<ApplicationSteps>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_SP_Step_GetAll]";

                    step = conn.Query<ApplicationSteps>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return step;
        }

        public IEnumerable<QuestionModel> GetListQuestions()
        {
            IEnumerable<QuestionModel> question = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[Sp_Get_Question]";

                    question = conn.Query<QuestionModel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return question;
        }
        public IEnumerable<QuestionModel> QuestionsSearch(string name)
        {
            IEnumerable<QuestionModel> question = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Question_In_Search]";
                    object dynamicParams = new { name = name };

                    question = conn.Query<QuestionModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return question;
        }
        public IEnumerable<QuestionModel> QuestionsByCourseLevel(int id)
        {
            IEnumerable<QuestionModel> question = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Get_Question_ByCourseId]";
                    object dynamicParams = new { id = id };

                    question = conn.Query<QuestionModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return question;
        }
        public int AssignBulk(int levelFromId, int levelToId)
        {
            int app_id = 0;
            //Application application = new Application();
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                var list = new List<CourseQuestion>();
                var listTo = new List<CourseQuestion>();
                var courseParams = new DynamicParameters();

                try
                {
                    string proc = "[dbo].[SP_Get_Question_By_Course_Id]";
                    object dynamicParams2 = new { levelFromId = levelFromId };


                    //string storedProc = "[dbo].[SP_AppPortal_Application_GetById]";
                    list = (List<CourseQuestion>)conn.Query<CourseQuestion>(proc, param: dynamicParams2, commandType: CommandType.StoredProcedure);

                    foreach (var item in list)
                    {
                        var toproc = "[dbo].[SP_Get_Question_By_Course_Id_To]";
                        object dynamicParams3 = new { levelToId = levelToId, number = item.qc_q_number };
                        listTo = (List<CourseQuestion>)conn.Query<CourseQuestion>(toproc, param: dynamicParams3, commandType: CommandType.StoredProcedure);

                        if (!listTo.Any())
                        {
                            //object dynamicParams = new { App_Id = app_Id };
                            courseParams.Add("@q_number", item.qc_q_number);
                            courseParams.Add("@qc_id_courselevel", levelToId);
                            string courseProc = "[dbo].[SP_Application_Question_Course_Add]";
                            app_id = conn.QueryFirstOrDefault<int>(courseProc, param: courseParams, commandType: CommandType.StoredProcedure);

                        }
                    }

                    // app_id = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return app_id;
        }
        public int AssignSingleBulk(int levelId, int number)
        {
            int app_id = 0;
            //Application application = new Application();
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                var list = new List<CourseQuestion>();
                var listTo = new List<CourseQuestion>();
                var courseParams = new DynamicParameters();

                try
                {
                    courseParams.Add("@q_number", number);
                    courseParams.Add("@qc_id_courselevel", levelId);
                    string courseProc = "[dbo].[SP_Application_Question_Course_Add]";
                    app_id = conn.QueryFirstOrDefault<int>(courseProc, param: courseParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return app_id;
        }

        public IEnumerable<QuestionModel> GetQuestionsByType(int id)
        {
            IEnumerable<QuestionModel> question = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Get_Question_ByType]";
                    object dynamicParams = new { id = id };

                    question = conn.Query<QuestionModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return question;
        }

        public ApplicationQuestions GetQuestionById(int id)
        {
            ApplicationQuestions questions = new ApplicationQuestions();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Question_GetById]";
                    object dynamicParams = new { id = id };

                    questions = conn.QueryFirstOrDefault<ApplicationQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return questions;
        }

        public ApplicationQuestions InsertUpdateQuestionRecord(ApplicationQuestions question)
        {
            var dynamicParams = new DynamicParameters();
            var courseParams = new DynamicParameters();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var count = GetAllQuestions();
                    var total = count.ToList().Count();
                    if (question.q_id > 0)
                    {
                        dynamicParams.Add("@q_id", question.q_id);

                    }
                    dynamicParams.Add("@q_title", question.q_title);
                    dynamicParams.Add("@q_sortorder", question.q_sortorder);
                    dynamicParams.Add("@q_id_appstep", question.q_id_appstep);
                    dynamicParams.Add("@q_id_appsection", question.q_id_appsection);
                    dynamicParams.Add("@q_type", question.q_type);
                    dynamicParams.Add("@q_optheader_title", question.q_optheader_title);
                    dynamicParams.Add("@q_maxlength", question.q_maxlength);
                    dynamicParams.Add("@q_minlength", question.q_minlength);
                    dynamicParams.Add("@q_id_dependency", question.q_id_dependency);
                    dynamicParams.Add("@q_is_mandatory", question.q_is_mandatory);
                    dynamicParams.Add("@q_mapname", question.q_mapname);
                    dynamicParams.Add("@q_class", question.q_class);
                    dynamicParams.Add("@q_is_admin", question.q_is_admin);
                    dynamicParams.Add("@q_validation_id", question.q_validation_id);
                    dynamicParams.Add("@q_depend_yesno", question.q_depend_yesno);
                    dynamicParams.Add("@q_is_enrolled", question.q_is_enrolled);
                    dynamicParams.Add("@q_is_confirm", question.q_is_confirm);
                    dynamicParams.Add("@q_html_lable", question.q_html_lable);




                    if (question.q_id > 0)
                    {
                        dynamicParams.Add("@q_number", question.q_number);

                        string storedProc = "[dbo].[SP_Application_Question_Update]";
                        conn.Query<ApplicationQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        courseParams.Add("@qc_id_courselevel", question.q_courselevelId);
                        courseParams.Add("@q_number", question.q_number);

                        string courseProc = "[dbo].[SP_Application_Question_Course_Update]";
                        conn.Query<CourseQuestion>(courseProc, param: courseParams, commandType: CommandType.StoredProcedure);


                    }
                    else
                    {
                        dynamicParams.Add("@q_number", total + 1);

                        string storedProc = "[dbo].[SP_Application_Question_Add]";
                        var questionId = conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (questionId > 0)
                        {
                            question.q_id = questionId;
                            courseParams.Add("@q_number", total + 1);
                            courseParams.Add("@qc_id_courselevel", question.q_courselevelId);
                            string courseProc = "[dbo].[SP_Application_Question_Course_Add]";
                            var courseId = conn.QueryFirstOrDefault<int>(courseProc, param: courseParams, commandType: CommandType.StoredProcedure);

                            IEnumerable<ApplicationUser> application = new List<ApplicationUser>();
                            string storedProcCrm = "[dbo].[SP_GET_USERAPP_FROM_COURSEID]";

                            object dynamicParamsCrm = new { id = question.q_courselevelId };

                            application = conn.Query<ApplicationUser>(storedProcCrm, param: dynamicParamsCrm, commandType: CommandType.StoredProcedure);
                            foreach (var item in application)
                            {
                                //if(item.appuser_issubmitted == 0)
                                //{
                                var app = new ApplicationVM();
                                string storedProc1 = "[dbo].[SP_GET_APP_FROM_APPLICATION]";

                                object dynamicParams1 = new { App_Id = item.appuser_id };

                                app = conn.QueryFirstOrDefault<ApplicationVM>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);

                                if (app != null)
                                {
                                    var dynamicParams2 = new DynamicParameters();
                                    var name = (app.app_id + "" + questionId + "q");
                                    dynamicParams2.Add("@app_id", app.app_id);
                                    dynamicParams2.Add("@q_id", questionId);
                                    dynamicParams2.Add("@q_title", question.q_title);
                                    dynamicParams2.Add("@q_type", question.q_type);
                                    dynamicParams2.Add("@q_opt_header", question.q_opt_header);
                                    dynamicParams2.Add("@q_min", question.q_minlength);
                                    dynamicParams2.Add("@q_mix", question.q_maxlength);
                                    dynamicParams2.Add("@q_id_appstep", question.q_id_appstep);
                                    dynamicParams2.Add("@q_id_appsection", question.q_id_appsection);
                                    dynamicParams2.Add("@q_mapname", name);
                                    dynamicParams2.Add("@q_is_mandatory", question.q_is_mandatory);
                                    dynamicParams2.Add("@q_is_admin", question.q_is_admin);
                                    dynamicParams2.Add("@q_validation_id", question.q_validation_id);
                                    dynamicParams2.Add("@q_id_dependency", question.q_id_dependency);
                                    dynamicParams2.Add("@q_depend_yesno", question.q_depend_yesno);
                                    dynamicParams2.Add("@q_is_enrolled", question.q_is_enrolled);
                                    dynamicParams2.Add("@q_is_confirm", question.q_is_confirm);
                                    dynamicParams2.Add("@q_html_lable", question.q_html_lable);



                                    string storedProc2 = "[dbo].[SP_Insert_Application_Question_Answer]";
                                    var id2 = conn.QuerySingleOrDefault<int>(storedProc2, param: dynamicParams2, commandType: CommandType.StoredProcedure);
                                }
                                //}
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return question;
        }

        public ApplicationQuestions AddCopyQuestion(ApplicationQuestions question)
        {
            var dynamicParams = new DynamicParameters();
            var courseParams = new DynamicParameters();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var count = GetAllQuestions();
                    var total = count.ToList().Count();
                    //if (question.q_id > 0)
                    //{
                    //    dynamicParams.Add("@q_id", question.q_id);

                    //}
                    dynamicParams.Add("@q_title", question.q_title);
                    dynamicParams.Add("@q_sortorder", question.q_sortorder);
                    dynamicParams.Add("@q_id_appstep", question.q_id_appstep);
                    dynamicParams.Add("@q_id_appsection", question.q_id_appsection);
                    dynamicParams.Add("@q_type", question.q_type);
                    dynamicParams.Add("@q_optheader_title", question.q_optheader_title);
                    dynamicParams.Add("@q_maxlength", question.q_maxlength);
                    dynamicParams.Add("@q_minlength", question.q_minlength);
                    dynamicParams.Add("@q_id_dependency", question.q_id_dependency);
                    dynamicParams.Add("@q_is_mandatory", question.q_is_mandatory);
                    dynamicParams.Add("@q_mapname", question.q_mapname);
                    dynamicParams.Add("@q_class", question.q_class);
                    dynamicParams.Add("@q_is_admin", question.q_is_admin);
                    dynamicParams.Add("@q_validation_id", question.q_validation_id);
                    dynamicParams.Add("@q_depend_yesno", question.q_depend_yesno);
                    dynamicParams.Add("@q_is_enrolled", question.q_is_enrolled);
                    dynamicParams.Add("@q_is_confirm", question.q_is_confirm);
                    dynamicParams.Add("@q_html_lable", question.q_html_lable);

                    //if (question.q_id > 0)
                    //{
                    //    dynamicParams.Add("@q_number", question.q_number);

                    //    string storedProc = "[dbo].[SP_Application_Question_Update]";
                    //    conn.Query<ApplicationQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    //    courseParams.Add("@qc_id_courselevel", question.q_courselevelId);
                    //    courseParams.Add("@q_number", question.q_number);

                    //    string courseProc = "[dbo].[SP_Application_Question_Course_Update]";
                    //    conn.Query<CourseQuestion>(courseProc, param: courseParams, commandType: CommandType.StoredProcedure);


                    //}
                    //else
                    //{
                    dynamicParams.Add("@q_number", total + 1);

                    string storedProc = "[dbo].[SP_Application_Question_Add]";
                    var id = conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    if (id > 0)
                    {
                        question.q_id = id;
                        courseParams.Add("@q_number", total + 1);
                        courseParams.Add("@qc_id_courselevel", question.q_courselevelId);
                        string courseProc = "[dbo].[SP_Application_Question_Course_Add]";
                        var courseId = conn.QueryFirstOrDefault<int>(courseProc, param: courseParams, commandType: CommandType.StoredProcedure);



                        // }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return question;
        }

        public bool RemoveQuestionFromCourse(int id, int levelId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_RemoveQuestion]";
                    object dynamicParams = new {
                        id = id ,
                        levelId= levelId
                    };

                    var data = conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
                return true;
            }
        }

        public ApplicationVM CheckAppQuestionExist(int levelId)
        {
            var model = new ApplicationVM();
            IEnumerable<ApplicationModel> app = new List<ApplicationModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Get_ApplicationByCourse]";
                    object dynamicParams = new { id = levelId };

                    app = conn.Query<ApplicationModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    model.isApp = false;
                    foreach (var item in app)
                    {
                        if (model.isApp == false)
                        {
                            string storedProc1 = "[dbo].[SP_Get_ApplicationByapp]";
                            object dynamicParams1 = new { id = item.appuser_id };

                            var data = conn.Query(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                            if (data.Count() > 0)
                            {
                                model.isApp = true;
                                model.app_id= item.appuser_id;
                                continue;
                            }
                        }
                    }
                    return model;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
