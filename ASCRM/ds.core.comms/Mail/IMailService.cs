using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ds.core.comms.Mail
{
    public interface IMailService
    {
        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="recipientEmails">The recipient emails.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="htmlMessage">The HTML message.</param>
        /// <param name="isHtml">if set to <c>true</c> [is HTML].</param>
        /// <param name="enableSSL">if set to <c>true</c> [enable SSL].</param>
        /// <returns></returns>
        Task SendEmailAsync(string[] recipientEmails, string subject, string htmlMessage, bool isHtml = true, bool enableSSL = true);
        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="htmlMessage">The HTML message.</param>
        /// <returns></returns>
        bool SendEmail(string email, string subject, string htmlMessage);

        bool SendEmailPortalContact(string subject, string htmlMessage, string userId);

        bool SaveEmailPortal(string subject, string htmlMessage, string fromUser,string toUser,bool isAdmin);

        /// <summary>
        /// Sends the email with attachments.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="htmlMessage">The HTML message.</param>
        /// <param name="FormFiles">The form files.</param>
        /// <returns></returns>
        /// 

        bool SendEmailWithAttachments(string email, string subject, string htmlMessage, List<IFormFile> FormFiles);
        /// <summary>
        /// Sends the email with attachments asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="htmlMessage">The HTML message.</param>
        /// <param name="FormFiles">The form files.</param>
        /// <param name="defaultAttchments">The default attchments.</param>
        /// <returns></returns>
        Task SendEmailWithAttachmentsAsync(string email, string subject, string htmlMessage, List<IFormFile> FormFiles, List<string> defaultAttchments = null);


        Task SendHtmlEmailNotificationAsync(string email, string subject, string message);
        bool SendEmailWithAttachmentsBytes(string email, string subject, string message, byte[] FormFiles, string filename);

    }
}
