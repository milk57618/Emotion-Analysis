using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using Android.Graphics;
using System.IO;
using Android.Content;
using Android.Provider;
using System;
using Android.Runtime;

namespace RCamera
{
    [Activity(Label = "RCamera", MainLauncher = true, Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        public CognitiveActivity cognitiveActivity;
        public ImageView logo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Main);
            logo = FindViewById<ImageView>(Resource.Id.Logo);
            var mBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.RCam);
            logo.SetImageBitmap(mBitmap);

            var btnCamera = FindViewById<Button>(Resource.Id.btnCamera);
            var btnText = FindViewById<Button>(Resource.Id.btnText);
            btnCamera.Click += BtnCamera_Click;
            btnText.Click += BtnText_Click;
        }

        /// <summary>
        /// TextOutput 창으로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnText_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(TextActivity));
            StartActivity(intent);
        }

        /// <summary>
        /// ImageOutput 창으로 이동
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCamera_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CognitiveActivity));
            StartActivity(intent);
        }        
    }
}

