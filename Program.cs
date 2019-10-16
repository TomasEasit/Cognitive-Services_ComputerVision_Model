using System;
using System.Collections.Generic;

namespace Cognitive_Services_ComputerVision_Model
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }


        // Creates a ImageObject that contains several other objects. 
        public class ImageObject
        {
            public Description description { get; set; }
            public string requestId { get; set; }
            public Metadata metadata { get; set; }
        }

        public class Caption
        {
            public string text { get; set; }
            public double confidence { get; set; }
        }

        public class Description
        {
            public List<string> tags { get; set; }
            public List<Caption> captions { get; set; }
        }

        public class Metadata
        {
            public int height { get; set; }
            public int width { get; set; }
            public string format { get; set; }
        }
    }
}
