using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace DreamCMS.FuncHelp
{
    public class DHelp
    {
        public static string SEOurl(string url)
        {
            // ensure the the is url lowercase
            string encodedUrl = (url ?? "").ToLower();

            // replace & with and
            encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

            // remove characters
            encodedUrl = encodedUrl.Replace("'", "");

            // a, e, u, i, o
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string strFormD = encodedUrl.Normalize(System.Text.NormalizationForm.FormD);
            encodedUrl = regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            // remove invalid characters
            encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

            // remove duplicates
            encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

            // trim leading & trailing characters
            encodedUrl = encodedUrl.Trim('-');

            return encodedUrl;
        }

        public class Email
        {

            public static bool SendMail(string AddressTo, string Title, string Content)
            {
                string idmail = "baobitoanquoc.services@gmail.com",
                    pwmail = "wjjmzswjohjvdwov"; 

                string _GmailID = idmail.Trim().Replace("@gmail.com", "");
                string _GmailPW = pwmail;

                System.Net.Mail.MailMessage MyMailMessage = new System.Net.Mail.MailMessage();

                MyMailMessage.From = new MailAddress(_GmailID + "@gmail.com");
                MyMailMessage.To.Add(AddressTo);
                MyMailMessage.Subject = Title;
                MyMailMessage.Body = Content;
                MyMailMessage.IsBodyHtml = true;
                SmtpClient SMTPServer = new SmtpClient("smtp.gmail.com");
                SMTPServer.Port = 587;
                SMTPServer.Credentials = new System.Net.NetworkCredential(_GmailID, _GmailPW);
                SMTPServer.EnableSsl = true;
                try
                {
                    SMTPServer.Send(MyMailMessage);
                    return true;

                }
                catch (Exception ex)
                {
                }
                return false;
            }

            public static string TemplateMail(string title, string title2,string idstr, string nguoigui, string diachi, string emailstr, string dienthoai, string noidung)
            {
                string s = "<!DOCTYPE html><html><head><meta charset='utf-8'><title>" + title + "</title></head><body ><table width='100%' border='0' cellspacing='0' cellpadding='0' ><tr><td><table width='600' border='0' cellspacing='0' cellpadding='0' bgcolor='#FFFFFF' align='center'><tr><td><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td width='144'><a href='http://baobitoanquoc.com' target='_blank' style='margin-left:20px;padding:15px;display:block'><img src='http://baobitoanquoc.net/Upload/images/web/logo%20bao%20bi%20carton%201.png' width='100' border='0' alt=''></a></td><td width='300'></td><td align='right' style='padding-right:15px'><font style='font-family:Verdana,Geneva,sans-serif;color:#666766;font-size:13px;line-height:21px'>#"+ idstr +"<br>"+DateTime.Now.ToString()+" </font></td></tr></table></td></tr><tr><td align='center'>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td><table width='100%' border='0' cellspacing='0' cellpadding='0'><tr><td width='5%'>&nbsp;</td><td width='90%' align='left' valign='top'><font style='font-family:Georgia,Times New Roman,Times,serif;color:#010101;font-size:24px'><strong><em>" + title2 + "</em></strong></font> <br> <br> <font style='font-family:Verdana,Geneva,sans-serif;color:#666766;font-size:13px;line-height:21px'><p><b>Người gửi:</b> " + nguoigui + "</p><p><b>Địa chỉ:</b> " + diachi + "</p><p><b>Email:</b> " + emailstr + "</p><p><b>Điện thoại:</b> " + dienthoai + "</p><p><b>Nội dung:</b> <br><pre>" + noidung + "</pre></p></font></td><td width='5%'>&nbsp;</td></tr></table></td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr></table></td></tr></table></body></html>";

                return s;
            }

        }

    }
}
