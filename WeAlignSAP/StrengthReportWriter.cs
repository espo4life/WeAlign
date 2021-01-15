using iText.Kernel.Events;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Kernel.Font;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Borders;
using iText.Kernel.Geom;
using System.Net.Mail;
using System.Net;
using LoggerService;

namespace WeAlignSAP
{
    public static class StrengthReportWriter
    {
        public static string ClientName { get; set; }
        public static string CoachName { get; set; }
        public static string CoachEmailAddress { get; set; }

        public static ILoggerManager Logger { get; set; }

        public static string GetImage(string imgpath)
        {


            using (System.Drawing.Image img = System.Drawing.Image.FromFile(imgpath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    byte[] imgBytes = ms.ToArray();
                    return Convert.ToBase64String(imgBytes);
                }
            }
        }

        public static string email_body(string _reportpath)
        {

        //    string img = Directory.GetCurrentDirectory() + @"\\Resources\WeAlignLogoSmall.png";
        //    string image_yay = GetImage(img);


            StringBuilder TableStart = new StringBuilder();
            TableStart.AppendLine(@"<table width=100% style=""font: 13px/1.2 verdana;BORDER-COLLAPSE: collapse;"">");
            TableStart.AppendLine(@"<tr><td width=""100%"" style=""width:100%;text-align:center;"">");
            TableStart.AppendLine(@"<table width=100% style=""max-width:600px;font: 13px/1.2 verdana;BORDER-COLLAPSE: collapse;"">");
            TableStart.AppendLine(@"<tr><td style=""padding:10px;""><img src=""https://wealigncoaching.com/wp-content/uploads/2021/01/WeAlignLogoSmall.png"" style=""height:50px;"" /></td>");
            TableStart.Append(@"<td style=""background-color: #18803c;color:#fff;font-size:20px;text-align:center;font-weight:bold;padding:10px;"">WeAlign Strengths Profile Request</td></tr>");
            TableStart.AppendLine(@"<tr><td width=25% style=""background-color:#f2f2f2;padding:10px;border-bottom:1px solid #999;text-align:right;font-weight:bold;"">Coach Name:</td><td width=75% style=""text-align:left;padding:10px;border-bottom:1px solid #999;"">" + CoachName + "</td></tr>");
            TableStart.AppendLine(@"<tr><td width=25% style=""background-color:#f2f2f2;padding:10px;border-bottom:1px solid #999;text-align:right;font-weight:bold;"">Coach Email:</td><td width=75% style=""text-align:left;padding:10px;border-bottom:1px solid #999;"">" + CoachEmailAddress + "</td></tr>");
            TableStart.AppendLine(@"<tr><td width=25% style=""background-color:#f2f2f2;padding:10px;border-bottom:1px solid #999;text-align:right;font-weight:bold;"">Client Name:</td><td width=75% style=""text-align:left;padding:10px;border-bottom:1px solid #999;"">" + ClientName + "</td></tr>");
            TableStart.AppendLine(@"<tr><td width=25% style=""background-color:#f2f2f2;padding:10px;border-bottom:1px solid #999;text-align:right;font-weight:bold;"">Report:</td><td width=75% style=""text-align:left;padding:10px;border-bottom:1px solid #999;"">" + _reportpath + "</td></tr>");
            //TableStart.AppendLine(@"<tr><td style="" padding:3px;padding-left:10px;border-bottom:1px solid #999;"">ESPO 2</td><td style="" padding:3px;padding-left:10px;border-bottom:1px solid #999;"">ESPO 4</td></tr>");
            TableStart.AppendLine(@"</table>");
            TableStart.AppendLine(@"</td></tr>");
            TableStart.AppendLine(@"</table>");

            Logger.LogInfo(TableStart.ToString());

            return TableStart.ToString();
        }

        //public static void email_send(System.IO.MemoryStream attchment)
        //{


        //    StringBuilder stringBuilder = new StringBuilder();

        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient("mail.bluefinsol.net");
        //    SmtpServer.Credentials = new System.Net.NetworkCredential("no-reply@bluefinsol.net", "B5F4S2");
        //    mail.From = new MailAddress("no-reply@bluefinsol.net");
        //    mail.To.Add("michael.esposito@gmail.com");
        //    mail.Subject = ClientName + " Strengths Alignment Profile";

        //    stringBuilder.AppendLine("Coach Name:");
        //    stringBuilder.AppendLine("Coach Email:");
        //    stringBuilder.AppendLine("Client Name:");
        //    stringBuilder.AppendLine("Gallup Full 34 Report:");
        //    stringBuilder.AppendLine("If scheduled, when is the first session scheduled for?:");

        //    mail.Body = "";


        //    string AttachmentName = ClientName + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf";


        //    byte[] byteArr = attchment.ToArray();
        //    System.IO.MemoryStream stream1 = new System.IO.MemoryStream(byteArr, true);
        //    stream1.Write(byteArr, 0, byteArr.Length);
        //    stream1.Position = 0;
        //    mail.Attachments.Add(new Attachment(stream1, AttachmentName));

        //    //FileStream file = new FileStream(@"C:\Users\micha\Dropbox\New folder\file.pdf", FileMode.Create, FileAccess.Write);
        //    //attchment.WriteTo(file);
        //    //file.Close();



        //    SmtpServer.Send(mail);

        //}

        public static void email_send(string _attachment, string link)
        {

            string blah = @"<a href=""{0}"">{1}</a>";
            blah = string.Format(blah, link, link);

            //link
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("mail.bluefinsol.net");
            SmtpServer.Credentials = new System.Net.NetworkCredential("no-reply@bluefinsol.net", "B5F4S2");
            mail.From = new MailAddress("no-reply@bluefinsol.net");
            mail.To.Add(CoachEmailAddress);
            //mail.CC.Add(ClientEmailAddress);
            mail.Bcc.Add("michael.esposito@gmail.com");

            mail.Subject = ClientName + " Strengths Alignment Profile";
            mail.IsBodyHtml = true;
            mail.Body = email_body(blah);

            Logger.LogInfo(email_body(blah));

            mail.Attachments.Add(new Attachment(_attachment));

            SmtpServer.Send(mail);


        }

        public static string SavePath
        {
            get
            {
                string target = Directory.GetCurrentDirectory() + @"\Resources\" + CoachName.Replace(' ', '-');

                if (!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                }
                return target;
            }
        }



        public static string HtmlPath
        {
            get
            {
                string target = @"http://devapi.wealigncoaching.com\api\WeAlign\Download\" + CoachName.Replace(' ', '-');
                return target;
            }
        }

        private static string Template { get {
                return Directory.GetCurrentDirectory() + @"\Resources\SAP-BLANK.pdf";
            } }


        public static void GenerateReport(List<string> strengths)
        {

            iText.Layout.Element.Paragraph paragraph;
            List<StrengthDomain> strengthDomains = ThemeStrengths.GetStrengthDomains();
            Table table = new Table(2).SetWidth(1f);
            table.SetBorder(Border.NO_BORDER);

            string path = ClientName.Replace(' ', '-') + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf";
            string dest = SavePath + @"\" + path;
            string html = HtmlPath + @"\" + path;

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(Template), new PdfWriter(dest));
            Document document = new Document(pdfDoc);
            iText.Kernel.Geom.Rectangle pageSize;
            PdfFont _helvetica = PdfFontFactory.CreateRegisteredFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
            PdfFont _helveticab = PdfFontFactory.CreateRegisteredFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            int n = 1;
            iText.Layout.Element.Cell cell;
            iText.Layout.Element.Cell cell2;
            iText.Layout.Element.Text order;
            iText.Layout.Element.Text sText;

            PdfPage page = pdfDoc.GetPage(1);
            pageSize = page.GetPageSize();

            PdfCanvas pdfCanvas = new PdfCanvas(page);

            float fontSize = 12f;
            float left = 66f;
            float bottom = (pageSize.GetBottom() + 56);
            table.SetFixedPosition(left, bottom, pageSize.GetWidth()).SetMargin(0);//.SetPadding(0f);


            foreach (string st in strengths)
            {
                StrengthDomain blah = strengthDomains.Find(c => c.Strength.ToLower() == st.ToLower());

                cell = new iText.Layout.Element.Cell(1, 1);
                cell2 = new iText.Layout.Element.Cell(1, 1);

                cell.SetBorder(Border.NO_BORDER);
                cell.SetWidth(20f).SetPaddingRight(10f).SetPaddingTop(0.25f).SetPaddingBottom(0.25f);
                cell2.SetBorder(Border.NO_BORDER);
                cell2.SetWidth(120f).SetPaddingRight(10f).SetPaddingTop(0.25f).SetPaddingBottom(0.25f);

                order = new Text(n.ToString())
                        .SetFont(_helvetica)
                        .SetFontColor(new DeviceRgb(0, 0, 0))
                        .SetBold()
                        .SetFontSize(fontSize);



                paragraph = new iText.Layout.Element.Paragraph(order).SetTextAlignment(TextAlignment.RIGHT);

                cell.Add(paragraph);

                table.AddCell(cell).SetWidth(1f);

                sText = new Text(st)
                       .SetFont(_helvetica)
                       .SetFontColor(new DeviceRgb(blah.RGBColor.R, blah.RGBColor.G, blah.RGBColor.B))
                       .SetBold()
                       .SetFontSize(fontSize);

                paragraph = new Paragraph(sText);
                cell2.Add(paragraph);
                table.AddCell(cell2).SetWidth(1f);
                table.StartNewRow();
                n++;
            }

            int x2 = ((int)(pageSize.GetWidth()) / 2);
            int y2 = ((int)(pageSize.GetHeight()) - 100);


            sText = new Text(ClientName)
                       .SetFont(_helveticab)
                       .SetFontColor(new DeviceRgb(255, 255, 255))
                       .SetFontSize(20);

            Paragraph paragraph2 = new Paragraph(sText);
            new Canvas(pdfCanvas, pageSize)
                .ShowTextAligned(paragraph2, x2, y2, TextAlignment.CENTER)
                .Add(table);

            pdfDoc.Close();
            //email_body(dest);
            email_send(dest, html);

        }      
    }
}

