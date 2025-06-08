using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RudolfApp.Model
{
    public class FaceResult
    {
        public Rect FaceBox { get; set; }
        public Point NosePosition { get; set; }
    
        public FaceResult()
        {
            FaceBox = new Rect();
            NosePosition = new Point();
        }
    }
}
