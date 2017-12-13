using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace WIM.Core.Common.Utility.Mail
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return configSendGridasync(message);
        }

        private Task configSendGridasync(IdentityMessage message)
        {
            #region formatter
            string html = "Please confirm your account by clicking this link: <a href=\"" + message.Body + "\">link</a><br/>";

            html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser: " + message.Body);
            #endregion

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.From = new System.Net.Mail.MailAddress("siwarakk@yamatothai.com", "Siwarak Kongsra");
            msg.To.Add(new System.Net.Mail.MailAddress("siwarakk@yamatothai.com"));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(System.Net.Mail.AlternateView.CreateAlternateViewFromString(html, null, System.Net.Mime.MediaTypeNames.Text.Html));

            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("mail1.yamatothai.com", Convert.ToInt32(26));
            NetworkCredential credentials = new NetworkCredential("siwarakk@yamatothai.com", "ytcsiwarak1617");
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = false;
            return smtpClient.SendMailAsync(msg);
        }
    }
}