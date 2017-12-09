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
using Newtonsoft.Json.Linq;
using System.Xml;
using Android.App;

/// <summary>
/// @author 강수지
/// </summary>
namespace RCamera.CogTask
{
    public class VisionTask : AsyncTask<Stream, string, string>
    {
        private const string VisionKey = "4cb1120fba1946e3abf46ddf2916d234";
        public VisionServiceRestClient VisionServiceRestClient = new VisionServiceRestClient(VisionKey);
        private CognitiveActivity cognitiveActivity;
        private ProgressDialog pd = new ProgressDialog(Application.Context);

        public VisionTask(CognitiveActivity cognitiveActivity)
        {
            this.cognitiveActivity = cognitiveActivity;
        }

        protected override void OnPreExecute()
        {
            pd.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
            pd.Show();
        }

        protected override void OnProgressUpdate(params string[] values)
        {
            pd.SetMessage(values[0]);
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
                PublishProgress("상황 인식 중..");
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
            if (result != null)
            {
                var analysisResult = JsonConvert.DeserializeObject<VisionModel>(result);
                if (analysisResult != null)
                {
                    string strValue = "";

                    foreach (var caption in analysisResult.description.captions)
                    {
                        strValue += caption.text;
                    }
                    
                    //영문값을 한국어로 번역하는 기능
                    VisionTranslateAsync(strValue);                    
                }
                else
                {
                    cognitiveActivity.tvText.Text = "번역할 수 없습니다.";
                    cognitiveActivity.Speak(cognitiveActivity.tvText.Text);
                    pd.Dismiss();
                }
            }
            else
            {
                cognitiveActivity.tvText.Text = "번역할 수 없습니다.";
                cognitiveActivity.Speak(cognitiveActivity.tvText.Text);
                pd.Dismiss();
            }
        }

        /// <summary>
        /// 영문 값 번역하는 기능
        /// </summary>
        /// <param name="str"></param>
        public async void VisionTranslateAsync(string str)
        {
            PublishProgress("번역 중..");

            //Azure function 으로 string 값을 보냄
            Web web = new Web();
            HttpResponseMessage respon = web.visionString(str);
            
            if (respon != null)
            {
                string rs = await respon.Content.ReadAsStringAsync();                
                if (rs.Contains("<html>"))
                {
                    cognitiveActivity.tvText.Text = "  " + str;
                }
                else
                {
                    //string값 잘라내기
                    int i1 = rs.IndexOf(">");
                    int i2 = rs.LastIndexOf("<");
                    rs = rs.Substring(i1 + 1, i2 - i1 - 1);
                    cognitiveActivity.tvText.Text = "  " + rs;
                }                
            }
            else
            {
                cognitiveActivity.tvText.Text = "  " + str;
            }           
            pd.Dismiss();
            //Text 값 음성 피드백
            cognitiveActivity.Speak(cognitiveActivity.tvText.Text);            
        }

    }
}