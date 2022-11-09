using Dapper;
using ds.core.emailtemplates.Models;
using ds.core.uow;
using portal.models.Comman;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ds.core.emailtemplates
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public EmailTemplateRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }
        public IEnumerable<EmailTemplateViewModel> GetAllEmailTemplates()
        {
            IEnumerable<EmailTemplateViewModel> emailTemplates = new List<EmailTemplateViewModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdCore_EmailTemplate_GetAll]";
                    //object dynamicParams = new { Type = "Select" };

                    emailTemplates = conn.Query<EmailTemplateViewModel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return emailTemplates;
        }
        public IEnumerable<EmailTemplate> GetTemplatesForDropdownByType(int typeId)
        {
            IEnumerable<EmailTemplate> emailTemplates = new List<EmailTemplate>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdCore_EmailTemplate_GetForDropdownByType]";
                    object dynamicParams = new { ET_Type = typeId };

                    emailTemplates = conn.Query<EmailTemplate>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return emailTemplates;
        }
        public EmailTemplate GetEmailTemplateById(int et_Id)
        {
            EmailTemplate emailTemplate = new EmailTemplate();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdCore_EmailTemplate_GetById]";
                    object dynamicParams = new { ET_Id = et_Id };

                    emailTemplate = conn.QuerySingleOrDefault<EmailTemplate>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return emailTemplate;
        }
        public bool InsertUpdateEmailTemplate(EmailTemplate emailTemplate)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    emailTemplate.ET_Body = FormatStringForSQL(emailTemplate.ET_Body);
                    if (emailTemplate.ET_Id > 0)
                    {
                        string storedProc = "[dbo].[SP_mdCore_EmailTemplate_Update]";
                        object dynamicParams = emailTemplate;

                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {                        
                        string storedProc = "[dbo].[SP_mdCore_EmailTemplate_Insert]";
                        object dynamicParams = emailTemplate;

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
        public bool DeleteEmailTemplateById(int config_id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdCore_EmailTemplate_DeleteById]";
                    object dynamicParams = new { ET_Id = config_id, ET_DeletedByUser = userId };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public Tuple<bool, bool> CheckTitleAndCodeExists(EmailTemplate emailTemplate)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdCore_EmailTemplate_CheckTitleAndCodeExists]";
                    object dynamicParams = new
                    {
                        ET_Id = emailTemplate.ET_Id,
                        ET_Title = emailTemplate.ET_Title,
                        ET_Code = emailTemplate.ET_Code
                    };

                    var dynamic = conn.QuerySingleOrDefault<dynamic>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    var fields = dynamic as IDictionary<string, object>;

                    return Tuple.Create<bool, bool>(Convert.ToBoolean(fields["TitleExists"]), Convert.ToBoolean(fields["CodeExists"]));
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public static string FormatStringForSQL(string str)
        {
            if (str == null)
                str = "";
            str = str.Replace("'", "''");
            return str;
        }

        public EmailTemplate GetEmailTemplateByCode(string code)
        {
            EmailTemplate emailTemplate = new EmailTemplate();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_EmailTemplate_GetEmailTemplatesByCode]";
                    object dynamicParams = new { @ETCode = code };

                    emailTemplate = conn.QuerySingleOrDefault<EmailTemplate>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return emailTemplate;
        }

        public Tuple<string, string> GetSubjectAndEmailTemplateBodyReplacedByCode(string template_code, Hashtable ht)
        {            
            EmailTemplate emailTemplate = GetEmailTemplateByCode(template_code);
           
            string sRowBody = "";
            string sSubject = "";

            if (!string.IsNullOrEmpty(emailTemplate.ET_Body))
            {
                try
                {
                    sRowBody = emailTemplate.ET_Body;
                    sSubject = emailTemplate.ET_Subject;
                }
                catch (Exception ex)
                {
                    sRowBody = "";
                }
            }
            else
            {
                sRowBody = "";
            }
            IDictionaryEnumerator _enumerator = ht.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                sRowBody = sRowBody.Replace(_enumerator.Key.ToString(), _enumerator.Value.ToString());
            }
            return Tuple.Create(sSubject, sRowBody);
        }

        public bool SaveQueueEmail(QueueEmails queueEmails)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    if (queueEmails.qe_to != "")
                    {
                        string storedProc = "[dbo].[SP_Email_Insert]";
                        var p = new DynamicParameters();

                        p.Add("@qe_from", queueEmails.qe_from);
                        p.Add("@qe_to", queueEmails.qe_to);
                        p.Add("@qe_subject", queueEmails.qe_subject);
                        p.Add("@qe_body", queueEmails.qe_body);
                        p.Add("@qe_is_html", queueEmails.qe_is_html);
                        p.Add("@qe_cc", queueEmails.qe_cc);
                        p.Add("@qe_bcc", queueEmails.qe_bcc);
                        p.Add("@qe_priority", queueEmails.qe_priority);
                        p.Add("@qe_sent", queueEmails.qe_sent);
                        p.Add("@qe_status", queueEmails.qe_status);
                        p.Add("@qe_created_by", queueEmails.qe_created_by);
                        p.Add("@qe_created_date", queueEmails.qe_created_date);
                        p.Add("@qe_mod_date", queueEmails.qe_mod_date);
                        p.Add("@qe_system_id", queueEmails.qe_system_id);
                        int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);



                    }
                }
                catch (Exception ex)
                {

                    throw;
                }


                return true;
            }

        }

        public List<QueueEmails> GetQueueEmailByNumber(int number)
        {
            List<QueueEmails> emailTemplates = new List<QueueEmails>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_GET_QUEUEEMAIL]";
                    object dynamicParams = new { number = number };

                    emailTemplates = (List<QueueEmails>)conn.Query<QueueEmails>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return emailTemplates;
        }

        public bool InsertUpdateQueueEmail(QueueEmails queueEmails)
        {

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    if (queueEmails.qe_id !=0)
                    {
                        string storedProc = "[dbo].[SP_UPDATE_EMAIL]";
                        var p = new DynamicParameters();

                        p.Add("@qe_id", queueEmails.qe_id);
            
                        int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);



                    }
                }
                catch (Exception ex)
                {

                    throw;
                }


                return true;
            }
        }

        public bool UpdateQueueEmail(int id)
        {
            throw new NotImplementedException();
        }
    }
}
