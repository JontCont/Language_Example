using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace tools
{
    class Mail
    {
        public Mail() { SendEmail(); }
        //設定smtp主機
        public string smtpAddress { get; set; } = "smtp.office365.com";
        //設定Port
        public int port { get; set; } = 587;
        public bool enableSSL { get; set; } = true;
        //填入寄送方email和密碼
        public string emailFrom { get; set; } = "noreply@pome.com.tw"; //= "cont.ma@pome.com.tw";
        public string password { get; set; } = "Pome123456"; //= "Sa45645600";

        //收信方email
        public string emailTo = "johncont99688@gmail.com";

        public void SendEmail()
        {
            //主旨
            string subject = "Hello !!! Project";
            //內容
            string body = "TEST,ithome";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                // 若你的內容是HTML格式，則為True
                mail.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient(smtpAddress, port))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    //smtp.Timeout = 5000;
                    smtp.Send(mail);
                }
            }
        }


    }
}
