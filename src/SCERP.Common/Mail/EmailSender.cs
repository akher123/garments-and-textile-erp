using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace SCERP.Common.Mail
{
    public sealed class EmailSender : IDisposable
    {
        private readonly SmtpClient _smtp;
        private readonly List<Attachment> attachments;
        private readonly List<AlternateView> alternateViews;

        public EmailSender(string host, int port, string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("User name or password not valid");
            }
            _smtp = new SmtpClient(host, port);
            _smtp.Credentials = new NetworkCredential(userName, password);

            attachments = new List<Attachment>();
            alternateViews = new List<AlternateView>();
        }

        MailAddress fromAddress;

        public string From
        {
            get { return fromAddress.ToString(); }
            set { fromAddress = new MailAddress(value); }
        }

        MailAddressCollection replyToAddresses = new MailAddressCollection();

        public string ReplyTo
        {
            get { return string.Join("; ", replyToAddresses.Select(x => x.ToString())); }
            set { replyToAddresses.Add(value); }
        }

        public string Subject
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }


        MailAddressCollection toAddresses = new MailAddressCollection();

        public string To
        {
            get { return string.Join(",", toAddresses.Select(x => x.Address)); }
            set
            {
                toAddresses.Clear();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    toAddresses.Add(string.Join(",", value.Split(new[] { ";", "," }, System.StringSplitOptions.RemoveEmptyEntries)));
                }
            }
        }

        MailAddressCollection ccAddresses = new MailAddressCollection();

        public string CC
        {
            get { return string.Join(",", ccAddresses.Select(x => x.Address)); }
            set
            {
                ccAddresses.Clear();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    ccAddresses.Add(value.Replace(";", ","));
                }
            }
        }

        MailAddressCollection bccAddresses = new MailAddressCollection();

        public string BCC
        {
            get { return string.Join(",", bccAddresses.Select(x => x.Address)); }
            set
            {
                bccAddresses.Clear();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    bccAddresses.Add(value.Replace(";", ","));
                }
            }
        }

        public void Send(bool isBodyHtml = true)
        {
            using (MailMessage message = new MailMessage())
            {
                message.From = fromAddress;

                foreach (MailAddress address in replyToAddresses)
                {
                    message.ReplyToList.Add(address);
                }

                foreach (MailAddress address in toAddresses)
                {
                    message.To.Add(address);
                }

                foreach (MailAddress address in ccAddresses)
                {
                    message.CC.Add(address);
                }

                foreach (MailAddress address in bccAddresses)
                {
                    message.Bcc.Add(address);
                }

                message.Subject = Subject;
                message.IsBodyHtml = isBodyHtml;

                message.Body = Body;
                //if (isBodyHtml)
                //{
                //    AlternateView alternate = AlternateView.CreateAlternateViewFromString(message.Body, new ContentType("text/html"));
                //    message.AlternateViews.Add(alternate);
                //}

                foreach (AlternateView view in alternateViews)
                {
                    message.AlternateViews.Add(view);
                }

                foreach (var attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }

                if (message.To.Count > 0)
                {
                    _smtp.Send(message);
                }
            }
        }

        public void AddAttachment(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
            {
                attachments.Add(new Attachment(fileName));
            }
        }

        public void AddAttachment(byte[] data, string name, string mediaType)
        {
            AddAttachment(new MemoryStream(data), name, mediaType);
        }

        public void AddAttachment(Stream stream, string name, string mediaType)
        {
            var contentType = new ContentType { MediaType = mediaType, Name = name };

            attachments.Add(new Attachment(stream, contentType));
        }

        public void AddLinkedResource(string contentId, string fileName, string mediaType = "image/png")
        {
            if (Body.Contains(string.Format("cid:{0}", contentId)) && File.Exists(fileName))
            {
                var htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

                var imagelink = new LinkedResource(fileName, mediaType)
                {
                    ContentId = contentId,
                    TransferEncoding = TransferEncoding.Base64
                };

                htmlView.LinkedResources.Add(imagelink);

                alternateViews.Add(htmlView);
            }
        }

        public void Dispose()
        {
        }
    }
}
