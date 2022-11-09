using Dapper;
using ds.core.comms.Mail;
using ds.core.configuration.Models;
using ds.core.emailtemplates;
using ds.core.uow;
using ds.portal.applications.Models;
using ds.portal.leads.Models;
using ds.portal.leads.Models.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace ds.portal.applications
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMailService _emailSender;
      
        public ApplicationRepository(IMailService emailSender, IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal, IEmailTemplateRepository emailTemplateRepository)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
            _unitOfWork_OSCA = unitOfWork_OSCA;
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _emailTemplateRepository = emailTemplateRepository;
            _emailSender = emailSender;
        }
        public bool InsertUpdateCourseLevel(CourseLevels levels)
        {
            var dynamicParams = new DynamicParameters();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (levels.CL_Id > 0)
                    {
                        dynamicParams.Add("@id", levels.CL_Id);

                    }
                    dynamicParams.Add("@title", levels.CL_Title);
                    dynamicParams.Add("@description", levels.CL_Description);
                    dynamicParams.Add("@code", levels.CL_Code);
                    dynamicParams.Add("@level", levels.CL_Level);
                    dynamicParams.Add("@sortOrder", levels.CL_SortOrder);

                    if (levels.CL_Id > 0)
                    {
                        string storedProc = "[dbo].[Update_Course_Level]";

                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[Insert_Course_Level]";

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
        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
        }
        public bool CheckHeader(string headertext)
        {
            bool retVal = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_APP_CHECK_HEADER]";
                    object dynamicParams = new { header = headertext };

                    retVal = conn.QuerySingleOrDefault<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retVal;
        }
        public bool SubmittedCheck(int id)
        {
            bool retVal = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_APP_CHECK_SUBMITTED_APPLICATION]";
                    object dynamicParams = new { UserId = id };

                    retVal = conn.QuerySingleOrDefault<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retVal;
        }
        public bool InsertUpdateHeader(string text)
        {
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    dynamicParams.Add("@header", text);
                    string storedProc = "[dbo].[SP_APP_ADD_HEADER]";

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }
        public bool InsertUpdateApplication(List<SaveAnswer> application)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var appId = application.Where(x => x.q_mapname == "id").FirstOrDefault();
                   
                    foreach (var item in application)
                    {
                        if (item.q_mapname == "sing")
                        {

                            //string result = Convert.ToBase64String(item.q_answer);


                            //  byte[] bytes = System.Convert.FromBase64String(result);
                            byte[] bytes = Convert.FromBase64String(item.q_answer.Split(',')[1]);
                            // application.Signature = bytes;

                            //var binary = StringToBinary(item.q_answer);
                            var dynamicParamSign = new DynamicParameters();
                            dynamicParamSign.Add("@app_id", Convert.ToInt32(appId.q_answer));
                            dynamicParamSign.Add("@sing", bytes);
                            dynamicParamSign.Add("@username", "");
                            dynamicParamSign.Add("@text", item.text);
                            string storedProc = "[dbo].[SP_Insert_Application_Signature]";

                            conn.Query(storedProc, param: dynamicParamSign, commandType: CommandType.StoredProcedure);
                        }
                        else if (item.q_mapname != "id" && item.q_mapname != "sign")
                        {
                            var dynamicParamSign = new DynamicParameters();

                            byte[] bytes;
                            if (item.q_file != null)
                            {

                                bytes = Convert.FromBase64String(item.q_file.Split(',')[1]);
                                dynamicParamSign.Add("@file", bytes);

                            }
                            else
                            {

                                dynamicParamSign.Add("@file", new byte[] { });
                            }
                            dynamicParamSign.Add("@q_answer", item.q_answer);
                            dynamicParamSign.Add("@q_mapname", item.q_mapname);
                            dynamicParamSign.Add("@extension", item.q_file_extention);
                            string storedProc = "[dbo].[SP_Update_QuestionAnswer]";

                            conn.Query(storedProc, param: dynamicParamSign, commandType: CommandType.StoredProcedure);
                        }
                    }
                    //if (application.app_id > 0)
                    //{
                    //    string storedProc = "[dbo].[SP_AppPortal_Application_Update]";
                    //    object dynamicParams = application;
                    //    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    //}
                    //else
                    //{
                    //    string storedProc = "[dbo].[SP_AppPortal_Application_Insert]";
                    //    object dynamicParams = application;

                    //    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    //}
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }
        public bool InsertUpdateEnrollmentApplication(List<SaveAnswer> application)
        {
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    var appId = application.Where(x => x.q_mapname == "id").FirstOrDefault();
                    var data = GetApplicationByIdForLearner(Convert.ToInt32(appId.q_answer));

                    DateTime dt = new DateTime();
                    var title = data.Where(x => x.q_title.Contains("Title")).FirstOrDefault();
                    var titleAns = "";
                    if (title != null)
                    {
                        titleAns = title.answer;
                    }

                    var firstName = data.Where(x => x.q_title.Contains("First")).FirstOrDefault();
                    var firstNameAns = "";
                    if (firstName != null)
                    {
                        firstNameAns = firstName.answer;
                    }
                    var lastName = data.Where(x => x.q_title.Contains("Surname")).FirstOrDefault();
                    var lastNameAns = "";
                    if (lastName != null)
                    {
                        lastNameAns = lastName.answer;
                    }
                    var middelName = data.Where(x => x.q_title.Contains("Middlename")).FirstOrDefault();
                    var middelNameAns = "";
                    if (middelName != null)
                    {
                        middelNameAns = middelName.answer;
                    }
                    var Gender = data.Where(x => x.q_title.Contains("Gender")).FirstOrDefault();
                    var GenderAns = "";
                    if (Gender != null)
                    {
                        
                        GenderAns = Gender.answer;
                        if(GenderAns == null)
                        {
                            GenderAns = "";
                        }
                    }
                    var DOB = data.Where(x => x.q_title.Contains("DOB")).FirstOrDefault();
                    var DOBAns = "";
                    if (DOB != null)
                    {
                        DOBAns = DOB.answer;
                    }
                    var Address1 = data.Where(x => x.q_title.Contains("Address")).FirstOrDefault();
                    var Address1Ans = "";
                    if (Address1 != null)
                    {
                        Address1Ans = Address1.answer;
                    }
                    var Address2 = data.Where(x => x.q_title.Contains("Address2")).FirstOrDefault();
                    var Address2Ans = "";
                    if (Address2 != null)
                    {
                        Address2Ans = Address2.answer;
                    }
                    var Address3 = data.Where(x => x.q_title.Contains("Address3")).FirstOrDefault();
                    var Address3Ans = "";
                    if (Address3 != null)
                    {
                        Address3Ans = Address3.answer;
                    }
                    var Address4 = data.Where(x => x.q_title.Contains("Address4")).FirstOrDefault();
                    var Address4Ans = "";
                    if (Address4 != null)
                    {
                        Address4Ans = Address4.answer;
                    }
                    var Address5 = data.Where(x => x.q_title.Contains("Address5")).FirstOrDefault();
                    var Address5Ans = "";
                    if (Address5 != null)
                    {
                        Address5Ans = Address5.answer;
                    }
                    var PostCode1 = data.Where(x => x.q_title.Contains("PostCode")).FirstOrDefault();
                    var PostCode1Ans = "";
                    if (PostCode1 != null)
                    {
                        PostCode1Ans = PostCode1.answer;
                    }
                    var Telephone = data.Where(x => x.q_title.Contains("Telephone")).FirstOrDefault();
                    var TelephoneAns = "";
                    if (Telephone != null)
                    {
                        TelephoneAns = Telephone.answer;
                    }
                    var Mobile = data.Where(x => x.q_title.Contains("Mobile")).FirstOrDefault();
                    var MobileAns = "";
                    if (Mobile != null)
                    {
                        MobileAns = Mobile.answer;
                    }
                    var Ethnicity = data.Where(x => x.q_title.Contains("Ethnicity")).FirstOrDefault();
                    var EthnicityAns = "";
                    if (Ethnicity != null)
                    {
                        EthnicityAns = Ethnicity.answer;
                    }
                    var email = data.Where(x => x.q_title.Contains("Email")).FirstOrDefault();
                    var emailAns = "";
                    if (email != null)
                    {
                        emailAns = email.answer;
                    }

                    var Nationality = data.Where(x => x.q_title.Contains("Nationality")).FirstOrDefault();
                    var NationalityAns = "";
                    if (Nationality != null)
                    {
                        NationalityAns = Nationality.answer;
                        if(NationalityAns == null)
                        {
                            NationalityAns = "";
                        }
                    }
                    var NationalInsuranceNumber = data.Where(x => x.q_title.Contains("National Insurance Number")).FirstOrDefault();
                    var NationalInsuranceNumberAns = "";
                    if (NationalInsuranceNumber != null)
                    {
                        NationalInsuranceNumberAns = NationalInsuranceNumber.answer;
                    }


                    // title = data..Contains("Title").ToString();
                    //var firstName = data.q_title.Contains("First Name").ToString();
                    //var email = data.q_title.Contains("Email").ToString();

                    string storedProc = "[dbo].[SP_AddNewLearner]";
                    object dynamicParams = new
                    {
                        Learner_Ref = "",
                        Learner_Id_Titles = titleAns,
                        Learner_FirstName = firstNameAns,
                        Learner_Middlename = middelNameAns,
                        Learner_Surname = lastNameAns,
                        Learner_Gender = GenderAns,
                        Learner_DOB = DOBAns,
                        Learner_IsActive = 1,
                        Learner_Address1 = Address1Ans,
                        Learner_Address2 = Address2Ans,
                        Learner_Address3 = Address3Ans,
                        Learner_Address4 = Address4Ans,
                        Learner_Address5 = Address5Ans,
                        Learner_PostCode1 = PostCode1Ans,
                        Learner_PostCode2 = "",
                        Learner_Telephone = TelephoneAns,
                        Learner_Telephone2 = "",
                        Learner_TelephoneWork1 = "",
                        Learner_TelephoneWork2 = "",
                        Learner_Mobile1 = MobileAns,
                        Learner_Mobile2 = "",
                        Learner_Email1 = emailAns,
                        Learner_Email2 = "",
                        Learner_IsOverseas = 0,
                        Learner_OCRDate = ConvertDateToSQL(dt.ToShortDateString()),
                        Learner_OCRNumber = "",
                        Learner_IsOCRRegistered = "0",
                        Learner_ReceivedDate = ConvertDateToSQL(dt.ToShortDateString()),
                        Learner_BookedDate = ConvertDateToSQL(dt.ToShortDateString()),
                        Learner_IsFunded = "0",
                        Learner_IsClaimed = "0",
                        Learner_Occupation = "0",
                        Learner_CreatedDate = ConvertDateToSQL(dt.ToShortDateString()),
                        Learner_Id_MaritalStatus = "1",
                        Learner_Id_Assessor1 = "0",
                        Learner_Id_Employer = "0",
                        Learner_Id_Status = "1",
                        Learner_Id_DealingPerson = "197",

                        Learner_Id_Ethnicity = EthnicityAns,
                        Learner_Id_Country = 224,
                        Learner_CreatedByUser = 291,
                        Learner_Id_PStatus = 1,
                        LastLearnerId = "0",
                    };

                    int iLastAdded = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    var LearnerIdData = data.FirstOrDefault(x => x.q_title == "Learner ID" && x.q_is_admin == 1);
                    if (LearnerIdData != null)
                    {
                        application.Add(new SaveAnswer()
                        {
                            q_answer = iLastAdded.ToString(),
                            q_mapname = LearnerIdData.q_mapname

                        });
                    }
                    InsertUpdateApplication(application);
                    var restult = UpdateOscaLearnerContact(iLastAdded, NationalityAns, NationalInsuranceNumberAns);
                    string displayName = firstNameAns + " " + lastNameAns;
                    string sRawPassword = GenerateRandomPasswordUsingGUID(6);
                    CreatePortalUser(iLastAdded, displayName, sRawPassword, emailAns, Convert.ToInt32(appId.q_answer));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }

        public bool CreatePortalUser(int learnerId, string sDisplayName, string sPassword, string sEmail, int appId)
        {
            using (SqlConnection conn = new SqlConnection(_connString_Portal))
            {
                try
                {
                    string pass = EncodePassword(sPassword);

                    string storedProc = "[dbo].[SP_CREATE_USER_FROMCRM]";
                    object dynamicParams = new
                    {
                        learnerId = learnerId,
                        email = sEmail,
                        password = pass,
                        display_name = sDisplayName
                    };

                    bool iLastAdded = conn.QuerySingleOrDefault<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                    // aspnet user
                    string storedProc1 = "[dbo].[aspnet_CreateUser]";
                    object dynamicParams1 = new
                    {
                        ApplicationName = "/",
                        UserName = learnerId,
                        Password = pass,
                        Email = sEmail,
                        PasswordQuestion = "",
                        PasswordAnswer = ""
                    };

                    int userAdded = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);

                    // aspnet user role
                    string storedProc2 = "[dbo].[aspnet_UsersInRoles_AddUsersToRoles]";
                    object dynamicParams2 = new
                    {
                        ApplicationName = "/",
                        RoleNames = "Learner",
                        UserNames = learnerId,
                        CurrentTimeUtc = "",

                    };

                    int roleAdded = conn.QuerySingleOrDefault<int>(storedProc2, param: dynamicParams2, commandType: CommandType.StoredProcedure);

                    //if(iLastAdded == true)
                    //{
                    //var ApplicationUser = new ApplicationUser();

                    var data = GetCourseLevelForPortal(appId);
                    int courseId = data.appuser_courselevel_id;
                    var id = InsertLearnerCourses(false, false, "", learnerId, courseId, "1");

                    SendAccountCreationNotifications(learnerId, sDisplayName, sPassword, sEmail, false);
                    //InsertLearnerNote(int iLearnerId, string sEmailContent)
                    //}

                    return iLastAdded;

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
        public int InsertLearnerCourses(bool bCompleted, bool bFunded, string sCompleteDate, int strLearnerId, int strCourseId, string strCourseStatus)
        {
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc1 = "[dbo].[SP_CREATE_LEARNER_COURSE_FROM_CRM]";
                    object dynamicParams1 = new
                    {
                        learnerId = strLearnerId,
                        courseId = strCourseId,
                        id = 0
                    };

                    int userAdded = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                    InsertRegInfoCourseActivity(strLearnerId, userAdded.ToString());
                    InsertJourneySSandMatrix(strLearnerId, userAdded);
                    InsertLearnerMatrix(strLearnerId, userAdded);


                    return userAdded;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

        }
        public void InsertLearnerMatrix(int iLearner, int iLearnerCourseId)
        {
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc1 = "[dbo].[SP_SS_MatrixLearnersCreate]";
                    object dynamicParams1 = new
                    {
                        LearnerId = iLearner,
                        LearnerCourseId = iLearnerCourseId,

                    };

                    int userAdded = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

        }
        public void InsertJourneySSandMatrix(int iLearner, int iLearnerCourseId)
        {
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc1 = "[dbo].[SP_SS_InsertJLP]";
                    object dynamicParams1 = new
                    {
                        LearnerId = iLearner,
                        LearnerCourseId = iLearnerCourseId,

                    };

                    int userAdded = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public void InsertRegInfoCourseActivity(int iLearnerId, string iCurrentCourseId)
        {
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc1 = "[dbo].[SP_INSERT_RegInfoCourseActivity_CRM]";
                    object dynamicParams1 = new
                    {
                        learnerId = iLearnerId,
                        courseId = iCurrentCourseId,

                    };

                    int userAdded = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public bool InsertLearnerNote(int iLearnerId, string sEmailContent)
        {
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc1 = "[dbo].[SP_INSERT_LearnerNotes_CRM]";
                    object dynamicParams1 = new
                    {
                        learner_id = iLearnerId,
                        content = sEmailContent,
                        userId="391"


                    };

                    bool userAdded = conn.QuerySingleOrDefault<bool>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                    return userAdded;
                }
                catch (Exception ex)
                {

                    throw;
                }
                

            }
        }
        public bool SendAccountCreationNotifications(int iLearnerId, string sDisplayName, string sPassword, string sRecipientEmail, bool bCopyToLearner)
        {
            string sEmailSubject = "";
            string sEmailBody = "";
            string sEmailFrom = "";
            if (sRecipientEmail != "")
            {
                Hashtable ht = new Hashtable();
                ht.Add("[USERNAME]", iLearnerId);
                ht.Add("[PASSWORD]", sPassword);
                Tuple<string, string> tuple = _emailTemplateRepository.GetSubjectAndEmailTemplateBodyReplacedByCode("ENR_LER", ht);
                 _emailSender.SendEmail(sRecipientEmail.Trim().ToLower(), tuple.Item1, tuple.Item2);
                InsertLearnerNote(iLearnerId, tuple.Item2);
            }
            return true;
        }
        public ApplicationUser GetCourseLevelForPortal(int id)
        {
            ApplicationUser application = new ApplicationUser();
            using (SqlConnection conn1 = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_AppPortal_Application_GetById]";
                    string storedProcCrm = "[dbo].[SP_GET_COURSELEVEL_FROM_APP_USER]";

                    object dynamicParamsCrm = new { App_Id = id };

                    application = conn1.QuerySingleOrDefault<ApplicationUser>(storedProcCrm, param: dynamicParamsCrm, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return application;
        }
        public int UpdateOscaLearnerContact(int learnerId, string nat, string ni)
        {
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_UPDATELEARNERCONTACT_FROM_CRM]";
                    object dynamicParams = new
                    {
                        Learner_Id = learnerId,
                        Nationality = nat,
                        NationalInsuranceNumber = ni
                    };

                    int iLastAdded = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return iLastAdded;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public int InsertNewApplication(int user_id, string user_name, int course_level_id)
        {
            int app_id = 0;
            var questions = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Insert]";
                    // string storedProc = "[dbo].[SP_AppPortal_Application_Insert]";
                    object dynamicParams = new
                    {
                        //app_advisoruserid = GetLearnerAdvisorId(user_name),
                        //app_email = user_name,
                        user_id = user_id,
                        course_level_id = course_level_id,
                        ap_id = 0
                        //user_name = user_name
                    };
                    app_id = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    string storedProcq = "[dbo].[SP_Get_Questions_ByCourseLevel]";

                    object dyamaicPramsQuestion = new
                    {
                        level_id = course_level_id
                    };
                    questions = (List<QuestionModel>)conn.Query<QuestionModel>(storedProcq, param: dyamaicPramsQuestion, commandType: CommandType.StoredProcedure);
                    bool isPrivacy = false;
                    foreach (var item in questions)
                    {

                        var dynamicParams1 = new DynamicParameters();
                        var name = (app_id + "" + item.q_id + "q");
                        dynamicParams1.Add("@app_id", app_id);
                        dynamicParams1.Add("@q_id", item.q_id);
                        dynamicParams1.Add("@q_title", item.q_title);
                        dynamicParams1.Add("@q_type", item.q_type);
                        dynamicParams1.Add("@q_opt_header", item.q_optheader_title);
                        dynamicParams1.Add("@q_min", item.q_minlength);
                        dynamicParams1.Add("@q_mix", item.q_maxlength);
                        dynamicParams1.Add("@q_id_appstep", item.q_id_appstep);
                        dynamicParams1.Add("@q_id_appsection", item.q_id_appsection);
                        dynamicParams1.Add("@q_mapname", name);
                        dynamicParams1.Add("@q_is_mandatory", item.q_is_mandatory);
                        dynamicParams1.Add("@q_is_admin", item.q_is_admin);
                        dynamicParams1.Add("@q_validation_id", item.q_validation_id);
                        dynamicParams1.Add("@q_id_dependency", item.q_id_dependency);
                        dynamicParams1.Add("@q_depend_yesno", item.q_depend_yesno);
                        dynamicParams1.Add("@q_is_enrolled", item.q_is_enrolled);
                        dynamicParams1.Add("@q_is_confirm", item.q_is_confirm);
                        dynamicParams1.Add("@q_html_lable", item.q_html_lable);



                        string storedProc1 = "[dbo].[SP_Insert_Application_Question_Answer]";
                        var id = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                        //isPrivacy = true;
                    }
                    //if(isPrivacy== true)
                    //{
                    //    var dynamicParams1 = new DynamicParameters();
                    //    var name = (app_id + "" + "noticeq");
                    //    dynamicParams1.Add("@app_id", app_id);
                    //    dynamicParams1.Add("@q_id", 0);
                    //    dynamicParams1.Add("@q_title", "Privacy Notice");
                    //    dynamicParams1.Add("@q_type", 0);
                    //    dynamicParams1.Add("@q_opt_header", 0);
                    //    dynamicParams1.Add("@q_min", 0);
                    //    dynamicParams1.Add("@q_mix", 0);
                    //    dynamicParams1.Add("@q_id_appstep", 0);
                    //    //dynamicParams1.Add("@q_id_appsection", item.q_id_appsection);
                    //    dynamicParams1.Add("@q_mapname", name);
                    //    //dynamicParams1.Add("@q_is_mandatory", item.q_is_mandatory);
                    //    dynamicParams1.Add("@q_is_admin", 0);
                    //    //dynamicParams1.Add("@q_validation_id", item.q_validation_id);
                    //    //dynamicParams1.Add("@q_id_dependency", item.q_id_dependency);
                    //    //dynamicParams1.Add("@q_depend_yesno", item.q_depend_yesno);


                    //    string storedProc1 = "[dbo].[SP_Insert_Application_Question_Answer]";
                    //    var id = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                    //}
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return app_id;
        }

        public IEnumerable<ApplicationModel> GetAllApplicationsForAdminByType(string type)
        {
            IEnumerable<ApplicationModel> applications = new List<ApplicationModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Applications_GetAllByType]";
                    object dynamicParams = new { Type = type };

                    applications = conn.Query<ApplicationModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return applications;
        }

        public IEnumerable<ApplicationModel> GetAllApplicationsByUserId(int userId)
        {
            IEnumerable<ApplicationModel> applications = new List<ApplicationModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_GetByUserId]";
                    object dynamicParams = new { UserId = userId };

                    applications = conn.Query<ApplicationModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return applications;
        }
        public IEnumerable<ApplicationQuestions> GetApplicationAttachment(int id)
        {
            IEnumerable<ApplicationQuestions> applications = new List<ApplicationQuestions>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_GET_APPLICATION_ATTACHMENT]";
                    object dynamicParams = new { id = id };

                    applications = conn.Query<ApplicationQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return applications;
        }

        public IEnumerable<QuestionModel> GetQuestionAnswerById(int app_Id)
        {
            IEnumerable<QuestionModel> application = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_AppPortal_Application_GetById]";
                    string storedProc = "[dbo].[SP_Get_Answser_For_Submit]";

                    object dynamicParams = new { App_Id = app_Id };

                    application = conn.Query<QuestionModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return application;
        }
        public IEnumerable<QuestionModel> GetEnrolledQuestions(int app_Id)
        {
            IEnumerable<QuestionModel> application = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_AppPortal_Application_GetById]";
                    string storedProc = "[dbo].[SP_Get_Application_Enrolled_Questions]";

                    object dynamicParams = new { App_Id = app_Id };

                    application = conn.Query<QuestionModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return application;
        }

        public IEnumerable<QuestionModel> GetDependentAnswerById(int app_Id)
        {
            IEnumerable<QuestionModel> application = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_AppPortal_Application_GetById]";
                    string storedProc = "[dbo].[SP_Get_Application_Dependent_Answer]";

                    object dynamicParams = new { App_Id = app_Id };

                    application = conn.Query<QuestionModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return application;
        }
        public IEnumerable<QuestionModel> GetConfirmQuestionsAnswer(int app_Id)
        {
            IEnumerable<QuestionModel> application = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_AppPortal_Application_GetById]";
                    string storedProc = "[dbo].[SP_GET_APPLICATION_CONFIRM_ANSWER]";

                    object dynamicParams = new { App_Id = app_Id };

                    application = conn.Query<QuestionModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return application;
        }

        public IEnumerable<QuestionModel> GetDependentAnswerForAdminById(int app_Id)
        {
            IEnumerable<QuestionModel> application = new List<QuestionModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_AppPortal_Application_GetById]";
                    string storedProc = "[dbo].[SP_Get_Application_Dependent_AnswerForAdmin]";

                    object dynamicParams = new { App_Id = app_Id };

                    application = conn.Query<QuestionModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return application;
        }

        public IEnumerable<ApplicationQuestions> GetApplicationById(int app_Id)
        {
            IEnumerable<ApplicationQuestions> application = new List<ApplicationQuestions>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_AppPortal_Application_GetById]";
                    string storedProc = "[dbo].[SP_Get_Answser_For_Submit]";

                    object dynamicParams = new { App_Id = app_Id };

                    application = conn.Query<ApplicationQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return application;
        }



        public CourseLevels GetCourseLevelById(int app_Id)
        {
            CourseLevels level = new CourseLevels();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[Sp_Get_Application_Course_Level_ById]";
                    object dynamicParams = new { id = app_Id };

                    level = conn.QuerySingleOrDefault<CourseLevels>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return level;
        }
        public ApplicationQuestions GetApplicationNoticeQuestion(int app_Id)
        {
            ApplicationQuestions level = new ApplicationQuestions();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var value = app_Id + "noticeq";
                    string storedProc = "[dbo].[Sp_Get_Application_Notice]";
                    object dynamicParams = new { id = value };

                    level = conn.QuerySingleOrDefault<ApplicationQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return level;
        }
        public ApplicationSignature LoadSignatureData(int app_Id)
        {
            ApplicationSignature level = new ApplicationSignature();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[GET_APPLICATION_SIGNATURE]";
                    object dynamicParams = new { id = app_Id };

                    level = conn.QuerySingleOrDefault<ApplicationSignature>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return level;
        }

        public IEnumerable<ApplicationQuestions> GetApplicationByIdForLearner(int app_Id)
        {
            List<ApplicationQuestions> application = new List<ApplicationQuestions>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //var value = app_Id;
                    string storedProc = "[dbo].[SP_GET_APPLICATION_FOR_LEARNER]";
                    object dynamicParams = new { id = app_Id };

                    application = (List<ApplicationQuestions>)conn.Query<ApplicationQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return application;
        }

        private IEnumerable<dynamic> GetDropdownOptionsByHeaderName(string headerName)
        {
            IEnumerable<dynamic> options = new List<dynamic>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Option_GetByHeaderName]";
                    object dynamicParams = new { Header = headerName };

                    options = conn.Query<dynamic>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return options;
        }
        private IEnumerable<MultiCheck> GetDropdownOptionsByHeaderNameCheck(string headerName)
        {
            IEnumerable<MultiCheck> options = new List<MultiCheck>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Option_GetByHeaderName]";
                    object dynamicParams = new { Header = headerName };

                    options = conn.Query<MultiCheck>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return options;
        }

        public IEnumerable<ApplicationSteps> GetAdminQuestion(int course_level_id, int app_id)
        {
            IEnumerable<ApplicationSteps> applicationSteps = new List<ApplicationSteps>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_GetStepsSectionQuestionByCourseLevel]";
                    object dynamicParams = new { course_level_id = course_level_id, app_id = app_id };
                    var data = conn.QueryMultiple(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    var item1 = data.Read<ApplicationSteps>();
                    var item2 = data.Read<ApplicationSection>();
                    var item3 = data.Read<ApplicationQuestions>();

                    foreach (var item in item3.Where(aq => aq.q_type == 3))
                    {
                        var data1 = item.q_opt_header;
                        item.q_ddldata = GetDropdownOptionsByHeaderName(item.q_opt_header);
                    }

                    foreach (var item in item3.Where(aq => aq.q_type == 11))
                    {
                        var data1 = item.q_opt_header;
                        item.q_ddlMultiCheck = GetDropdownOptionsByHeaderName(item.q_opt_header);
                    }



                    var db_applicationSteps = item1.Select(x => new ApplicationSteps
                    {
                        as_id = x.as_id,
                        as_description = x.as_description,
                        as_icon = x.as_icon,
                        as_sortorder = x.as_sortorder,
                        as_title = x.as_title,
                        applicationsections = item2.Where(y => y.ase_id_appstep == x.as_id)
                        .Select(
                            z => new ApplicationSection
                            {
                                ase_id = z.ase_id,
                                ase_id_appstep = z.ase_id_appstep,
                                ase_description = z.ase_description,
                                ase_sortorder = z.ase_sortorder,
                                ase_title = z.ase_title,
                                applicationquestions = item3.Where(y => y.q_id_appsection == z.ase_id && y.q_id_appstep == x.as_id)
                            .Select(
                                q => new ApplicationQuestions
                                {
                                    q_id = q.q_id,
                                    q_is_mandatory = q.q_is_mandatory,
                                    q_mapname = q.q_mapname,
                                    q_title = q.q_title,
                                    q_type = q.q_type,
                                    q_sortorder = q.q_sortorder,
                                    q_class = q.q_class,
                                    q_optheader_title = q.q_optheader_title,
                                    answer = q.answer,
                                    q_ddldata = q.q_ddldata,
                                    q_mix = q.q_mix,
                                    q_min = q.q_min,
                                    q_ddlMultiCheck = q.q_ddlMultiCheck,
                                    q_validation_id = q.q_validation_id,
                                    q_is_enrolled = q.q_is_enrolled
                                })
                            }
                     )
                    });

                    applicationSteps = db_applicationSteps;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return applicationSteps;
        }
        public IEnumerable<ApplicationSteps> GetListStepsForLearner(int course_level_id, int app_id)
        {
            IEnumerable<ApplicationSteps> applicationSteps = new List<ApplicationSteps>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_GetStepsSectionQuestionByCourseLevel_Learner]";
                    object dynamicParams = new { course_level_id = course_level_id, app_id = app_id };
                    var data = conn.QueryMultiple(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    var item1 = data.Read<ApplicationSteps>();
                    var item2 = data.Read<ApplicationSection>();
                    var item3 = data.Read<ApplicationQuestions>();

                    foreach (var item in item3.Where(aq => aq.q_type == 3))
                    {
                        var data1 = item.q_opt_header;
                        item.q_ddldata = GetDropdownOptionsByHeaderName(item.q_opt_header);
                    }
                    var ddlMultiList1 = new List<MultiCheck>();
                    foreach (var item in item3.Where(aq => aq.q_type == 11))
                    {
                        var checkValue = 0;
                        var answer = item.answer;
                        string[] arrList = { };
                        if (answer != null)
                        {
                            arrList = answer.Split(",");
                        }
                        //  var temparr = arrList;
                        var data1 = item.q_opt_header;
                        //ddlMultiList1 = (List<MultiCheck>)GetDropdownOptionsByHeaderNameCheck(item.q_opt_header);
                        foreach (var i in GetDropdownOptionsByHeaderNameCheck(item.q_opt_header))
                        {

                            int pos = Array.IndexOf(arrList, i.Opt_Value);

                            if (pos > -1)
                            {
                                checkValue = 1;
                            }
                            else
                            {
                                checkValue = 0;
                            }
                            ddlMultiList1.Add(new MultiCheck()
                            {
                                Opt_Title = i.Opt_Title,
                                Opt_Value = i.Opt_Value,
                                Opt_Id_OptHeader = i.Opt_Id_OptHeader,
                                Check_Value = checkValue
                            });
                        }
                        item.q_ddlMultiCheck = ddlMultiList1;
                    }

                    var db_applicationSteps = item1.Select(x => new ApplicationSteps
                    {
                        as_id = x.as_id,
                        as_description = x.as_description,
                        as_icon = x.as_icon,
                        as_sortorder = x.as_sortorder,
                        as_title = x.as_title,
                        applicationsections = item2.Where(y => y.ase_id_appstep == x.as_id)
                        .Select(
                            z => new ApplicationSection
                            {
                                ase_id = z.ase_id,
                                ase_id_appstep = z.ase_id_appstep,
                                ase_description = z.ase_description,
                                ase_sortorder = z.ase_sortorder,
                                ase_title = z.ase_title,
                                applicationquestions = item3.Where(y => y.q_id_appsection == z.ase_id && y.q_id_appstep == x.as_id)
                            .Select(
                                q => new ApplicationQuestions
                                {
                                    q_id = q.q_id,
                                    q_is_mandatory = q.q_is_mandatory,
                                    q_mapname = q.q_mapname,
                                    q_title = q.q_title,
                                    q_type = q.q_type,
                                    q_sortorder = q.q_sortorder,
                                    q_class = q.q_class,
                                    q_optheader_title = q.q_optheader_title,
                                    answer = q.answer,
                                    bin_answer = q.bin_answer,
                                    q_file_extension = q.q_file_extension,
                                    q_ddldata = q.q_ddldata,
                                    q_ddlMultiCheck = q.q_ddlMultiCheck,
                                    q_validation_id = q.q_validation_id,
                                    q_is_enrolled = q.q_is_enrolled

                                })
                            }
                     )
                    });

                    applicationSteps = db_applicationSteps;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return applicationSteps;
        }

        public int GetLearnerAdvisorId(string sUsername)
        {
            int salesAdvisorId = 0;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_AppInvites_GetLearnerAdvisorId]";
                    object dynamicParams = new { api_email = sUsername };

                    salesAdvisorId = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return salesAdvisorId;
        }
        public int GetLearnerCourseLevelId(string sUsername)
        {
            int courseLevelId = 0;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_AppInvites_GetLearnerCourseLevelId]";
                    object dynamicParams = new { api_email = sUsername };

                    courseLevelId = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return courseLevelId;
        }

        public bool ReadyToEnrollApplicationWithReason(ReasonModel reasonModel)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_ReadyToEnrollWithReason]";
                    object dynamicParams = new
                    {
                        App_Id = reasonModel.app_id,
                        ReadyToEnroll_Comment = reasonModel.reason,
                        user_id = reasonModel.user_id,
                        user_name = reasonModel.user_name
                    };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool RejectApplicationWithReason(ReasonModel reasonModel)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_RejectWithReason]";
                    object dynamicParams = new
                    {
                        App_Id = reasonModel.app_id,
                        RejectedReason = reasonModel.reason,
                        user_id = reasonModel.user_id,
                        user_name = reasonModel.user_name
                    };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool DeleteApplicationWithReason(ReasonModel reasonModel)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_DeleteWithReason]";
                    object dynamicParams = new
                    {
                        App_Id = reasonModel.app_id,
                        DeletedReason = reasonModel.reason,
                        user_id = reasonModel.user_id,
                        user_name = reasonModel.user_name
                    };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool UpdateOfficeUseOnly(App_OfficeUse app_OfficeUse)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_UpdateOfficeUseOnly]";
                    object dynamicParams = app_OfficeUse;
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool UpdateOfficeUse1(App_OfficeUse1 app_OfficeUse1)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_UpdateOfficeUse1]";
                    object dynamicParams = app_OfficeUse1;
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public string GetSignatureByAppId(int app_Id)
        {
            string signature = "";
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_GetSignatureByAppId]";
                    object dynamicParams = new { App_Id = app_Id };

                    signature = conn.QuerySingleOrDefault<string>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return signature;
        }

        public bool SaveChangesToApplicationsTrackChanges(List<ApplicationsTrackChanges> ApplicationsTrackChangesList, int app_id, int user_id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    foreach (var atc in ApplicationsTrackChangesList)
                    {
                        string storedProc = "[dbo].[SP_AppPortal_Application_SaveTrackChanges]";
                        object dynamicParams = new
                        {
                            ATC_AppID = app_id,
                            ATC_FieldName = atc.ATC_FieldName,
                            ATC_ValueFrom = atc.ATC_ValueFrom,
                            ATC_ValueTo = atc.ATC_ValueTo,
                            ATC_FieldDesciption = atc.ATC_FieldDesciption,
                            ATC_ChangedBy = user_id,
                            atc_q_mapname = atc.atc_q_mapname
                        };
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
        public IEnumerable<ApplicationsTrackChanges> GetApplicationsTrackChanges(int app_id)
        {
            IEnumerable<ApplicationsTrackChanges> applicationsTrackChanges = new List<ApplicationsTrackChanges>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_GetTrackChanges]";
                    object dynamicParams = new { ATC_AppID = app_id };

                    applicationsTrackChanges = conn.Query<ApplicationsTrackChanges>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return applicationsTrackChanges;
        }
        public bool UpdateApplicationStatusAfterNotifiedChanges(int app_id, int user_id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_UpdateStatusAfterNotifiedChanges]";
                    object dynamicParams = new
                    {
                        ATC_AppID = app_id,
                        ATC_NotifiedBy = user_id
                    };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public static string ConvertDateToSQL(string theDate)
        {
            string[] theData = theDate.Split("/".ToCharArray());
            int iDay = int.Parse(theData[0]);
            int iMonth = int.Parse(theData[1]);
            int iYear = int.Parse(theData[2]);
            return iMonth.ToString() + "/" + iDay.ToString() + "/" + iYear.ToString();
        }
        public int GetCourseLevelIdByApplicationId(int app_id)
        {
            int course_level_id = 0;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_GetCourseLevelIdByApplicationId]";
                    object dynamicParams = new
                    {
                        app_id = app_id
                    };
                    course_level_id = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return course_level_id;
        }
        public int GetCourseLevelIdByUserId(int user_id)
        {
            int course_level_id = 0;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_GetCourseLevelIdByUserId]";
                    object dynamicParams = new
                    {
                        Users_Id = user_id
                    };
                    course_level_id = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return course_level_id;
        }
        public bool SubmitApplication(int app_id, string user_name)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_Application_Submit]";
                    object dynamicParams = new
                    {
                        app_id = app_id,
                        user_name = user_name
                    };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    string storedProc1 = "[dbo].[SP_UPDATE_APPLICATIONUSER_TRACK_CHANGE]";
                    object dynamicParams1 = new
                    {
                        app_id = app_id,
                        appuser_istrackchange=1
                    };
                    conn.Query(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public IEnumerable<DropdownOptionsHeader> GetApplicationDropdownOptionsHeaders()
        {
            IEnumerable<DropdownOptionsHeader> optionsHeaders = new List<DropdownOptionsHeader>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Option_GetAllHeaders]";

                    optionsHeaders = conn.Query<DropdownOptionsHeader>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return optionsHeaders;
        }
        public IEnumerable<DropdownOption> GetApplicationDropdownOptionsByHeaderId(int headerId)
        {
            IEnumerable<DropdownOption> options = new List<DropdownOption>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Option_GetByHeaderId]";
                    object dynamicParams = new { Opt_Id_OptHeader = headerId };

                    options = conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return options;
        }
        public Tuple<bool, bool> CheckTitleAndValueExists(DropdownOption dropdownOption)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Option_CheckTitleAndValueExists]";
                    object dynamicParams = new { Opt_Id = dropdownOption.Opt_Id, Opt_Value = dropdownOption.Opt_Value, Opt_Title = dropdownOption.Opt_Title, Opt_Id_OptHeader = dropdownOption.Opt_Id_OptHeader };

                    var dynamic = conn.QuerySingleOrDefault<dynamic>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    var fields = dynamic as IDictionary<string, object>;

                    return Tuple.Create<bool, bool>(Convert.ToBoolean(fields["TitleExists"]), Convert.ToBoolean(fields["ValueExists"]));
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public bool InsertUpdateOptionRecord(DropdownOption dropdownOption)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (dropdownOption.Opt_Id > 0)
                    {
                        string storedProc = "[dbo].[SP_Application_Option_Update]";
                        object dynamicParams = dropdownOption;

                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_Application_Option_Insert]";
                        object dynamicParams = dropdownOption;

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
        public bool DeleteDropdownOptionByOptionId(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Application_Option_DeleteById]";
                    object dynamicParams = new { Opt_Id = id };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public IEnumerable<Course> GetAllCourses()
        {
            IEnumerable<Course> courses = new List<Course>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Courses_GetAll]";

                    courses = conn.Query<Course>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return courses;
        }
        public IEnumerable<CourseLevels> LoadCourseLevel()
        {
            IEnumerable<CourseLevels> courses = new List<CourseLevels>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Get_CourseLevel]";

                    courses = conn.Query<CourseLevels>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return courses;
        }

        public IEnumerable<QuestionType> LoadQuestionType()
        {
            IEnumerable<QuestionType> courses = new List<QuestionType>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Load_QuestionType]";

                    courses = conn.Query<QuestionType>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return courses;
        }

        public bool DeleteCourseLevel(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Delete_Course_Level]";
                    object dynamicParams = new
                    {
                        id = id,

                    };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public ConfigurationModel GetConfigs()
        {
            ConfigurationModel config = new ConfigurationModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[Sp_Get_Config_ByType]";
                    //object dynamicParams = new { type = type };

                    config = conn.QuerySingleOrDefault<ConfigurationModel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return config;
        }
        public static string GenerateRandomPasswordUsingGUID(int length)
        {
            // Get the GUID
            string guidResult = System.Guid.NewGuid().ToString();

            // Remove the hyphens
            guidResult = guidResult.Replace("-", string.Empty);

            // Make sure length is valid
            /*   if (length <= 0 || length > guidResult.Length)
                   throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
               */
            // Return the first length bytes
            return guidResult.Substring(0, length).ToUpper();
        }
        public static string EncodePassword(string originalPassword)
        {
            // SALT PASSWORD
            originalPassword = "stusPasswordSalt" + originalPassword;

            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
    }
}
