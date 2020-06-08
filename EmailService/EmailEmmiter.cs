using Contracts;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{

    /* 
     * http://www.mimekit.net/docs/html/P_MimeKit_BodyBuilder_Attachments.htm
     * More infor on BodyBuilder (Just Documentation. Not yet another Nutrition site).
     */

    public class EmailEmmiter : IEmailEmmiter
    {
        private readonly EmailConfig _emailConfig;
        private readonly ILoggerManager _logger;
        

        public EmailEmmiter(EmailConfig emailConfig, ILoggerManager logger)
        {
            _emailConfig = emailConfig;
            _logger = logger;
        }

        public void SendMail(Email message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public async Task SendMailAsync(Email message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        public async Task SendResetEmail(Email message)
        {
            var emailMessage = CreateEmailForPasswordReset(message);
            await SendAsync(emailMessage);
        }

        public async Task SendChangeEmailEmail(Email message)
        {
            var emailMessage = CreateEmailForEmailReset(message);
            await SendAsync(emailMessage);
        }

        public async Task SendUserRegistrationConfirmationEmail(Email message)
        {
            var emailMessage = CreateEmailForUserRegistrationConfirmation(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailForUserRegistrationConfirmation(Email message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.Add(message.Recipient);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = string.Format($"" +
                $"<h3>Wiadomość wygenerowana automatycznie</h3>" +
                $"<h3>Witamy w serwisie RentalAgent!<h3>" +
                $"<p style='color:red;'>{message.Content}</p>" +
                $"<a href='http://192.166.218.136:4200/' style='color:blue;'>Link do serwisu!</a>")
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private MimeMessage CreateEmailForPasswordReset(Email message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.Add(message.Recipient);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = string.Format($""+
                $"<h3>Wiadomość wygenerowana automatycznie</h3>"+
                $"<h3>Oto token do resetu hasła, skopiuj go i wklej na stronie do której zostałeś przekierowany<h3>"+
                $"<p style='color:red;'>{message.Content}</p>" +
                $"<a href='http://192.166.218.136:4200/authenticate/forgot-password' style='color:blue;'>Lub kliknij tutaj!</a>")
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private MimeMessage CreateEmailMessage(Email message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.Recipients);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format($"" +
                $"<h2 style='color:black'>Wiadomość wygenerowana automatycznie. Proszę nie odpowiadać.</h2>" +
                $"<h2 style='color:red;'>{message.Content}</h2>" ) + 
                $"<h3 style='color:gray'>Jeśli potrzebujesz skontaktować się z Wynajmującym, proszę pisać na e-mail właściciela.</h3>"};

            if (message.Attachments != null && message.Attachments.Any())
            {
                int i = 0;
                foreach (var attachment in message.Attachments)
                {
                    bodyBuilder.Attachments.Add(message.FileNames.ToList()[i], attachment, ContentType.Parse("application/pdf"));
                    i++;
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private MimeMessage CreateEmailForEmailReset(Email message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.Add(message.Recipient);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = string.Format($"" +
               $"<h3>Wiadomość wygenerowana automatycznie</h3>" +
               $"<h3>Oto token do zmiany emaila, skopiuj go i wklej na stronie do której zostałeś przekierowany<h3>" +
               $"<p style='color:red;'>{message.Content}</p>")
            };

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Email could not be sent. Failure in EmailEmmiter.Send() method. Error MSG: {e.Message}.");
                    throw e;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Email to: reciepents[{mailMessage.To}] could not be sent. Failure in EmailEmmiter.Send() method. Error MSG: {e.Message}.");
                    throw e;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
