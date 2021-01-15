using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeAlignSAP;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;

namespace WeAlignAPI.Controllers
{

    public class StudentModel
    {
        public string formID { get; set; }
        public string submissionID { get; set; }
        public string rawRequest { get; set; }
    }


    [ApiController]
    [Route("api/[controller]/{action}")]
    public class WeAlignController : ControllerBase
    {
       
        private readonly ILoggerManager _logger;
        private readonly IConfiguration _configuration;

        public WeAlignController(IConfiguration configuration, ILoggerManager logger)
        {

            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Get Not Implemented";
        }

        [HttpPost]
        public async Task SingleSAP([FromForm]StudentModel std)
        {

            try
            {
                string webHookInfo = std.rawRequest;
                _logger.LogInfo(std.rawRequest);


                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(webHookInfo);

                string url = @"{0}?apiKey={1}";

                               
                string APIKey = _configuration["APIKey"];
                string filepath = myDeserializedClass.gallupFull[0];


                _logger.LogInfo(filepath);
                _logger.LogInfo(APIKey);

                url = String.Format(url,   filepath, APIKey);
                _logger.LogInfo(url);

                ProcessReport.CoachName = myDeserializedClass.q3_coachName;
                ProcessReport.CoachEmail = myDeserializedClass.q4_coachEmail;
                ProcessReport.ClientName = myDeserializedClass.q5_clientName5;
                ProcessReport.Logger = _logger;

                await ProcessReport.ProcessAsync(url);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.ToString());
            }

        }

        [HttpGet("{coach}/{file}")]
        public FileStreamResult Download(string coach, string file)
        {

            try
            {
                _logger.LogInfo(coach);
                string filePath = Directory.GetCurrentDirectory() + @"\Resources\" + coach;
                IFileProvider provider = new PhysicalFileProvider(filePath);
                IFileInfo fileInfo = provider.GetFileInfo(file);
                var readStream = fileInfo.CreateReadStream();
                var mimeType = "application/pdf";

                _logger.LogInfo(filePath);


                return new FileStreamResult(readStream, mimeType)
                {
                    FileDownloadName = file
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
           
        }

    }
}

