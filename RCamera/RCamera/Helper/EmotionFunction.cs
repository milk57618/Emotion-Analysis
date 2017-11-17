using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RCamera.Model;
using Com.Microsoft.Projectoxford.Emotion;
using System.IO;
using GoogleGson;
using Newtonsoft.Json;
using Microsoft.ProjectOxford.Emotion;
using Com.Microsoft.Projectoxford.Emotion.Contract;

/// <summary>
/// @author 강수지
/// </summary>
namespace RCamera.Helper
{
    public class EmotionFunction
    {
        
        /// <summary>
        /// max value of emotion
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetEmo(EmotionModel item)
        {
            List<double> list = new List<double>();
            EScores sc = item.scores;
            list.Add(sc.anger);
            list.Add(sc.happiness);
            list.Add(sc.contempt);
            list.Add(sc.neutral);
            list.Add(sc.disgust);
            list.Add(sc.sadness);
            list.Add(sc.surprise);
            list.Add(sc.fear);

            var listSorted = list.OrderBy(i => i).ToList();

            double maxElementInList = listSorted[listSorted.Count - 1];

            if (maxElementInList == sc.anger)
                return "화가 나있습니다.";
            else if (maxElementInList == sc.happiness)
                return "행복해 합니다.";
            else if (maxElementInList == sc.contempt)
                return "경멸스러워 합니다.";
            else if (maxElementInList == sc.sadness)
                return "슬퍼하고 있습니다.";
            else if (maxElementInList == sc.fear)
                return "무서워하고 있습니다.";
            else if (maxElementInList == sc.surprise)
                return "놀랐습니다.";
            else if (maxElementInList == sc.disgust)
                return "역겨워 미쳐합니다.";
            else
                return "아무런 반응 없이 있습니다.";
        }
        
        /// <summary>
        /// Emotion Result Function
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string EmotionResult(IList<RecognizeResult> result)
        {
            List<EmotionModel> EMI = new List<EmotionModel>();

            foreach (var item in result)
            {
                EmotionModel eM = new EmotionModel();

                Com.Microsoft.Projectoxford.Emotion.Contract.FaceRectangle faceRect = item.FaceRectangle;

                eM.faceRectangle = new EFaceRectangle();
                eM.faceRectangle.left = faceRect.Left;
                eM.faceRectangle.top = faceRect.Top;
                eM.faceRectangle.width = faceRect.Width;
                eM.faceRectangle.height = faceRect.Height;

                Scores sc = item.Scores;
                eM.scores = new EScores();
                eM.scores.anger = sc.Anger;
                eM.scores.happiness = sc.Happiness;
                eM.scores.contempt = sc.Contempt;
                eM.scores.fear = sc.Fear;
                eM.scores.surprise = sc.Surprise;
                eM.scores.neutral = sc.Neutral;
                eM.scores.sadness = sc.Sadness;
                eM.scores.disgust = sc.Disgust;
                EMI.Add(eM);
            }

            EmotionModel EMax = new EmotionModel();

            EMax = EMI[0];
            foreach (var face in EMI)
            {
                if (EMax.faceRectangle.height * EMax.faceRectangle.width <= face.faceRectangle.height * face.faceRectangle.width)
                {
                    EMax = face; //제일 큰 얼굴값을 택함
                }
            }

            return GetEmo(EMax);
        }
    }
}