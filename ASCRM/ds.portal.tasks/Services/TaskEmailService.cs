using ds.core.common;
using ds.core.comms.Mail;
using ds.core.emailtemplates;
using ds.portal.tasks.Constants;
using ds.portal.users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ds.portal.tasks.Services
{
    public class TaskEmailService : ITaskEmailService
    {
        /// <summary>
        /// The email template code
        /// </summary>
        private const string EMAIL_TEMPLATE_CODE = "CAL_BOOKED";
        /// <summary>
        /// The mail service
        /// </summary>
        private readonly IMailService _mailService;
        /// <summary>
        /// The email template repository
        /// </summary>
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly ILead_UserRepository _userRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEmailService"/> class.
        /// </summary>
        /// <param name="mailService">The mail service.</param>
        /// <param name="emailTemplateRepository">The email template repository.</param>
        /// <param name="lead_UserRepository">The lead user repository.</param>
        public TaskEmailService(
            IMailService mailService,
            IEmailTemplateRepository emailTemplateRepository,
            ILead_UserRepository lead_UserRepository)
        {
            _mailService = mailService;
            _emailTemplateRepository = emailTemplateRepository;
            _userRepository = lead_UserRepository;
        }
        /// <summary>
        /// Sends the calendar update.
        /// </summary>
        /// <param name="taskEvent">The calendar event.</param>
        /// <returns></returns>
        public async Task SendCalendarUpdate(TaskEvent taskEvent)
        {
            if (taskEvent.reason == TaskReason.Reason.BOOKING)
            {
                var user = _userRepository.GetUserById(taskEvent.task_id_user);
                var emailTemplate = _emailTemplateRepository.GetEmailTemplateByCode(EMAIL_TEMPLATE_CODE);
                if (user != null && !string.IsNullOrEmpty(user.Users_Email) && !string.IsNullOrEmpty(user.Users_DisplayName) &&
                    emailTemplate != null && !string.IsNullOrEmpty(emailTemplate.ET_Subject) && !string.IsNullOrEmpty(emailTemplate.ET_Body))
                {
                    emailTemplate.ET_Body = emailTemplate.ET_Body.ReplaceMultipleValues(new List<Tuple<string, string>>()
                {
                    new Tuple<string, string>("[ASSESSOR]", user.Users_DisplayName),
                    new Tuple<string, string>("[LEARNER]", taskEvent.learner),
                    new Tuple<string, string>("[TOPIC]", taskEvent.topic),
                    new Tuple<string, string>("[START]", taskEvent.task_start.ToString()),
                    new Tuple<string, string>("[END]", taskEvent.task_end.ToString()),
                });

                    await _mailService.SendEmailAsync(new string[] { user.Users_Email }, emailTemplate.ET_Subject, emailTemplate.ET_Body);
                }
            }
        }
    }
}
