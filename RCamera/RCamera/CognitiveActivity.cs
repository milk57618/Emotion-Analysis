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
using Android.Graphics;
using System.IO;
using Android.Provider;
using RCamera.CogTask;

namespace RCamera
{
    [Activity(Label = "CognitiveActivity")]
    public class CognitiveActivity : Activity
    {
        public ImageView imageView;
        public Bitmap mBitmap;
        public TextView tvText;
        public MainActivity mainActivity;
        public MemoryStream inputStream1;
        public MemoryStream inputStream2;
        public MemoryStream inputStream3;
        public String textValue;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ImageOutput);

            //각각의 UI value
            textValue = "";
            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            tvText = FindViewById<TextView>(Resource.Id.tvValue);
            var btnBack = FindViewById<Button>(Resource.Id.btnBack);
            var btnReplay = FindViewById<Button>(Resource.Id.btnReplay);

            //카메라 키는 기능
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);
            

            btnReplay.Click += delegate
            {
                if (mBitmap != null)
                {
                    new FaceTask(this).Execute(inputStream2);  //Face Task 시작점       
                    
                }                
            };

            btnBack.Click += delegate
            {
                //textView 초기화하기
                tvText.Text = "";
                StartActivity(typeof(MainActivity));
            };
        }

        /// <summary>
        /// Get Camera Bitmap and streaming
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="resultCode"></param>
        /// <param name="data"></param>
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            mBitmap = (Bitmap)data.Extras.Get("data");

            if (mBitmap != null)
            {
                imageView.SetImageBitmap(mBitmap);
                byte[] bitmapData;
                using (var stream = new MemoryStream())
                {
                    mBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                    bitmapData = stream.ToArray();
                }

                inputStream1 = new MemoryStream(bitmapData);  //Face cognitive에 쓰일 inputStream
                inputStream2 = new MemoryStream(bitmapData);  //Emotion cognitive에 쓰일 inputStream
                inputStream3 = new MemoryStream(bitmapData);  //Vision cognitive에 쓰일 inputStream
                
            }
            
        }
    }
}