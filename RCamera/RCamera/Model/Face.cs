namespace RCamera.Model
{
    public class FaceRectangle
    {
        public int height { get; set; }
        public int left { get; set; }
        public int top { get; set; }
        public int width { get; set; }
    }

    public class FacialHair
    {
        public double beard { get; set; }
        public double moustache { get; set; }
        public double sideburns { get; set; }
    }

    public class HeadPose
    {
        public double pitch { get; set; }
        public double roll { get; set; }
        public double yaw { get; set; }
    }

    public class FaceAttributes
    {
      public double Age { get; set; }
      public FacialHair FacialHair { get; set; }    
      public string Gender { get; set; }
      public string Glasses { get; set; }
      public HeadPose HeadPose { get; set; }
      public double Smile { get; set; }
    }

    public class FaceModel
    {
        public FaceAttributes faceAttributes { get; set; }
        public string faceId { get; set; }
        public FaceRectangle faceRectangle { get; set; }        
    }
}