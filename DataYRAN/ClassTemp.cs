using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataYRAN
{
  public  class ClassTemp
    {
      public  double[] mTemp { get; set; }
      
        public double Temp
        {
            get
            {
                return mTemp.Sum();
            }
        }
        public double TempPro(int i)
        {
            
                return mTemp[i]*100;
            
        }
        public double TempPro1
        {
            get
            {
                return mTemp[0] * 100;
            }

          

        }
        public int colSob { get; set; }
        public DateTime dateTime { get; set; }
        public string date()
        {
            return dateTime.Date.ToString();
        }
      
    }
}
