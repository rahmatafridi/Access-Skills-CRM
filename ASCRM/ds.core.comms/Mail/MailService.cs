using ds.core.configuration;
using ds.core.emailtemplates;
using Microsoft.AspNetCore.Http;
using portal.models.Comman;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ds.core.comms.Mail
{
    public class MailService : IMailService
    {
        private IConfigurationRepository configuration;
        private string toEmailId = string.Empty;
        private string toDisplayName = string.Empty;
        private string fromEmailId = string.Empty;
        private string fromDisplayName = string.Empty;
        private string smtp = string.Empty;
        private string networkId = string.Empty;
        private string networkPassword = string.Empty;
        private string support = string.Empty;
        private int port = 0;
        private bool useSSL = false;
        IEmailTemplateRepository _emailTemplateRepository;

        public MailService(IConfigurationRepository iConfig , IEmailTemplateRepository emailTemplateRepository)
        {
             configuration = iConfig;
             toEmailId = configuration.GetSingleConfigValueByConfigKey("ToEmail");
             toDisplayName = configuration.GetSingleConfigValueByConfigKey("ToDisplayName");
             fromEmailId = configuration.GetSingleConfigValueByConfigKey("FromEmail");
             fromDisplayName = configuration.GetSingleConfigValueByConfigKey("FromDisplayName");
             smtp = configuration.GetSingleConfigValueByConfigKey("SMTPClient");
             networkId = configuration.GetSingleConfigValueByConfigKey("NetworkUserId");
             networkPassword = configuration.GetSingleConfigValueByConfigKey("NetworkPassword");
             port = Convert.ToInt32(configuration.GetSingleConfigValueByConfigKey("Port"));
             useSSL = Convert.ToBoolean(configuration.GetSingleConfigValueByConfigKey("UseSSL"));
            support = configuration.GetSingleConfigValueByConfigKey("supportContact");
            _emailTemplateRepository = emailTemplateRepository;
        }

        public bool SendEmail(string email, string subject, string message)
        {
            //send email from here
            // implement smtp?
            try
            {                
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.To.Add(new MailAddress(email));
                    mailMessage.To.Add(new MailAddress(toEmailId, toDisplayName));
                    mailMessage.From = new MailAddress(fromEmailId, fromDisplayName);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;                                     

                    using (var client = new SmtpClient(smtp))
                    {
                        client.Port = port;
                        client.Credentials = new NetworkCredential(networkId,networkPassword);
                        client.EnableSsl = useSSL;
                        client.Send(mailMessage);
                    }
                }

                Debug.WriteLine($"Sending Mail to : {email}, subject: {subject} ");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public bool SendEmailPortalContact( string subject, string message,string userId)
        {
            //send email from here
            // implement smtp?
            try
            {
                var emailModel = new QueueEmails();
                emailModel.qe_bcc = toEmailId;
                emailModel.qe_body = message;
                emailModel.qe_cc = "";
                emailModel.qe_created_by = userId;
                emailModel.qe_created_date = DateTime.Now;
                emailModel.qe_from = fromEmailId;
                emailModel.qe_is_html = true;
                emailModel.qe_mod_date = DateTime.Now;
                emailModel.qe_priority = 1;
                emailModel.qe_sent = false;
                emailModel.qe_status = "";
                emailModel.qe_subject = "Learner Contact";
                emailModel.qe_to = support;
                emailModel.qe_system_id = 3;
                _emailTemplateRepository.SaveQueueEmail(emailModel);

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.To.Add(new MailAddress(support));
                    mailMessage.To.Add(new MailAddress(toEmailId, toDisplayName));
                    mailMessage.From = new MailAddress(fromEmailId, fromDisplayName);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;

                    using (var client = new SmtpClient(smtp))
                    {
                        client.Port = port;
                        client.Credentials = new NetworkCredential(networkId, networkPassword);
                        client.EnableSsl = useSSL;
                        client.Send(mailMessage);
                    }
                }

                Debug.WriteLine($"Sending Mail to : {support}, subject: {subject} ");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool SendEmailWithAttachmentsBytes(string email, string subject, string message, byte[] FormFiles, string filename)
        {
            //send email from here
            // implement smtp?
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.To.Add(new MailAddress(email));
                    //mailMessage.To.Add(new MailAddress(toEmailId, toDisplayName));
                    mailMessage.From = new MailAddress(fromEmailId, fromDisplayName);
                    //mailMessage.CC.Add(new MailAddress("supubaidfdesigns@gmail.com", "CC Name"));
                    //mailMessage.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;

                    Attachment att = new Attachment(new MemoryStream(FormFiles), filename);
                    mailMessage.Attachments.Add(att);
                    using (var client = new SmtpClient(smtp))
                    {
                        client.Port = port;
                        client.Credentials = new NetworkCredential(networkId, networkPassword);
                        client.EnableSsl = useSSL;
                        client.Send(mailMessage);
                    }
                }

                Debug.WriteLine($"Sending Mail to : {email}, subject: {subject} ");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool SendEmailWithAttachments(string email, string subject, string message, List<IFormFile> FormFiles)
        {
            //send email from here
            // implement smtp?
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.To.Add(new MailAddress(email));
                    mailMessage.To.Add(new MailAddress(toEmailId,toDisplayName));
                    mailMessage.From = new MailAddress(fromEmailId, fromDisplayName);
                    //mailMessage.CC.Add(new MailAddress("supubaidfdesigns@gmail.com", "CC Name"));
                    //mailMessage.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;
                   
                    //foreach (var file in files)
                    //{
                    //    // Specify the file to be attached and sent.
                    //    // This example assumes that a file named Data.xls exists in the
                    //    // current working directory.
                    //    //string file = "data.xls";

                    //    // Create  the file attachment for this email message.
                    //    Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                    //    // Add time stamp information for the file.
                    //    ContentDisposition disposition = data.ContentDisposition;
                    //    disposition.CreationDate = System.IO.File.GetCreationTime(file);
                    //    disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                    //    disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                    //    // Add the file attachment to this email message.
                    //    mailMessage.Attachments.Add(data);
                    //}
                    
                    foreach (var file in FormFiles)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                Attachment att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                                mailMessage.Attachments.Add(att);
                            }
                        }
                    }

                    using (var client = new SmtpClient(smtp))
                    {
                        client.Port = port;
                        client.Credentials = new NetworkCredential(networkId, networkPassword);
                        client.EnableSsl = useSSL;
                        client.Send(mailMessage);
                    }
                }

                Debug.WriteLine($"Sending Mail to : {email}, subject: {subject} ");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public Task SendEmailWithAttachmentsAsync(string email, string subject, string message, List<IFormFile> FormFiles, List<string> defaultAttchments = null)
        {
            //send email from here
            // implement smtp?
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.To.Add(new MailAddress(email));
                    mailMessage.To.Add(new MailAddress(toEmailId, toDisplayName));
                    mailMessage.From = new MailAddress(fromEmailId, fromDisplayName);
                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                    mailMessage.IsBodyHtml = true;

                    foreach (var file in FormFiles)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                var fileBytes = ms.ToArray();
                                Attachment att = new Attachment(new MemoryStream(fileBytes), file.FileName);
                                mailMessage.Attachments.Add(att);
                            }
                        }
                    }

                    if (defaultAttchments != null && defaultAttchments.Count > 0)
                    {
                        foreach (var file in defaultAttchments)
                        {
                            // Specify the file to be attached and sent.
                            // This example assumes that a file named Data.xls exists in the
                            // current working directory.
                            //string file = "data.xls";

                            // Create  the file attachment for this email message.
                            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                            // Add time stamp information for the file.
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(file);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                            // Add the file attachment to this email message.
                            mailMessage.Attachments.Add(data);
                        }
                    }

                    using (var client = new SmtpClient(smtp))
                    {
                        client.Port = port;
                        client.Credentials = new NetworkCredential(networkId,networkPassword);
                        client.EnableSsl = useSSL;
                        client.Send(mailMessage);
                    }
                }

                Debug.WriteLine($"Sending Mail to : {email}, subject: {subject} ");

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="recipientEmails">The recipients.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="isHtml">if set to <c>true</c> [is HTML].</param>
        /// <param name="enableSSL">if set to <c>true</c> [enable SSL].</param>
        public async Task SendEmailAsync(string[] recipientEmails, string subject, string message, bool isHtml = true, bool enableSSL = true)
        {
            using (var mailMessage = new MailMessage())
            {
                foreach (var recipientEmail in recipientEmails)
                {
                    mailMessage.To.Add(new MailAddress(recipientEmail));
                }
                mailMessage.To.Add(new MailAddress(toEmailId, toDisplayName));
                mailMessage.From = new MailAddress(fromEmailId, fromDisplayName);
                mailMessage.Subject = subject;
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = isHtml;

                using (var client = new SmtpClient(smtp))
                {
                    client.Port = port;
                    client.Credentials = new NetworkCredential(networkId, networkPassword);
                    client.EnableSsl = enableSSL;
                    await client.SendMailAsync(mailMessage);
                }
            }

            Debug.WriteLine($"Sending Mail to : {recipientEmails}, subject: {subject} ");
        }


        public Task SendHtmlEmailNotificationAsync(string email, string subject, string message)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.To.Add(new MailAddress(email));
                    //mailMessage.To.Add(new MailAddress(toEmailId));
                    mailMessage.From = new MailAddress(fromEmailId, fromDisplayName);
                   // mailMessage.Bcc.Add(new MailAddress(toEmailId,toDisplayName));
                    //mailMessage.CC.Add(new MailAddress(emailcc));

                    mailMessage.Subject = subject;
                    mailMessage.Body = message;
                  // mailMessage.BodyEncoding = Encoding.UTF8;

                    mailMessage.IsBodyHtml = true;
                    //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(defaultAttchments);
                    //mailMessage.Attachments.Add(attachment);
                    //  var client = new SmtpClient();
                    //SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    

                    ////client.Port = port;
                    // client.Credentials = new NetworkCredential(networkId, networkPassword);
                    //client.EnableSsl = true;
                    //client.UseDefaultCredentials = false;
                    //client.Send(mailMessage);

                    using (var client = new SmtpClient(smtp))
                    {
                        client.Port = port;
                        client.Credentials = new NetworkCredential(networkId, networkPassword);
                        client.EnableSsl = useSSL;
                        client.Send(mailMessage);
                    }
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool SaveEmailPortal(string subject, string message, string fromUser, string toUser, bool isAdmin)
        {
            try
            {
                var emailModel = new QueueEmails();
                if (isAdmin)
                {
                    emailModel.qe_bcc = toEmailId;
                }
                else
                {
                    emailModel.qe_bcc = toUser;
                }
                emailModel.qe_body = message;
                emailModel.qe_cc = "";
                emailModel.qe_created_by = fromUser;
                emailModel.qe_created_date = DateTime.Now;

                emailModel.qe_from = fromUser;
                emailModel.qe_is_html = true;
                emailModel.qe_mod_date = DateTime.Now;
                emailModel.qe_priority = 1;
                emailModel.qe_sent = false;
                emailModel.qe_status = "";
                emailModel.qe_subject = subject;
                emailModel.qe_to = support;
                emailModel.qe_system_id = 3;
                _emailTemplateRepository.SaveQueueEmail(emailModel);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
