using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace WeAlignSAP
{

    public class StrengthDomain
    {
        public string Strength { get; set; }
        public string HexColor { get; set; }
        public Color RGBColor { get { return ThemeStrengths.HexToColor(HexColor); } }

        public StrengthDomain(string strength, string hexColor)
        {
            this.Strength = strength;
            this.HexColor = hexColor;
        }
    }


    public static class ThemeStrengths
    {

        internal static Color HexToColor(string hexString)
        {
            //replace # occurences
            if (hexString.IndexOf('#') != -1)
                hexString = hexString.Replace("#", "");

            int r, g, b = 0;

            r = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            g = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            b = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

            return Color.FromArgb(r, g, b);
        }
               

        private static JObject GetTalentThemeValues()
        {
            string baseUrl = "https://api.quickbase.com/v1/";
            string hoseName = "builderprogram-blong.quickbase.com";
            string userAgent = "SAP";
            string authToken = "QB-USER-TOKEN b49cd9_njtc_bj3m89pd8wsbfhdbfypgubwa624x";
            string appId = "bq2ru72ub";

            var client = new RestClient(baseUrl + "/reports/6/run?tableId=bq2ru73fe&skip=0&top=100");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("QB-Realm-Hostname", hoseName);
            client.UserAgent = userAgent;
            request.AddHeader("Authorization", authToken);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return JObject.Parse(response.Content);
        }

        public static List<StrengthDomain> GetStrengthDomains()
        {
            List<StrengthDomain> strengths = new List<StrengthDomain>();
            JObject jobj = GetTalentThemeValues();

            foreach (JObject blahhh in jobj["data"].Children())
            {
                strengths.Add(new StrengthDomain(blahhh["6"]["value"].ToString(), blahhh["9"]["value"].ToString()));
            }

            return strengths;

        }

        
    }
}
