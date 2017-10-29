using System.Collections.Generic;
using Android.Graphics;
using RCamera.Model;

/// <summary>
/// @author 강수지
/// </summary>
namespace RCamera.Helper
{
    public class FaceFunction
    {
        /// <summary>
        /// Paint of the face
        /// </summary>
        /// <param name="mBitmap"></param>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static Bitmap DrawRectanglesOnBitmap(Bitmap mBitmap, List<FaceModel> faces)
        {
            Bitmap bitmap = mBitmap.Copy(Bitmap.Config.Argb8888, true);
            Canvas canvas = new Canvas(bitmap);
            Paint paint = new Paint();
            paint.AntiAlias = true;
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = Color.White;
            paint.StrokeWidth = 2;

            foreach (var face in faces)
            {
                var faceRectangle = face.faceRectangle;
                canvas.DrawRect(faceRectangle.left, faceRectangle.top, faceRectangle.left + faceRectangle.width, faceRectangle.top + faceRectangle.height,
                    paint);
            }
            return bitmap;
        }

        /// <summary>
        /// Function of the max face
        /// </summary>
        /// <param name="faces"></param>
        /// <param name="mainActivity"></param>
        public static void setImageOutput(List<FaceModel> faces, CognitiveActivity cognitiveActivity)
        {
            List<FaceModel> FM = new List<FaceModel>();

            foreach (var faceTemp in faces)
            {
                FM.Add(faceTemp);
            }

            FaceModel faceMax = new FaceModel();
            faceMax = FM[0];
            foreach (var face in FM)
            {
                if (faceMax.faceRectangle.height * faceMax.faceRectangle.width <= face.faceRectangle.height * face.faceRectangle.width)
                {
                    faceMax = face; //제일 큰 얼굴값을 택함
                }
            }

            string gender="";
            if (faceMax.faceAttributes.Gender == "female" || faceMax.faceAttributes.Gender == "Female" || faceMax.faceAttributes.Gender == "FEMALE")
            {
                gender = "여자";
            }
            else
            {
                gender = "남자";
            }
            int age =(int)faceMax.faceAttributes.Age;
            cognitiveActivity.tvGender.Text =gender;
            cognitiveActivity.tvAge.Text = age.ToString();
        }
    }
}