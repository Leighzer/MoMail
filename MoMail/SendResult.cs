using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MoMail
{
    /// <summary>
    /// Holds information pertaining to an entire set of send attempts
    /// </summary>
    public class SendResult
    {
        public bool IsSuccess { get; set; }        
        public DateTime DateAttempted { get; set; }
        public DateTime? DateSent { get; set; }        
        public List<SendAttempt> Attempts { get; set; } = new List<SendAttempt>();
        public Exception Exception { get; set; }
        public string ErrorNote { get; set; }
    }
}
