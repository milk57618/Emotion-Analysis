using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Graphics;
using System.IO;

namespace RCamera
{
    [Activity(Label = "RCamera", MainLauncher = true, Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        public ImageView imageView;
        public Bitmap mBitmap;
        public TextView tvAge;
        public TextView tvGender;
        public TextView tvEmotion;
        public TextView tvDo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.Main);

            //각각의 UI value
            var btnEmotion = FindViewById<Button>(Resource.Id.btnEmotion);
            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            tvAge = FindViewById<TextView>(Resource.Id.tvAge);  //나이 추정값 
            tvGender = FindViewById<TextView>(Resource.Id.tvGender); //성별 추정값
            tvEmotion = FindViewById<TextView>(Resource.Id.tvEmotion);  //감정 추정값(가장 높은 감정)
            tvDo = FindViewById<TextView>(Resource.Id.tvDo);  //현재 하고있는 동작 or 상황

            //이미지 파일을 비트맵 값으로 바꿔줌
            mBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.yunzi2);
            imageView.SetImageBitmap(mBitmap);
            byte[] bitmapData;
            using (var stream = new MemoryStream())
            {
                mBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
                bitmapData = stream.ToArray();
            }

            var inputStream1 = new MemoryStream(bitmapData);  //face cognitive에 쓰일 inputStream
            var inputStream2 = new MemoryStream(bitmapData);  //Emotion cognitive에 쓰일 inputStream
            var inputStream3 = new MemoryStream(bitmapData);  //Vision cognitive에 쓰일 inputStream

            btnEmotion.Click += delegate
             {    
                 new EmotionTask(this).Execute(inputStream1);  //Emotion Task 시작점
                 new FaceTask(this).Execute(inputStream2);  //Face Task 시작점
                 new VisionTask(this).Execute(inputStream3);  //Vision Task 시작점
             };
            
        }        
    }
}

