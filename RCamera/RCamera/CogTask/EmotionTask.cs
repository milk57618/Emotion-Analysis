using System.Collections.Generic;
using Android.OS;
using Newtonsoft.Json;
using RCamera.Model;
using Com.Microsoft.Projectoxford.Emotion;
using System.IO;
using RCamera.Helper;
using Android.App;

/// <summary>
/// @author 강수지
/// </summary>
namespace RCamera.CogTask
{
    public class EmotionTask : AsyncTask<Stream, string, string>
    {
        public EmotionServiceRestClient emotionRestClient;
        private const string EmotionKey = "f8cb6813da324f22a239d928677f5e47";
        private CognitiveActivity cognitiveActivity;
        private ProgressDialog pd = new ProgressDialog(Application.Context);

        public EmotionTask(CognitiveActivity cognitiveActivity)
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
        /// Result of Emotion
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        protected override string RunInBackground(params Stream[] @params)
        {
            try
            {
                PublishProgress("감정 분석 중입니다...");
                emotionRestClient = new EmotionServiceRestClient(EmotionKey);
                var result = emotionRestClient.RecognizeImage(@params[0]);
                var list = new List<EmotionModel>();

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        EmotionModel eM = new EmotionModel();
                        
                        //감정분석 API
                        Com.Microsoft.Projectoxford.Emotion.Contract.FaceRectangle faceRect = item.FaceRectangle;

                        eM.faceRectangle = new EFaceRectangle();
                        eM.faceRectangle.left = faceRect.Left;
                        eM.faceRectangle.top = faceRect.Top;
                        eM.faceRectangle.width = faceRect.Width;
                        eM.faceRectangle.height = faceRect.Height;

                        Com.Microsoft.Projectoxford.Emotion.Contract.Scores sc = item.Scores;
                        eM.scores = new EScores();
                        eM.scores.anger = sc.Anger;
                        eM.scores.happiness = sc.Happiness;
                        eM.scores.contempt = sc.Contempt;
                        eM.scores.fear = sc.Fear;
                        eM.scores.surprise = sc.Surprise;
                        eM.scores.neutral = sc.Neutral;
                        eM.scores.sadness = sc.Sadness;
                        eM.scores.disgust = sc.Disgust;
                        list.Add(eM);
                    }
                    return JsonConvert.SerializeObject(list);
                }

                return null;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Show the best of Emotion
        /// </summary>
        /// <param name="result"></param>
        protected override void OnPostExecute(string result)
        {
            pd.Dismiss();
            if (result!=null)
            {
                var list = JsonConvert.DeserializeObject<List<EmotionModel>>(result);
                EmotionModel EMax = new EmotionModel();
                if (list.Count>0)
                {
                    EMax = list[0];
                    foreach (var face in list)
                    {
                        if (EMax.faceRectangle.height * EMax.faceRectangle.width <= face.faceRectangle.height * face.faceRectangle.width)
                        {
                            EMax = face; //제일 큰 얼굴값을 택함
                        }
                    }
                    string tmp = EmotionFunction.GetEmo(EMax);
                    cognitiveActivity.textValue += tmp;
                    cognitiveActivity.tvText.Text = cognitiveActivity.textValue;
                    cognitiveActivity.Speak(cognitiveActivity.tvText.Text);                    
                }
                
            }
            
        }
    }
}