using System;
using System.Collections.Generic;
using System.Text;

namespace WeAlignSAP
{
    //// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    //public class Q7IfScheduled
    //{
    //    public string month { get; set; }
    //    public string day { get; set; }
    //    public string year { get; set; }
    //}

    //public class TempUpload
    //{
    //    public List<string> q6_gallupFull { get; set; }
    //}

    //public class FormData
    //{
    //    public string slug { get; set; }
    //    public string q3_coachName { get; set; }
    //    public string q4_coachEmail { get; set; }
    //    public string q5_clientName5 { get; set; }
    //    public Q7IfScheduled q7_ifScheduled { get; set; }
    //    public string q11_notes11 { get; set; }
    //    public string q9_exampleStrengths { get; set; }
    //    public string q10_typeA { get; set; }
    //    public string preview { get; set; }
    //    public TempUpload temp_upload { get; set; }
    //    public string file_server { get; set; }
    //    public List<string> gallupFull { get; set; }
    //}

    //public class Root
    //{
    //    public string formID { get; set; }
    //    public string submissionID { get; set; }
    //    public string webhookURL { get; set; }
    //    public string ip { get; set; }
    //    public string formTitle { get; set; }
    //    public string pretty { get; set; }
    //    public string username { get; set; }
    //    public string rawRequest { get; set; }
    //    public string type { get; set; }
    //    public FormData formData { get; set; }
    //}

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Q7IfScheduled
    {
        public string month { get; set; }
        public string day { get; set; }
        public string year { get; set; }
    }

    public class TempUpload
    {
        public List<string> q6_gallupFull { get; set; }
    }

    public class Root
    {
        public string slug { get; set; }
        public string q3_coachName { get; set; }
        public string q4_coachEmail { get; set; }
        public string q5_clientName5 { get; set; }
        public Q7IfScheduled q7_ifScheduled { get; set; }
        public string q11_notes11 { get; set; }
        public string q9_exampleStrengths { get; set; }
        public string q10_typeA { get; set; }
        public string event_id { get; set; }
        public TempUpload temp_upload { get; set; }
        public string file_server { get; set; }
        public List<string> gallupFull { get; set; }
    }



}
