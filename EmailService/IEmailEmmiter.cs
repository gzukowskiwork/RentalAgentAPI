using EmailService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public interface IEmailEmmiter
    {
        void SendMail(Email message);
        Task SendMailAsync(Email message);
        Task SendResetEmail(Email message);
        Task SendChangeEmailEmail(Email message);
        Task SendUserRegistrationConfirmationEmail(Email message);
    }
}
