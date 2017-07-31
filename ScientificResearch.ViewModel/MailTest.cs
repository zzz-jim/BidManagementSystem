using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ScientificResearch.ViewModel
{
    public class MailTest
    {
        //private static void Main(string[] args)
        //{
        //    //邮件发送对象  
        //    var mailclass = new MailClass();
        //    //邮件SMTP服务器地址  
        //    mailclass.MailServer = "smtp.163.com";
        //    //收件人邮件地址  
        //    mailclass.MailFrom = "qqww080808@163.com";
        //    //发件人账号用户  
        //    mailclass.MailUserName = "qqww080808@163.com";
        //    //发件人账号密码  
        //    mailclass.MailUserPassword = "qqww74886qqww";
        //    //邮件编码格式  
        //    mailclass.MailCharset = "utf-8";

        //    SendMailMethod(mailclass);
        //}

        /// <summary>
        ///     发送邮件信息
        /// </summary>
        /// <param name="mailclass"></param>
        public static void SendMailMethod(MailClass mailclass)
        {
            try
            {
                var mail = new MailMessage();
                //收件人  
                mail.From =
                    new MailAddress(mailclass.MailFrom, "汪汪", Encoding.GetEncoding(mailclass.MailCharset));
                //发送人  
                mail.Sender =
                    new MailAddress(mailclass.MailFrom, "厅厅", Encoding.GetEncoding(mailclass.MailCharset));
                mail.To.Add("2451854140@qq.com"); //发件人  
                mail.SubjectEncoding = Encoding.GetEncoding(mailclass.MailCharset);
                mail.Subject = "报表分析";
                mail.BodyEncoding = Encoding.GetEncoding(mailclass.MailCharset);
                mail.Priority = MailPriority.Normal;
                //是否为网页格式  
                mail.IsBodyHtml = true;

                var smtpMail = new SmtpClient(mailclass.MailServer);
                smtpMail.UseDefaultCredentials = true;
                smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpMail.EnableSsl = false;
                //smtp主机上的端口号,默认是25  
                smtpMail.Port = 25;
                //验证发件人身份  
                smtpMail.Credentials =
                    new NetworkCredential(mailclass.MailUserName, mailclass.MailUserPassword);
                //邮件的内容可以是一个html文本.  
                // 事务失败。 服务器响应为:DT:SPM 163 smtp8,DMCowAAnvn3WGGNZNMxxFg--.13365S2 1499666648,please see http://mail.163.com/help/help_spam_16.htm?ip=52.80.7.202&hostid=smtp8&time=1499666648
                // 广告邮件 会报错。。。。
                var filePath = AppDomain.CurrentDomain.BaseDirectory + "../../淘宝网_百度百科.html";
                var read =
                    new StreamReader(filePath, Encoding.GetEncoding("GB2312"));
                var mailBody = read.ReadToEnd();//  "汪汪测试邮件";// read.ReadToEnd();
                //邮件内容  
                mail.Body = mailBody;

                // 附件：在 .net 4.0中，附件名称含有特殊名称时，接收方将收到名字为空的附件
                mail.Attachments.Add(new Attachment(@"D:\WorkForJim\Account.txt"));

                smtpMail.Send(mail);
                //释放附件对象，否则文件无法删除  
                foreach (var item in mail.Attachments)
                    item.Dispose();
                smtpMail.Dispose();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}