using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using code_project.Models;
using log4net;
using Newtonsoft.Json;

namespace code_project.Services
{
    internal class APIService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<List<ProcessLog>> GetProcessLog(int factoryCode, int orderNumber, long rxarrangementNumber, int processCode)
        {
            log.Debug($"GetProcessLog factoryCode:{factoryCode}, orderNumber:{orderNumber}, rxarrangementNumber:{rxarrangementNumber}, processCode:{processCode}");
            try
            {
                string fullURI = $"{ConfigurationObject.Uri}/ProcessLog/details?factoryCode={factoryCode}&orderNumber={orderNumber}&rxarrangementNumber={rxarrangementNumber}&processCode={processCode}";
                //log.Debug(fullURI);

                Task<string> taskGetDataAsync = GetDataAsync(fullURI);
                string jsonProcessLog = await taskGetDataAsync;
                if (string.IsNullOrEmpty(jsonProcessLog))
                {
                    log.Warn("jsonProcessLog IsNullOrEmpty");
                    return null;
                }
                //log.Debug(jsonLensDetail);
                return JsonConvert.DeserializeObject<List<ProcessLog>>(jsonProcessLog);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        private async Task<string> GetDataAsync(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse httpWebResponse = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (Stream stream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader streamReader = new StreamReader(stream))
                        {
                            return await streamReader.ReadToEndAsync();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }
}
