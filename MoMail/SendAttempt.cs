using System;
using System.Net.Mail;

namespace MoMail
{
    /// <summary>
    /// Holds information pertaining to a single send attempt
    /// </summary>
    public class SendAttempt
    {
        public bool IsSuccess { get; set; }
        public DateTime DateAttempted { get; set; }
        public DateTime? DateSent { get; set; }
        public MailMessage Email { get; set; }
        public Exception Exception { get; set; }
        public string ErrorNote { get; set; }
    }
}
