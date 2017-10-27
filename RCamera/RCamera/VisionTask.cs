using System;
using System.Text;
using Android.OS;
using System.IO;
using Com.Microsoft.Projectoxford.Vision.Contract;
using Com.Microsoft.Projectoxford.Vision;
using GoogleGson;
using Newtonsoft.Json;
using RCamera.Model;

/// <summary>
/// @author 강수지
/// </summary>
namespace RCamera
{
    public class VisionTask : AsyncTask<Stream, string, string>
    {
        private MainActivity mainActivity;
        private const string VisionKey = "d5b9984f7f4c4bfdbc59428834d08fde";
        public VisionServiceRestClient VisionServiceRestClient = new VisionServiceRestClient(VisionKey);

        public VisionTask(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
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
        protected override void OnPostExecute(string result)
        {            
            var analysisResult = JsonConvert.DeserializeObject<VisionModel>(result);           
            StringBuilder strBuilder = new StringBuilder();

            foreach (var caption in analysisResult.description.captions)
            {
                strBuilder.Append(caption.text);
            }

            mainActivity.tvDo.Text = strBuilder.ToString();
        }
    }
}