using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Graphics;
using System.IO;
using Com.Microsoft.Projectoxford.Face;
using GoogleGson;
using Java.Lang;
using Newtonsoft.Json;
using System.Collections.Generic;
using cognitiveTestApp.Model;
using System;

namespace cognitiveTestApp
{
    [Activity(Label = "cognitiveTestApp", MainLauncher = true, Theme ="@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        public ImageView imageView;
        public Bitmap mBitmap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            var btnProcess = FindViewById<Button>(Resource.Id.btnProcess);
            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            mBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.jin);
            imageView.SetImageBitmap(mBitmap);

            btnProcess.Click += delegate
            {
                byte[] bitmapData;
                using(var stream=new MemoryStream())
                {
                    mBitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                    bitmapData = stream.ToArray();
                }
                var inputStream = new MemoryStream(bitmapData);
                new DetectTask(this).Execute(inputStream);
            };
        }

        class DetectTask:AsyncTask<Stream, string, string>
        {
            private MainActivity mainActivity;
            private ProgressDialog pd = new ProgressDialog(Application.Context);

            public DetectTask(MainActivity mainActivity)
            {
                this.mainActivity = mainActivity;
            }

            protected override string RunInBackground(params Stream[] @params)
            {
                PublishProgress("Detecting...");
                var faceServiceClient = new FaceServiceRestClient("cf34900a1b6549e1882d0d9bd83dc795");
                Com.Microsoft.Projectoxford.Face.Contract.Face[] result = faceServiceClient.Detect(@params[0], 
                    true, //return FaceId
                    false, //return Face Landmarks
                    null); //return Face Attributes : age, gender..etc


                if (result == null)
                {
                    PublishProgress("Detection Finished. Nothing detected");
                    return null;
                }
                PublishProgress($"Detection Finished. {result.Length} faces(s) detected");

                Gson gson = new Gson();
                var strResult = gson.ToJson(result);

                return strResult;
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

            protected override void OnPostExecute(string result)
            {
                var faces = JsonConvert.DeserializeObject<List<FaceModel>>(result);
                var bitmap = DrawRectanglesOnBitmap(mainActivity.mBitmap, faces);

                mainActivity.imageView.SetImageBitmap(bitmap);
                pd.Dismiss();
            }

            private Bitmap DrawRectanglesOnBitmap(Bitmap mBitmap, List<FaceModel> faces)
            {
                Bitmap bitmap = mBitmap.Copy(Bitmap.Config.Argb8888, true);
                Canvas canvas = new Canvas(bitmap);
                Paint paint = new Paint();
                paint.AntiAlias = true;
                paint.SetStyle(Paint.Style.Stroke);
                paint.Color = Color.White;
                paint.StrokeWidth = 12;

                foreach(var face in faces)
                {
                    var faceRectangle = face.faceRectangle;
                    canvas.DrawRect(faceRectangle.left, faceRectangle.top, faceRectangle.left + faceRectangle.width, faceRectangle.top + faceRectangle.height,
                        paint);
                }
                return bitmap;
            }
        }
    }
}

