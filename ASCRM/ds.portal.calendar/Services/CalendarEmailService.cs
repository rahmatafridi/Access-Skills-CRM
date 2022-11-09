using ds.core.common;
using ds.core.comms.Mail;
using ds.core.emailtemplates;
using ds.portal.calendar.Constants;
using ds.portal.users;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ds.portal.calendar.Services
{
    public class CalendarEmailService : ICalendarEmailService
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
        /// Initializes a new instance of the <see cref="CalendarEmailService"/> class.
        /// </summary>
        /// <param name="mailService">The mail service.</param>
        /// <param name="emailTemplateRepository">The email template repository.</param>
        /// <param name="lead_UserRepository">The lead user repository.</param>
        public CalendarEmailService(
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
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        public async Task SendCalendarUpdate(CalendarEvent calendarEvent)
        {
            if (calendarEvent.reason == Calendar.Reason.BOOKING)
            {
                var user = _userRepository.GetUserById(calendarEvent.user_id);
                var emailTemplate = _emailTemplateRepository.GetEmailTemplateByCode(EMAIL_TEMPLATE_CODE);
                if (user != null && !string.IsNullOrEmpty(user.Users_Email) && !string.IsNullOrEmpty(user.Users_DisplayName) &&
                    emailTemplate != null && !string.IsNullOrEmpty(emailTemplate.ET_Subject) && !string.IsNullOrEmpty(emailTemplate.ET_Body))
                {
                    emailTemplate.ET_Body = emailTemplate.ET_Body.ReplaceMultipleValues(new List<Tuple<string, string>>()
                {
                    new Tuple<string, string>("[ASSESSOR]", user.Users_DisplayName),
                    new Tuple<string, string>("[LEARNER]", calendarEvent.learner),
                    new Tuple<string, string>("[TOPIC]", calendarEvent.topic),
                    new Tuple<string, string>("[START]", calendarEvent.event_start.ToString()),
                    new Tuple<string, string>("[END]", calendarEvent.event_end.ToString()),
                });

                    await _mailService.SendEmailAsync(new string[] { user.Users_Email }, emailTemplate.ET_Subject, emailTemplate.ET_Body);
                }
            }
        }
    }
}
