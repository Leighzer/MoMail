using MoMail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.Json;

namespace SayHi
{
    public class Program
    {
        const string emailConfigFileName = "emailConfig.json";

        public static void Main(string[] args)
        {
            string emailArg = args.Count() >= 1 ? args[0] : string.Empty;
            try
            {
                MailAddress emailAddress = new MailAddress(emailArg);
                using (StreamReader r = new StreamReader(emailConfigFileName))
                {
                    string emailConfigJson = r.ReadToEnd();
                    EmailConfig emailConfig = JsonSerializer.Deserialize<EmailConfig>(emailConfigJson);
                    Emailer emailer = new Emailer(emailConfig);

                    var result = emailer.SendEmail(
                        new List<string>() { emailAddress.ToString() },
                        new List<string>(),
                        new List<string>(),
                        new List<string>(),
                        "Hello!",
                        "Hi there!",
                        MailPriority.Normal
                        );

                    if (result.IsSuccess)
                    {
                        Console.WriteLine($"Email send successful!");
                        Console.WriteLine($"Attempt start {result.DateAttempted}");
                        Console.WriteLine($"Sent on {result.DateSent.Value}");
                        Console.WriteLine($"After {result.Attempts.Count} attempts");
                    }
                    else
                    {
                        Console.WriteLine($"Email send unsuccessful!");
                        Console.WriteLine($"Attempt start {result.DateAttempted}");
                        Console.WriteLine($"Attempts made {result.Attempts.Count}");
                        Console.WriteLine($"Error note {result.ErrorNote}");
                        if (result.Exception is not null)
                        {
                            Console.WriteLine($"{result.Exception.Message}");
                            Console.WriteLine($"{result.Exception.StackTrace}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Email send unsuccessful!");
                Console.WriteLine($"{e.Message}");
                Console.WriteLine($"{e.StackTrace}");
            }
        }
    }
}
