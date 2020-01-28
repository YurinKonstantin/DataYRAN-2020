using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataYRAN.Pasport
{
   public class RasAmp
    {
        public int znacRas { get; set; }
        public int[] mAmp = new int[12];
        public int[] MAmp
        {
            get
            {
                return mAmp;
            }
            set
            {
                mAmp = value;
            }
        }
        public int[] mAmpC = new int[12];
        public int[] MAmpC
        {
            get
            {
                return mAmpC;
            }
            set
            {
                mAmpC = value;
            }
        }

    }
}
