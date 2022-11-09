using ds.core.emailtemplates.Models;
using portal.models.Comman;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ds.core.emailtemplates
{
    public interface IEmailTemplateRepository
    {
        IEnumerable<EmailTemplateViewModel> GetAllEmailTemplates();
        IEnumerable<EmailTemplate> GetTemplatesForDropdownByType(int typeId);
        EmailTemplate GetEmailTemplateById(int et_Id);        
        bool InsertUpdateEmailTemplate(EmailTemplate emailTemplate);
        bool DeleteEmailTemplateById(int et_Id, int userId, string userName);
        Tuple<bool, bool> CheckTitleAndCodeExists(EmailTemplate emailTemplate);
        EmailTemplate GetEmailTemplateByCode(string code);
        Tuple<string, string> GetSubjectAndEmailTemplateBodyReplacedByCode(string template_code, Hashtable ht);
        bool SaveQueueEmail(QueueEmails queueEmails);
        bool UpdateQueueEmail(int id);

        List<QueueEmails> GetQueueEmailByNumber(int number);
        bool InsertUpdateQueueEmail(QueueEmails emailTemplate);

    }
}
