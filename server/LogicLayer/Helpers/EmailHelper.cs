﻿using System;
using System.Net;
using System.Net.Mail;

namespace LogicLayer.Helpers
{
    public class EmailHelper
    {
        public static void Send(string email)
        {
            var emailConfig = ConfigurationHelper.Configuration.GetSection("EmailConfiguration");
            SmtpClient client = new SmtpClient(emailConfig["SmtpServer"], Int32.Parse(emailConfig["Port"]));
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(emailConfig["Username"], emailConfig["Password"]);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(emailConfig["From"]);
            mailMessage.To.Add(email);
            mailMessage.Subject = emailConfig["Subject"];
            mailMessage.Body = emailConfig["Body"];
            client.SendAsync(mailMessage, "message");
        }
    }
}
