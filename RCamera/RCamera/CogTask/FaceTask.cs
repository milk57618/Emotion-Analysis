using System.Collections.Generic;
using Android.App;
using Android.OS;
using RCamera.Model;
using System.IO;
using Com.Microsoft.Projectoxford.Face;
using GoogleGson;
using Newtonsoft.Json;
using RCamera.Helper;
using Android.Content;

/// <summary>
/// @author 강수지
/// </summary>
namespace RCamera.CogTask
{
    public class FaceTask : AsyncTask<Stream, string, string>
    {
        private const string FaceKey = "098184e62f0e47fcb4dcb8706d15240b";
       
        public FaceServiceRestClient faceServiceClient;
        private CognitiveActivity cognitiveActivity;

        public FaceTask(CognitiveActivity cognitiveActivity)
        {
            this.cognitiveActivity = cognitiveActivity;
        }

        /// <summary>
        /// Detecting Face with Cognitive API
        /// </summary>
        /// <param name="params"></param>
        /// <returns></returns>
        protected override string RunInBackground(params Stream[] @params)
        {
            try
            { 
                faceServiceClient = new FaceServiceRestClient(FaceKey);

                //Get Congnitive API result
                Com.Microsoft.Projectoxford.Face.Contract.Face[] result = faceServiceClient.Detect(@params[0],
                    true, //return FaceId
                    false, new FaceServiceClientFaceAttributeType[] { FaceServiceClientFaceAttributeType.Age, FaceServiceClientFaceAttributeType.Gender
                , FaceServiceClientFaceAttributeType.FacialHair
                , FaceServiceClientFaceAttributeType.Smile
                , FaceServiceClientFaceAttributeType.Glasses
                , FaceServiceClientFaceAttributeType.HeadPose});
              
                if (result != null)
                {
                    //parse json to String
                    Gson gson = new Gson();
                    return gson.ToJson(result);
                }    
                return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }
  
        /// <summary>
        /// Show the result of face
        /// </summary>
        /// <param name="result"></param>
        protected override void OnPostExecute(string result)
        {
            if (result != null)
            {
                var faces = JsonConvert.DeserializeObject<List<FaceModel>>(result);
                if (faces != null)
                {
                    var bitmap = FaceFunction.DrawRectanglesOnBitmap(cognitiveActivity.mBitmap, faces);
                    FaceFunction.setImageOutput(faces, cognitiveActivity);
                    cognitiveActivity.imageView.SetImageBitmap(bitmap);

                    new EmotionTask(cognitiveActivity).Execute(cognitiveActivity.inputStream1);  //Emotion Task 시작점                    
                }
                else
                {
                    new VisionTask(cognitiveActivity).Execute(cognitiveActivity.inputStream3);  //Vision Task 시작점
                }
               
            }
            else
            {
                new VisionTask(cognitiveActivity).Execute(cognitiveActivity.inputStream3);  //Vision Task 시작점
            }           
        }      
    }         
}