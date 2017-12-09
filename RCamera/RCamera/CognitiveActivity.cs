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
using Android.Support.V7.App;
using Android.Speech.Tts;

namespace RCamera
{
    [Activity(Label = "CognitiveActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class CognitiveActivity : AppCompatActivity, TextToSpeech.IOnInitListener
    {
        public ImageView imageView;
        public Bitmap mBitmap;
        public TextView tvText;
        public MainActivity mainActivity;
        public MemoryStream inputStream1;
        public MemoryStream inputStream2;
        public MemoryStream inputStream3;
        public String textValue;
        private TextToSpeech _speaker;

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

            //카메라 켜는 기능
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);           

            //음성 다시 피드백
            btnReplay.Click += delegate
            {
                Speak(tvText.Text);          
            };

            //main화면으로 돌아가기
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

                new FaceTask(this).Execute(inputStream2);  //Face Task 시작점   
            }
           
        }
        /// <summary>
        /// 텍스트 값 음성으로 내보내기
        /// </summary>
        /// <param name="text"></param>
        public void Speak(string text)
        {
            if (_speaker == null)
            {
                _speaker = new TextToSpeech(Application.Context, this);
            }
            else
            {
                _speaker.Speak(text, QueueMode.Add, null, null);
            }
        }

        /// <summary>
        /// speak 기능 초기화
        /// </summary>
        /// <param name="operationResult"></param>
        public void OnInit(OperationResult operationResult)
        {
            if (operationResult.Equals(OperationResult.Success))
            {
                _speaker.Speak(tvText.Text, QueueMode.Flush, null, null);
            }
        }
    }
}