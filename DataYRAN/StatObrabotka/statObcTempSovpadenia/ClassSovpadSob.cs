using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataYRAN.StatObrabotka.statObcTempSovpadenia
{
  public  class ClassSovpadSob
    {
        public int Temp { get; set; }


        public int colSob1 { get; set; }
        public int colSob2 { get; set; }
        public int colSob3 { get; set; }
        public int colSob4 { get; set; }
        public int colSob5 { get; set; }
        public int colSob6 { get; set; }
        public int colSob12 { get; set; }
        public int colSob13 { get; set; }
        public int colSob23 { get; set; }
        public int colSob15 { get; set; }

        public DateTime dateTime { get; set; }
        public string date()
        {
            return dateTime.Date.ToString();
        }
        public string date1
        {
            get
            {
                return dateTime.Date.ToString();
            }


        }
    }
}

