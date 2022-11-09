using crm.osca.Interfaces.Learners;
using crm.osca.Models;
using crm.portal.Interfaces.User;
using Dapper;
using ds.core.uow;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace crm.osca.Repositories.Lerarners
{
    public class LearnersRepository : ILearnersRepository
    {
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;
        private readonly IPortalUserRepository _portalUserRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public LearnersRepository(IHostingEnvironment hostingEnvironment, IPortalUserRepository portalUserRepository,IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();
            _portalUserRepository = portalUserRepository;
            _hostingEnvironment = hostingEnvironment;

        }

        public List<LearnerComman> LoadGender()
        {

            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {

                    string storedProc = "[dbo].[SP_GetListGenders]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadGroup()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_GetListCandidateGroup]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadImmigrationStatus()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_GetListImmigrationStatus]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<Learner> LoadLearners()
        {
            List<Learner> courseConent = new List<Learner>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Osca_Learner_Listing]";
                    object dynamicParams = new
                    {

                    };

                    courseConent = (List<Learner>)conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return courseConent;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadMarital()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {

                    string storedProc = "[dbo].[SP_GetListMaritalStatus]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadStatus()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_GetListCandidateStatusWithRoles]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadTitle()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaGetListTitles]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadRegion()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetListRegions]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<CourseDetail> LoadCourseDatail(int id)
        {
            List<CourseDetail> common = new List<CourseDetail>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_GetOascLearnerCourseOnLearnerID_ActiveDays]";
                    object dynamicParams = new
                    {
                        learnerId = id
                    };

                    common = (List<CourseDetail>)conn.Query<CourseDetail>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public LearnerData LoadLearnerDatail(int id)
        {
            LearnerData common = new LearnerData();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaGetLearnerDetailsByLearnerId]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    common = conn.QueryFirstOrDefault<LearnerData>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public PrimeCPD LoadPrimeCPD(int id)
        {
            PrimeCPD common = new PrimeCPD();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaSS_CPDGetPrimeReviewStats]";
                    object dynamicParams = new
                    {
                        cpdprime_learner_id = id
                    };

                    common = conn.QueryFirstOrDefault<PrimeCPD>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadCountry()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaLoadCountries]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerUser> LoadSkillsAdvisors()
        {
            List<LearnerUser> common = new List<LearnerUser>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_Role_GetListSkillsAdvisors]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerUser>)conn.Query<LearnerUser>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadFundingType()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetListTrainingProviders]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadProjectType()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {

                    string storedProc = "[dbo].[SP_GetDDLOptionsByHeaderTitle]";
                    object dynamicParams = new
                    {
                        Header = "Project Type",
                        SortColumn = "",
                        SortOrder = "ASC"
                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadAwardingBody()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetListAwardingBodyLearners]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadCourseStatus1()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_GetOscaListCoursesStatus]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadQualCourses()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_GetOscaListQualCourses]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadLearnerOutcome()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetListLearnerOutcomes]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadFinanceStaus()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetListFinanceStatus]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadRisking()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetListRiskRating]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerUser> LoadObservationAssessors()
        {
            List<LearnerUser> common = new List<LearnerUser>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {

                    string storedProc = "[dbo].[SP_SS_GetListUsersAssessors]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerUser>)conn.Query<LearnerUser>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadLearnerHHS()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaLearnerHHS]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadEthnicity()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaLearnerEthnicity]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadNationality()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaNationality]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadLearnerLLDD()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaLearnerLLDD]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadCourseStatus()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetListLearnerCourseStatus]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerReview> LoadReviewList(int id, int courseId)
        {
            List<LearnerReview> common = new List<LearnerReview>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_SS_QRGetReviewsForLearner]";
                    object dynamicParams = new
                    {
                        learnerId = id,
                        learnerCourseId = courseId
                    };

                    common = (List<LearnerReview>)conn.Query<LearnerReview>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerUploadDoc> LoadProfileActivty(int id)
        {
            List<LearnerUploadDoc> common = new List<LearnerUploadDoc>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_LoadLearnerUpoadDocs]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    common = (List<LearnerUploadDoc>)conn.Query<LearnerUploadDoc>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<DocUploadTopic> LoadDocUploadTopic(int id)
        {
            List<DocUploadTopic> common = new List<DocUploadTopic>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var loadData = LoadDocLearnerCourseId(id);

                    string storedProc = "[dbo].[SP_SS_OscaGetListJourneyLearnerProgressSummaryNotCompleted]";
                    object dynamicParams = new
                    {
                        learnerId = loadData.LearnerCourses_Id
                    };

                    common = (List<DocUploadTopic>)conn.Query<DocUploadTopic>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadReviewCourse(int id)
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_GetOscaLearnerCourseOnLearnerID_ActiveDays]";
                    object dynamicParams = new
                    {
                        learnerId = id
                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<SinupDoc> LoadSingupDocs(int id)
        {
            List<SinupDoc> common = new List<SinupDoc>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaGetSinupDoc]";
                    object dynamicParams = new
                    {
                        learnerId = id
                    };

                    common = (List<SinupDoc>)conn.Query<SinupDoc>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<IVEVDoc> LoadIVEVDOC(int id)
        {
            List<IVEVDoc> common = new List<IVEVDoc>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OScaGetAllFilesForLearnerSection3]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    common = (List<IVEVDoc>)conn.Query<IVEVDoc>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerEmployer> SearchEmployer(string name)
        {
            List<LearnerEmployer> common = new List<LearnerEmployer>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[Sp_GetEmployerByName]";
                    object dynamicParams = new
                    {
                        name = name
                    };

                    common = (List<LearnerEmployer>)conn.Query<LearnerEmployer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadPrimaryContact(int id)
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var employer = new LearnerEmployer();
                    string storedProc1 = "[dbo].[Get_OScaEmployer]";
                    object dynamicParams1 = new
                    {
                        LearnerId = id
                    };
                    employer = conn.QueryFirstOrDefault<LearnerEmployer>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);


                    string storedProc = "[dbo].[Get_OScaEmployerPrimaryContact]";
                    object dynamicParams = new
                    {
                        ECD_CareId = employer.EmployerId
                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<CompleteActivites> LoadCompleteActivites(int id)
        {
            List<CompleteActivites> common = new List<CompleteActivites>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaCPD_Load_Completed_Activities]";
                    object dynamicParams = new
                    {
                        learner_id = id
                    };

                    common = (List<CompleteActivites>)conn.Query<CompleteActivites>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<AdditionalActivites> LoadAdditionalActivites(int id)
        {
            List<AdditionalActivites> common = new List<AdditionalActivites>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaCPD_Load_Additional_Activities]";
                    object dynamicParams = new
                    {
                        learner_id = id
                    };

                    common = (List<AdditionalActivites>)conn.Query<AdditionalActivites>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public Employer LoadEmployer(int id)
        {
            Employer common = new Employer();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetCurrentEmployerForLearner2]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    common = conn.QueryFirstOrDefault<Employer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<TabHistory> LoadTabHistory(int id)
        {
            List<TabHistory> common = new List<TabHistory>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaSS_TAP2GetReviews]";
                    object dynamicParams = new
                    {
                        learnerId = id
                    };

                    common = (List<TabHistory>)conn.Query<TabHistory>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadEmployerPaid()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {

                    string storedProc = "[dbo].[SP_GetDDLOptionsByHeaderTitle]";
                    object dynamicParams = new
                    {
                        Header = "Marketing Contact Consent",
                        SortColumn = "",
                        SortOrder = "ASC"
                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadPaymentStaus()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetListAccountStatus]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<LearnerComman> LoadPriorAttainment()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaPriorAttainment]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<LearnerComman> LoadAccountStatus()
        {
            List<LearnerComman> common = new List<LearnerComman>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetListAccountStatus]";
                    object dynamicParams = new
                    {

                    };

                    common = (List<LearnerComman>)conn.Query<LearnerComman>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public DocLearnerCourseId LoadDocLearnerCourseId(int learnerId)
        {
            DocLearnerCourseId common = new DocLearnerCourseId();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaGetCourseLevelId]";
                    object dynamicParams = new
                    {
                        LearnerId = learnerId
                    };

                    common = conn.QueryFirstOrDefault<DocLearnerCourseId>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<NoteCatgory> LoadNoteCatgory()
        {
            List<NoteCatgory> common = new List<NoteCatgory>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaLoadNoteCat]";
                    object dynamicParams = new
                    {
                    };

                    common = (List<NoteCatgory>)conn.Query<NoteCatgory>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public bool AddNote(LearnerNote learnerNote)
        {
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                try
                {
                    dynamicParams.Add("@LearnerNotes_Id_Learner", learnerNote.LearnerId);
                    dynamicParams.Add("@LearnerNotes_DateTime", learnerNote.NoteDate);
                    dynamicParams.Add("@LearnerNotes_Note", learnerNote.note);
                    dynamicParams.Add("@LearnerNotes_IsActive", 1);
                    dynamicParams.Add("@LearnerNotes_IsDeleted", 0);
                    dynamicParams.Add("@LearnerNotes_Id_EnteredByUser", "");
                    dynamicParams.Add("@LearnerNotes_isChecked", learnerNote.pinned);
                    dynamicParams.Add("@LearnerNotes_Id_LearnerNotesCategory", learnerNote.cateId);
                    dynamicParams.Add("@LearnerNotes_IsPinned", learnerNote.pinned);
                    dynamicParams.Add("@LearnerNotes_PinnedBy", 1);

                    string storedProc = "[dbo].[SP_Osca_AddNote]";

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {

                    throw;
                }
                return true;
            }
        }

        public List<LearnerNote> NoteList(int leanerId, string note, int cateId)
        {
            List<LearnerNote> common = new List<LearnerNote>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_GetListLearnerNotes2]";
                    object dynamicParams = new
                    {
                        LearnerID = leanerId,
                        LNote = note,
                        LNoteCatId = cateId
                    };

                    common = (List<LearnerNote>)conn.Query<LearnerNote>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<MatrixCourse> LoadCourses(int learnerId)
        {
            List<MatrixCourse> common = new List<MatrixCourse>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaPortal_GetLearnerCourses]";
                    object dynamicParams = new
                    {
                        LearnerId = learnerId

                    };

                    common = (List<MatrixCourse>)conn.Query<MatrixCourse>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<OscaHeaderOption> LoadOscaHeader(string header)
        {
            List<OscaHeaderOption> common = new List<OscaHeaderOption>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaGetDDLOptionsByHeaderTitle]";
                    object dynamicParams = new
                    {
                        Header = header

                    };

                    common = (List<OscaHeaderOption>)conn.Query<OscaHeaderOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public CombineData LoadCombineData(int id)
        {
            CombineData common = new CombineData();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaCombineData]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    common = conn.QueryFirstOrDefault<CombineData>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public LeanerFinanceLastDate LoadLastDate(int id)
        {
            LeanerFinanceLastDate common = new LeanerFinanceLastDate();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OScaGetLearnerLastBadDebtFinanceStatusDate]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    common = conn.QueryFirstOrDefault<LeanerFinanceLastDate>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public LeanerStatusChangeDate LoadStatusChangeDate(int id)
        {
            LeanerStatusChangeDate common = new LeanerStatusChangeDate();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_Track_OscaGetLearnerLastStatusChangeDate]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    common = conn.QueryFirstOrDefault<LeanerStatusChangeDate>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return common;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public LearnerData UpdateLearnerData(LearnerData learnerData)
        {
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                try
                {
                    dynamicParams.Add("@Learner_Ref", learnerData.Learner_Ref);
                    dynamicParams.Add("@Learner_Id_Titles", learnerData.Learner_Id_Titles);
                    dynamicParams.Add("@Learner_FirstName", learnerData.Learner_FirstName);
                    dynamicParams.Add("@Learner_Surname", learnerData.Learner_Surname);
                    dynamicParams.Add("@Learner_Middlename", learnerData.Learner_Middlename);
                    dynamicParams.Add("@Learner_Gender", learnerData.Learner_Gender);
                    dynamicParams.Add("@Learner_DOB", learnerData.Learner_DOB);
                    dynamicParams.Add("@Learner_Address1", learnerData.Learner_Address1);
                    dynamicParams.Add("@Learner_Address2", learnerData.Learner_Address2);
                    dynamicParams.Add("@Learner_Address3", learnerData.Learner_Address3);

                    dynamicParams.Add("@Learner_Address4", learnerData.Learner_Address4);
                    dynamicParams.Add("@Learner_Address5", learnerData.Learner_Address5);
                    dynamicParams.Add("@Learner_PostCode1", learnerData.Learner_PostCode1);
                    dynamicParams.Add("@Learner_PostCode2", learnerData.Learner_PostCode2);
                    dynamicParams.Add("@Learner_Telephone", learnerData.Learner_Telephone);
                    dynamicParams.Add("@Learner_Telephone2", learnerData.Learner_Telephone2);
                    dynamicParams.Add("@Learner_Mobile1", learnerData.Learner_Mobile1);
                    dynamicParams.Add("@Learner_Mobile2", learnerData.Learner_Mobile2);
                    dynamicParams.Add("@Learner_TelephoneWork1", learnerData.Learner_TelephoneWork1);
                    dynamicParams.Add("@Learner_TelephoneWork2", learnerData.Learner_TelephoneWork2);
                    dynamicParams.Add("@Learner_Email1", learnerData.Learner_Email1);
                    dynamicParams.Add("@Learner_Email2", learnerData.Learner_Email2);
                    dynamicParams.Add("@Learner_Class", learnerData.Learner_Class);
                    dynamicParams.Add("@Learner_NI", learnerData.Learner_NI);
                    dynamicParams.Add("@Learner_ULN", learnerData.Learner_ULN);
                    dynamicParams.Add("@Learner_Eq_Difficulties", learnerData.Learner_Eq_Difficulties);
                    dynamicParams.Add("@Learner_Eq_NeedAssessReq", learnerData.Learner_Eq_NeedAssessReq);
                    dynamicParams.Add("@Learner_Eq_SocialDifficulties", learnerData.Learner_Eq_SocialDifficulties);
                    dynamicParams.Add("@Learner_Id_Nationality", learnerData.Learner_Id_Nationality);
                    dynamicParams.Add("@Learner_Id_LearnerLLD", learnerData.Learner_Id_LearnerLLD);
                    dynamicParams.Add("@Learner_Id_LearnerPA", learnerData.Learner_Id_LearnerPA);
                    dynamicParams.Add("@Learner_Id_LearnerHHS", learnerData.Learner_Id_LearnerHHS);
                    dynamicParams.Add("@Learner_MathDiagnostic_Assessment", learnerData.Learner_MathDiagnostic_Assessment);
                    dynamicParams.Add("@Learner_EnglishDiagnostic_Assessment", learnerData.Learner_EnglishDiagnostic_Assessment);
                    dynamicParams.Add("@Learner_MathInitial_Assessment", learnerData.Learner_MathInitial_Assessment);
                    dynamicParams.Add("@Learner_EnglishInitial_Assessment", learnerData.Learner_EnglishInitial_Assessment);
                    dynamicParams.Add("@Learner_EPAGrade", learnerData.Learner_EPAGrade);
                    dynamicParams.Add("@Learner_Id_LearnerOutcome", learnerData.Learner_Id_LearnerOutcome);
                    dynamicParams.Add("@Learner_Id_AwardingBodyLearner", learnerData.Learner_Id_AwardingBodyLearner);
                    dynamicParams.Add("@Learner_Id_RiskRating", learnerData.Learner_Id_RiskRating);
                    dynamicParams.Add("@Learner_Id", learnerData.Learner_Id);
                    dynamicParams.Add("@Learner_IsOverseas", learnerData.Learner_IsOverseas);
                    dynamicParams.Add("@Learner_Eq_IsDifficulties", learnerData.Learner_Eq_IsDifficulties);
                    dynamicParams.Add("@Learner_Eq_IsNeedAssessReq", learnerData.Learner_Eq_IsNeedAssessReq);
                    dynamicParams.Add("@Learner_Eq_IsSocialDifficulties", learnerData.Learner_Eq_IsSocialDifficulties);
                    dynamicParams.Add("@Learner_IsPBS", learnerData.Learner_IsPBS);
                    dynamicParams.Add("@Learner_IsUKResident", learnerData.Learner_IsUKResident);
                    dynamicParams.Add("@Learner_IsVisibleInPortal", learnerData.Learner_IsVisibleInPortal);
                    dynamicParams.Add("@Learner_IsWorkshopLearner", learnerData.Learner_IsWorkshopLearner);
                    dynamicParams.Add("@Learner_Id_MaritalStatus", learnerData.Learner_Id_MaritalStatus);
                    dynamicParams.Add("@Learner_Id_Status", learnerData.Learner_Id_Status);
                    dynamicParams.Add("@Learner_Id_UserSkillsAdvisors", learnerData.Learner_Id_UserSkillsAdvisors);
                    dynamicParams.Add("@Learner_Id_Ethnicity", learnerData.Learner_Id_Ethnicity);
                    dynamicParams.Add("@Learner_Id_PriorityStatus", learnerData.Learner_Id_PriorityStatus);
                    dynamicParams.Add("@Learner_Id_Country", learnerData.Learner_Id_Country);
                    dynamicParams.Add("@Learner_UpdatedDate", learnerData.Learner_UpdatedDate);
                    dynamicParams.Add("@Learner_UpdatedByUser", learnerData.Learner_UpdatedByUser);
                    dynamicParams.Add("@Learner_Id_CandidateStatus", learnerData.Learner_Id_CandidateStatus);
                    dynamicParams.Add("@Learner_Id_CandidateGroup", learnerData.Learner_Id_CandidateGroup);
                    dynamicParams.Add("@Learner_Id_ImmigrationStatus", learnerData.Learner_Id_ImmigrationStatus);
                    dynamicParams.Add("@Learner_Id_TrainingProvider", learnerData.Learner_Id_TrainingProvider);
                    dynamicParams.Add("@Learner_Id_ProjectType", learnerData.Learner_Id_ProjectType);
                    dynamicParams.Add("@Learner_Id_EmploymentStatus", learnerData.Learner_Id_EmploymentStatus);
                    dynamicParams.Add("@Learner_Id_EmploymentIntensityIndicator", learnerData.Learner_Id_EmploymentIntensityIndicator);
                    dynamicParams.Add("@Learner_Id_LengthOfEmployment", learnerData.Learner_Id_LengthOfEmployment);
                    dynamicParams.Add("@Learner_IddRefNo", learnerData.Learner_IddRefNo);
                    dynamicParams.Add("@Learner_IddABFNo", learnerData.Learner_IddABFNo);
                    dynamicParams.Add("@Learner_IddRefNo3", learnerData.Learner_IddRefNo3);
                    dynamicParams.Add("@Learner_Idd1Day", learnerData.Learner_Idd1Day);
                    dynamicParams.Add("@Learner_Idd2Day", learnerData.Learner_Idd2Day);
                    dynamicParams.Add("@Learner_Idd3Day", learnerData.Learner_Idd3Day);
                    dynamicParams.Add("@Learner_JobRole", learnerData.Learner_JobRole);
                    dynamicParams.Add("@Learner_IsRegisteredAwardingBody", learnerData.@Learner_IsRegisteredAwardingBody);
                    dynamicParams.Add("@Learner_IsMidPoint", learnerData.Learner_IsMidPoint);
                    dynamicParams.Add("@Learner_IsAuditSample", learnerData.Learner_IsAuditSample);
                    dynamicParams.Add("@Learner_IsEnterredILR", learnerData.Learner_IsEnterredILR);
                    dynamicParams.Add("@Learner_IsEnterredACE", learnerData.Learner_IsEnterredACE);
                    dynamicParams.Add("@Learner_IsConfirmedLLP", learnerData.Learner_IsConfirmedLLP);
                    dynamicParams.Add("@Learner_Id_ObsAssessor", learnerData.Learner_Id_ObsAssessor);
                    dynamicParams.Add("@Learner_Id_SupportiveAssessor", learnerData.Learner_Id_SupportiveAssessor);
                    dynamicParams.Add("@Learner_Id_AddSupportiveAssessor", learnerData.Learner_Id_AddSupportiveAssessor);
                    dynamicParams.Add("@Learner_Id_CourseStatusDetails", learnerData.Learner_Id_CourseStatusDetails);
                    dynamicParams.Add("@Learner_PercentageCompleted", learnerData.Learner_PercentageCompleted);
                    dynamicParams.Add("@Learner_Id_LearnerObservationStatus", learnerData.Learner_Id_LearnerObservationStatus);
                    dynamicParams.Add("@Learner_Id_MarketingContactConsent", learnerData.Learner_Id_MarketingContactConsent);
                    dynamicParams.Add("@Learner_Id_EmployerPaid", learnerData.Learner_Id_EmployerPaid);
                    dynamicParams.Add("@Learner_Id_Regions", learnerData.Learner_Id_Regions);
                    dynamicParams.Add("@Learner_IsObservationQuestionnaireSent", learnerData.Learner_IsObservationQuestionnaireSent);
                    dynamicParams.Add("@Learner_ObservationQuestionnaireSentDate", learnerData.Learner_ObservationQuestionnaireSentDate);
                    dynamicParams.Add("@Learner_IsObservationQuestionnaireReceived", learnerData.Learner_IsObservationQuestionnaireReceived);
                    dynamicParams.Add("@Learner_ObservationQuestionnaireReceivedDate", learnerData.Learner_ObservationQuestionnaireReceivedDate);
                    dynamicParams.Add("@Learner_DBS_Number", learnerData.Learner_DBS_Number);


                    string storedProc = "[dbo].[Sp_OscaUpdateLearnerContactDetail]";

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {

                    throw;
                }
                return learnerData;
            }
        }

        public bool UpdateEmployer(int learnerId, int employerId)
        {
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                try
                {
                    dynamicParams.Add("@LearnerId", learnerId);
                    dynamicParams.Add("@EmployerId", employerId);
                    string storedProc = "[dbo].[Sp_UpdateEmployer]";

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception e)
                {

                }
                
          }
            return true;
         }

        public bool UpdateAssessment(int learnerId, int employerId)
        {
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                try
                {
                    dynamicParams.Add("@LearnerId", learnerId);
                    dynamicParams.Add("@EmployerId", employerId);
                    string storedProc = "[dbo].[Sp_UpdateAssessments]";

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception e)
                {

                }
            }
            return true;
        }

        public bool AddAdditionalActivites(AdditionalActivites model)
        {
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                try
                {
                    var loadData = LoadDocLearnerCourseId(model.Learner_Id);

                    dynamicParams.Add("@learner_Id", model.Learner_Id);
                    dynamicParams.Add("@activity_Title", model.Activity_Title);
                    dynamicParams.Add("@completed_Date", Convert.ToDateTime(model.Completed_Date));
                    dynamicParams.Add("@description", model.Description);
                    dynamicParams.Add("@learner_Course_Id", loadData.LearnerCourses_Id);
                    dynamicParams.Add("@actual_Otj", model.Actual_Otj);
                    string storedProc = "[dbo].[SP_CPD_Additional_Insert]";

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception e)
                {

                }
            }
            return true;
        }

        public bool UpdateCourse(UpdateCourse model)
        {
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                try
                {

                    dynamicParams.Add("@LearnerCourses_Id_CoursesStatus", model.LearnerCourses_Id_CoursesStatus);
                    dynamicParams.Add("@LearnerCourses_CompletedDate", model.LearnerCourses_CompletedDate);
                    dynamicParams.Add("@LearnerCourses_IsCompleted", model.LearnerCourses_IsCompleted);
                    dynamicParams.Add("@LearnerCourses_Id_Learner", model.LearnerCourses_Id_Learner);
                    dynamicParams.Add("@LearnerCourses_Id_Course", model.LearnerCourses_Id_Course);
                    string storedProc = "[dbo].[SP_Osca_UpdateCourse]";

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception e)
                {

                }
            }
            return true;
        } 
        public bool InsertCourse(UpdateCourse model)
        {
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                try
                {

                    dynamicParams.Add("@LearnerCourses_Id_Learner", model.LearnerCourses_Id_Learner);
                    dynamicParams.Add("@LearnerCourses_Id_Course", model.LearnerCourses_Id_Course);
                    dynamicParams.Add("@LearnerCourses_IsCompleted", model.LearnerCourses_IsCompleted);
                    dynamicParams.Add("@LearnerCourses_CompletedDate", model.LearnerCourses_CompletedDate);
                    dynamicParams.Add("@LearnerCourses_IsFunded", model.LearnerCourses_IsFunded);
                    dynamicParams.Add("@LearnerCourses_Id_CoursesStatus", model.LearnerCourses_Id_CoursesStatus);
                    dynamicParams.Add("@LearnerCourses_CreatedByUser", model.LearnerCourses_CreatedByUser);
                    dynamicParams.Add("@LearnerCourses_CreatedDate", model.LearnerCourses_CreatedDate);
                    string storedProc = "[dbo].[SP_Osca_InsertCourse]";

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception e)
                {

                }
            }
            return true;
        }

        public List<UpdateCourse> CheckUpdateCouser(int learnerId, int courseId)
        {
            var list = new List<UpdateCourse>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_CheckCourseUpdate]";
                    object dynamicParams = new
                    {
                        LearnerCourses_Id_Learner = learnerId,
                        LearnerCourses_Id_Course = courseId

                    };

                    list = (List<UpdateCourse>)conn.Query<UpdateCourse>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return list;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public Doc_CourseSummary GenerateCourseSummary(int learnerId)
        {
            try
            {
                Doc_CourseSummary oDoc = new Doc_CourseSummary();

         
         
                var data = LoadSummary(learnerId);

                var WordDocumentmodel = new Doc_CourseSummary();

                if (data != null)
                {
                    WordDocumentmodel.LearnerId = data.LearnerId;
                    WordDocumentmodel.LearnerName = data.LearnerName;
                    WordDocumentmodel.Learner_Class = data.Learner_Class;
                    WordDocumentmodel.LearnerEmail = data.LearnerEmail;
                    WordDocumentmodel.Learner_StartDate = data.Learner_StartDate;
                    WordDocumentmodel.Learner_PlannedEndDate = data.Learner_PlannedEndDate;
                    WordDocumentmodel.Learner_EstimatedEndDate = data.Learner_EstimatedEndDate;
                    WordDocumentmodel.LearnerDOB = data.LearnerDOB;
                    WordDocumentmodel.Learner_CurrentCourse = data.Learner_CurrentCourse;
                    WordDocumentmodel.LearnerCourseId = data.LearnerCourseId;
                    WordDocumentmodel.CoordinatorName = data.CoordinatorName;
                    WordDocumentmodel.todaydate = DateTime.Now.ToShortDateString();
                    WordDocumentmodel.Percentage = data.Percentage;
                    WordDocumentmodel.Planning = "";
                    WordDocumentmodel.NextTopic = data.NextTopic;
                    WordDocumentmodel.NextTopicDueDate = data.NextTopicDueDate;
                    WordDocumentmodel.us_UpdatedOn = DateTime.Now.ToShortDateString();
                    WordDocumentmodel.listItem = ListData(data.LearnerCourseId);
                }
                return WordDocumentmodel;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Doc_CourseSummary LoadSummary(int learnerIId)
        {
            Doc_CourseSummary model = new Doc_CourseSummary();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaTap2_GetLearnerDetails]";
                    object dynamicParams = new
                    {
                        Learner_Id = learnerIId
                    };

                    model = conn.QueryFirstOrDefault<Doc_CourseSummary>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return model;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<Doc_CourseSummary_TopicItems> ListData(int id)
        {
            var list = new List<Doc_CourseSummary_TopicItems>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    string storedProc = "[dbo].[SP_OscaPortal_GetListJourneyLearnerProgressSummary]";
                    object dynamicParams = new
                    {
                        LearnerCourseId = id,
                      

                    };

                    list = (List<Doc_CourseSummary_TopicItems>)conn.Query<Doc_CourseSummary_TopicItems>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return list;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
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



    }
}
