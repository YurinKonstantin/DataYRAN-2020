using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataYRAN
{
  public  class ClassSobColl
    {
        string startTime;
        public string StartTime
        { get { return startTime; } set { startTime = value; } }
        string endTime;
        public string EndTime
        { get { return endTime; } set { endTime = value; } }
        int summAmpl;
        public int SummAmpl
        { get { return summAmpl; } set { summAmpl = value; } }
        int summNeu;
        public int SummNeu
        { get { return summNeu; } set { summNeu = value; } }
        int sumClast;
        public int SumClast
        { get { return sumClast; } set { sumClast = value; } }
        public DataTimeUR GetDataisNS(long time)
        {
            DataTimeUR dataTimeUR = new DataTimeUR();
            Int64 result;
            
            Int64 div = Math.DivRem(time, Convert.ToInt64(3.6 * 1000000000000), out result);
            dataTimeUR.HH = Convert.ToInt32(div);

            div = Math.DivRem(result, Convert.ToInt64(60000000000), out result);
            dataTimeUR.Min= Convert.ToInt32(div);

            div = Math.DivRem(result, Convert.ToInt64(1000000000), out result);
            dataTimeUR.CC = Convert.ToInt32(div);

            div = Math.DivRem(result, Convert.ToInt64(1000000), out result);
            dataTimeUR.Mil = Convert.ToInt32(div);

            div = Math.DivRem(result, Convert.ToInt64(1000), out result);
            dataTimeUR.ML = Convert.ToInt32(div);

            dataTimeUR.NN = Convert.ToInt32(result);

            return dataTimeUR;

        }

        int sumClastUp;
        public int SumClastUp
        {
            get
            {
                int x = 0;
                foreach (ClassSob classSob in col)
                {
                    for(int i=0; i<12; i++)
                    {
                        if(classSob.mAmp[i]>10)
                        {
                            x++;
                        }
                    }
                

                }
                return x;
            }
            set { sumClastUp = value; } }
        public void SumAmpAndNeutronAndClaster()
        {
            foreach(ClassSob classSob in col)
            {
                SummAmpl = SummAmpl + classSob.SumAmp;
                SummNeu = SummNeu + classSob.SumNeu;
                
            }
            SumClast = col.Count;
        }
        public void FirstTimeS()
        {
            foreach (ClassSob classSob in col)
            {
                SummAmpl = SummAmpl + classSob.SumAmp;
                SummNeu = SummNeu + classSob.SumNeu;

            }
        }
       public List<ClassSob> col = new List<ClassSob>();
        public DataTimeUR dateUR { get; set; }
        public string klSob
        {
            
            get
            {
                string ki = String.Empty;
                foreach (ClassSob classSob in col)
                {
                    ki += classSob.nameklaster.ToString() + ",";

                }
                return ki;
            }
        }
        public Int64 GetTimeNS
        {
            get
            {


               return dateUR.GetTimeNS;
            }
        }
        public int NDn
        {

            get
            {
                int x = 0;
                foreach (ClassSob classSob in col)
                {
                   if(classSob.Nnut0>0)
                    {
                        x++;
                    }
                    if (classSob.Nnut1 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut2 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut3 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut4 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut5 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut6 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut7 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut8 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut9 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut10 > 0)
                    {
                        x++;
                    }
                    if (classSob.Nnut11 > 0)
                    {
                        x++;
                    }

                }
                return x;
            }
        }
        public string GetID()
        {


        
            return dateUR.GG.ToString("0000") + "-" + dateUR.MM.ToString("00") + "-" + dateUR.DD.ToString("00")
                                         + "_ue_" + dateUR.HH.ToString("00") + ":" + dateUR.Min.ToString("00") + ":" + dateUR.CC.ToString("00")
                                         + "." + dateUR.Mil.ToString("000") + "." + dateUR.ML.ToString("000") + "." + dateUR.NN.ToString("000");
            
        }
        public string GetID(long d)
        {


            DataTimeUR dataTimeUR = GetDataisNS(d);
            return dateUR.GG.ToString("0000") + "-" + dateUR.MM.ToString("00") + "-" + dateUR.DD.ToString("00")
                                         + "_ue_" + dataTimeUR.HH.ToString("00") + ":" + dataTimeUR.Min.ToString("00") + ":" + dataTimeUR.CC.ToString("00")
                                         + "." + dataTimeUR.Mil.ToString("000") + "." + dataTimeUR.ML.ToString("000") + "." + dataTimeUR.NN.ToString("000");

        }
        public int GetMask
        {
            get
            {


                string nemaska = String.Empty;

                for (int i = 5; i >= 0; i--)
                {


                    var coll = (from f in col where f.nameklaster == Convert.ToString(i + 1) select f).ToList();
                    Debug.WriteLine(i + "\t" + coll.Count());
                    if (coll.Count > 0)
                    {
                        nemaska += "1";
                    }
                    else
                    {
                        nemaska += "0";
                    }
                }

                return Convert.ToInt32(nemaska, 2);
            }
            
        }
       

    }
}
