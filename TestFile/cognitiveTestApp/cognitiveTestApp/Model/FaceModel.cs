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

namespace cognitiveTestApp.Model
{
    public class FaceRectangle
    {
        public int height { get; set; }
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
    }
    public class FaceModel
    {
        public string faceId { get; set; }
        public FaceRectangle faceRectangle { get; set; }
    }
}