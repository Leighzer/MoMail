namespace MoMail
{
    public class EmailConfig
    {
        public string DefaultFrom { get; set; }
        public string DefaultSender { get; set; }

        public string MailServer { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public int MaxAttempts { get; set; }
        public bool IsBodyHtml { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
