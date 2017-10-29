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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Main);

            var btnCamera = FindViewById<Button>(Resource.Id.btnCamera);            
            btnCamera.Click += BtnCamera_Click;
        }

        private void BtnCamera_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(CognitiveActivity));
            StartActivity(intent);
        }        
    }
}

