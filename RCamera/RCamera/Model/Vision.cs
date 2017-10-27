using System.Collections.Generic;

namespace RCamera.Model
{
    public class VisionModel
    {
        public Description description { get; set; }
        public Metadata metadata { get; set; }
        public string requestIdp { get; set; }
    }

    public class Caption
    {
        public double confidence { get; set; }
        public string text { get; set; }
    }

    public class Description
    {
        public List<Caption> captions { get; set; }
        public List<string> tags { get; set; }
    }

    public class Metadata
    {
        public string format { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }
}