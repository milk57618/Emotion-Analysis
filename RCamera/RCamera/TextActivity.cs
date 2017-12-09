using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Gms.Vision;
using Android.Gms.Vision.Texts;
using Android.Util;
using Android.Graphics;
using Android.Runtime;
using Android;
using Android.Content.PM;
using static Android.Gms.Vision.Detector;
using System.Text;
using Android.Support.V4.App;
using Android.Speech.Tts;
using Android.Content;
using Android.Provider;
using System.IO;

namespace RCamera
{
    [Activity(Label = "TextActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class TextActivity : AppCompatActivity, TextToSpeech.IOnInitListener
    {
        private ImageView imageView;
        private TextView tvResult;
        private CameraSource cameraSource;
        public TextToSpeech _speaker;
        public Bitmap mBitmap;
        public MemoryStream inputStreamT;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.TextOutput);

            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            tvResult = FindViewById<TextView>(Resource.Id.tvValue);
            var btnBack = FindViewById<Button>(Resource.Id.btnBack);
            var btnReplay = FindViewById<Button>(Resource.Id.btnReplay);
            

            //카메라 켜는 기능
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 0);

            //음성 다시 피드백하는 기능
            btnReplay.Click += delegate
            {
                Speak(tvResult.Text);
            };

            //main 창으로 돌아가는 기능
            btnBack.Click += delegate
            {
                //textView 초기화하기
                tvResult.Text = "";
                StartActivity(typeof(MainActivity));
            };
        }

        /// <summary>
        /// Text OCR 기능
        /// </summary>
        /// <param name="bitmap"></param>
        public void TextOCRFunction(Bitmap bitmap)
        {
            TextRecognizer textRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
            if (!textRecognizer.IsOperational)
            {
                Log.Error("ERROR", "Detector dependencies are not yet available");
            }
            else
            {
                Frame frame = new Frame.Builder().SetBitmap(bitmap).Build();
                SparseArray items = textRecognizer.Detect(frame);
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < items.Size(); ++i)
                {
                    TextBlock item = (TextBlock)items.ValueAt(i);
                    strBuilder.Append(item.Value);
                    strBuilder.Append("\n  ");
                }
                if (strBuilder.ToString() != "")
                {
                    tvResult.Text = "  " + strBuilder.ToString();
                    Speak(tvResult.Text);
                }
                else
                {
                    tvResult.Text = "  인식할 수 없습니다.";
                    Speak(tvResult.Text);
                }                    
            }
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
                TextOCRFunction(mBitmap);
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
        /// speak 초기화
        /// </summary>
        /// <param name="operationResult"></param>
        public void OnInit(OperationResult operationResult)
        {
            if (operationResult.Equals(OperationResult.Success))
            {
                _speaker.Speak(tvResult.Text, QueueMode.Flush, null, null);
            }
        }

        public void Release()
        {

        }
    }
}