using System.Collections.Generic;
using Android.OS;
using Newtonsoft.Json;
using RCamera.Model;
using Com.Microsoft.Projectoxford.Emotion;
using System.IO;
using RCamera.Helper;

/// <summary>
/// @author 강수지
/// </summary>
namespace RCamera.CogTask
{
    public class EmotionTask : AsyncTask<Stream, string, string>
    {
        public EmotionServiceRestClient emotionRestClient;
        private const string EmotionKey = "c0aa6b40408142d0975d2aec77914c61";
        private CognitiveActivity cognitiveActivity;

        
        public EmotionTask(CognitiveActivity cognitiveActivity)
        {
            this.cognitiveActivity = cognitiveActivity;
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
            var list = JsonConvert.DeserializeObject<List<EmotionModel>>(result);
            EmotionModel EMax = new EmotionModel();

            EMax = list[0];
            foreach (var face in list)
            {
                if (EMax.faceRectangle.height * EMax.faceRectangle.width <= face.faceRectangle.height * face.faceRectangle.width)
                {
                    EMax = face; //제일 큰 얼굴값을 택함
                }
            }
            
            cognitiveActivity.tvEmotion.Text = EmotionFunction.GetEmo(EMax);
        }
    }
}