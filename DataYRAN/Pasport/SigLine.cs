using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataYRAN.Pasport
{
    public class ClassRasSig
    {
        public double[] mNullLine { get; set; }



        public DateTime dateTime { get; set; }
        public string date()
        {
            return dateTime.Date.ToString();
        }
    }
}
