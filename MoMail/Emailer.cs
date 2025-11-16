using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace MoMail
{
    public class Emailer
    {
        private readonly EmailConfig config;

        public Emailer(EmailConfig config)
        {
            this.config = config;
        }

        public SendResult SendEmail(List<string> to, List<string> cc, List<string> bcc, List<string> replyTo, string subject, string body, MailPriority priority = MailPriority.Normal)
        {
            return SendEmail(config.DefaultSender, config.DefaultFrom, to, cc, bcc, replyTo, subject, body, priority);
        }

        public SendResult SendEmail(string sender, string from, List<string> to, List<string> cc, List<string> bcc, List<string> replyTo, string subject, string body, MailPriority priority = MailPriority.Normal)
        {
            // Try to initialize outgoing email
            MailMessage outgoingMessage;
            try
            {
                outgoingMessage = new MailMessage()
                {
                    Sender = new MailAddress(sender),
                    From = new MailAddress(from),
                    Subject = subject,
                    Body = body,
                    Priority = priority,
                    IsBodyHtml = config.IsBodyHtml
                };

                for (int j = 0; j < to.Count; j++)
                {
                    outgoingMessage.To.Add(new MailAddress(to[j]));
                }
                for (int j = 0; j < cc.Count; j++)
                {
                    outgoingMessage.CC.Add(new MailAddress(cc[j]));
                }
                for (int j = 0; j < bcc.Count; j++)
                {
                    outgoingMessage.Bcc.Add(new MailAddress(bcc[j]));
                }
                for (int j = 0; j < replyTo.Count; j++)
                {
                    outgoingMessage.ReplyToList.Add(new MailAddress(replyTo[j]));
                }
            }
            catch (Exception ex)
            {
                return new SendResult()
                {
                    IsSuccess = false,
                    DateAttempted = DateTime.Now,
                    Exception = ex,
                    ErrorNote = "Failure to initialize mail message."
                };
            }

            // Try to initialize SMTP client
            SmtpClient smtpClient;
            try
            {
                smtpClient = GetSmtpClient();
            }
            catch (Exception ex)
            {
                return new SendResult()
                {
                    IsSuccess = false,
                    DateAttempted = DateTime.Now,
                    Exception = ex,
                    ErrorNote = "Failure to initialize SMTP client."
                };
            }

            // Try to send out email
            SendResult sendResult = new SendResult()
            {
                IsSuccess = false,
                DateAttempted = DateTime.Now,
            };
            for (int i = 0; i < config.MaxAttempts; i++)
            {
                DateTime dateAttempted = DateTime.Now;
                try
                {
                    smtpClient.Send(outgoingMessage);
                    DateTime dateSent = DateTime.Now;
                    sendResult.Attempts.Add(new SendAttempt()
                    {
                        IsSuccess = true,
                        DateAttempted = dateAttempted,
                        DateSent = dateSent,
                        Email = outgoingMessage
                    });
                    sendResult.IsSuccess = true;
                    sendResult.DateSent = dateSent;
                    return sendResult;
                }
                catch (Exception ex)
                {
                    sendResult.Attempts.Add(new SendAttempt()
                    {
                        IsSuccess = false,
                        DateAttempted = dateAttempted,
                        Email = outgoingMessage,
                        Exception = ex,
                        ErrorNote = "Failure to send email.",
                    });
                }
            }

            sendResult.ErrorNote = "Failure to send all emails.";
            return sendResult;
        }

        public SmtpClient GetSmtpClient()
        {
            return new SmtpClient()
            {
                Host = config.MailServer,
                Port = config.Port,
                Credentials = new NetworkCredential(config.UserName, config.Password),
                EnableSsl = config.UseSsl,
            };
        }
    }
}
