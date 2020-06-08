using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailService
{

    public class Email
    {
        public ICollection<MailboxAddress> Recipients { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public IEnumerable <byte[]> Attachments { get; set; }
        public IEnumerable<string> FileNames { get; set; }
        public MailboxAddress Recipient { get; set; }
        public bool HasManyRecipients { get; set; }

        public Email (string recipient, string subject, string content)
        {
            Recipient = new MailboxAddress(recipient);
            Subject = subject;
            Content = content;
            HasManyRecipients = false;

        }

        public Email(IEnumerable<string> recipients, string subject, string content, IEnumerable <byte[]> attachments, IEnumerable<string> fileNames)
        {
            Recipients = new List<MailboxAddress>();
            foreach (String recipient in recipients)
            {
                Recipients.Add(new MailboxAddress(recipient) );
            }
            Subject = subject;
            Content = content;
            Attachments = attachments;
            FileNames = fileNames;
            HasManyRecipients = true;
        }

    }

}
