using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using LoggerService;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace WeAlignSAP
{
    public static class ProcessReport
    {
        private static byte[] pdfFile;
        public static string CoachEmail { get; set; }
        public static string CoachName { get; set; }
        public static string ClientName { get; set; }

        public static ILoggerManager Logger { get; set; }

        public static string APIKey { get; set; }

        private static async System.Threading.Tasks.Task<byte[]> ReadPDFAsync(string strReport)
        //private static void ReadPDFAsync(string strReport)
        {

            //strReport = @"http://www.jotform.com/uploads/Brice_Long/203565142875156/4844773945752828592/all-34-501-0-60085059-20200731094406591000000-4zXcjj.pdf?apiKey=4644d9649eae226d3383091e43f0a53d";
            //Create an instance of HttpClient
           HttpClient httpClient = new HttpClient();
            //httpClient.
            //Byte[] contentBytes = await httpClient.GetByteArrayAsync(strReport + @"?apiKey=4644d9649eae226d3383091e43f0a53d");
            Byte[] contentBytes = await httpClient.GetByteArrayAsync(strReport);

            //File.WriteAllBytes(@"C:\Users\micha\Dropbox\New folder\blah.pdf", contentBytes);

            return contentBytes;



        }

    
        public static async System.Threading.Tasks.Task ProcessAsync(string filePath)
        {

            Uri outUri;            
            List<string> clientStrength;
            StrengthReportReader strengthReportReader;

            //pdfFile = ReadPDFAsync(filePath).Result;


            if (!filePath.StartsWith("http"))
                pdfFile = File.ReadAllBytes(filePath);
            else
                if (Uri.TryCreate(filePath, UriKind.Absolute, out outUri))
                pdfFile = ReadPDFAsync(filePath).Result;
            //ReadPDFAsync(filePath);
            else
                pdfFile = File.ReadAllBytes(filePath);

            

            strengthReportReader = new StrengthReportReader(pdfFile);
            clientStrength = strengthReportReader.ReadFile();

            StrengthReportWriter.CoachName = CoachName;
            StrengthReportWriter.ClientName = ClientName;
            StrengthReportWriter.CoachEmailAddress = CoachEmail;
            StrengthReportWriter.Logger = Logger;

            StrengthReportWriter.email_body("");

            StrengthReportWriter.GenerateReport(clientStrength);
            


        }

    }
}
