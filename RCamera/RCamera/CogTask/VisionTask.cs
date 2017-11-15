using System;
using System.Text;
using Android.OS;
using System.IO;
using Com.Microsoft.Projectoxford.Vision.Contract;
using Com.Microsoft.Projectoxford.Vision;
using GoogleGson;
using Newtonsoft.Json;
using RCamera.Model;
using System.Dynamic;
using RCamera.Helper;
using System.Net.Http;

/// <summary>
/// @author 강수지
/// </summary>
namespace RCamera.CogTask
{
    public class VisionTask : AsyncTask<Stream, string, string>
    {
        private const string VisionKey = "d5b9984f7f4c4bfdbc59428834d08fde";
        public VisionServiceRestClient VisionServiceRestClient = new VisionServiceRestClient(VisionKey);
        private CognitiveActivity cognitiveActivity;
       
        public VisionTask(CognitiveActivity cognitiveActivity)
        {
            this.cognitiveActivity = cognitiveActivity;
        }

        /// <summary>
        /// Vision Detection Function
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        protected override string RunInBackground(params Stream[] @params)
        {
            try
            {
                string[] features = { "Description" };
                string[] details = { };

                AnalysisResult result = VisionServiceRestClient.AnalyzeImage(@params[0], features, details);

                if (result != null)
                {
                    string strResult = new Gson().ToJson(result);
                    return strResult;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }         
        }

        /// <summary>
        /// Show vision Result
        /// </summary>
        /// <param name="result"></param>
        protected override void OnPostExecuteAsync(string result)
        {            
            var analysisResult = JsonConvert.DeserializeObject<VisionModel>(result);
            String strValue = "";

            foreach (var caption in analysisResult.description.captions)
            {
                strValue+=caption.text;
            }
            //영문값을 한국어로 번역하는 기능
            VisionTranslateAsync(strValue);
        }

        public async void VisionTranslateAsync(string str)
        {
            Web web = new Web();
            HttpResponseMessage respon = web.visionString(str);
            string rs = await respon.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<string>(rs);
            cognitiveActivity.tvDo.Text = value;
        }
    }
}