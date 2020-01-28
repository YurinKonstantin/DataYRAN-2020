﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace DataYRAN
{
    public class ClassSob
    {


        public int count { get; set; }
        public string nameTip;
        public string nameFile { get; set; }
        public string nameklaster { get; set; }
        public string nameBAAK { get; set; }

        int sumAmp = 0;
        public int SumAmp
        {
            get
            {
                return mAmp.Sum();
            }

            set
            {
                sumAmp = value;
            }
        }
        public DataTimeUR dateUR { get; set; }
        public string DataTimeV
            {
            get
            {
                return dateUR.DateString();
            }
            }
        int sumNeu = 0;
        public int SumNeu {

            get
            {
                return classSobNeutronsList.Count;
            }
            set
            {
                classSobNeutronsList = new List<ClassSobNeutron>(value);


            }
        }
        public string time { get; set; }
        int[] _mAmp = new int[12];
        public int[] mAmp
        {
            get
            {
                return _mAmp;
            }
            set
            {
                _mAmp = value;
            }
        }
       
        int[] _mTimeD = new int[12];
        public int[] mTimeD { get; set; }
      public int[] mCountN
        {
            get
            {
                int[] bb = new int[12];
                for(int i=0; i<12; i++)
                {
                    var dd = from ClassSobNeutron in classSobNeutronsList.AsParallel()
                             where ClassSobNeutron.D == (i + 1)
                             select ClassSobNeutron;
                    bb[i] = bb[i] + dd.Count(); count=count+ dd.Count();

                   // foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                    {
                       // if (classSobNeutron.D == i+1)
                        {
                         //   bb[i] = bb[i] + 1; count++;
                        }
                    }
                }
                return bb;
                
            }
        }
        public short Nnut0
        {
            get
            {
                short count = 0;
                foreach(ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if(classSobNeutron.D==1)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public short Nnut1
        {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 2)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut2
        {


            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 3)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut3 {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 4)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut4
        {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 5)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut5
        {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 6)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut6
        {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 7)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut7
        {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 8)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut8
        {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 9)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut9
        {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 10)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut10
        {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 11)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public short Nnut11
        {
            get
            {
                short count = 0;
                foreach (ClassSobNeutron classSobNeutron in classSobNeutronsList)
                {
                    if (classSobNeutron.D == 12)
                    {
                        count++;
                    }
                }
                return count;
            }
        }
        public double sig0 { get; set; }
        public double sig1 { get; set; }
        public double sig2 { get; set; }
        public double sig3 { get; set; }
        public double sig4 { get; set; }
        public double sig5 { get; set; }
        public double sig6 { get; set; }
        public double sig7 { get; set; }
        public double sig8 { get; set; }
        public double sig9 { get; set; }
        public double sig10 { get; set; }
        public double sig11 { get; set; }
        public double[] mSig
        {
            get
            {
                double[] m = new double[12];
                m[0] = sig0;
                m[1] = sig1;
                m[2] = sig2;
                m[3] = sig3;
                m[4] = sig4;
                m[5] = sig5;
                m[6] = sig6;
                m[7] = sig7;
                m[8] = sig8;
                m[9] = sig9;
                m[10] = sig10;
                m[11] = sig11;
                return m;
            }
        }
        public int[] mNull
        {
            get
            {
                int[] m = new int[12];
                m[0] = Convert.ToInt32(Nnull0);
                m[1] = Convert.ToInt32(Nnull1);
                m[2] = Convert.ToInt32(Nnull2);
                m[3] = Convert.ToInt32(Nnull3);
                m[4] = Convert.ToInt32(Nnull4);
                m[5] = Convert.ToInt32(Nnull5);
                m[6] = Convert.ToInt32(Nnull6);
                m[7] = Convert.ToInt32(Nnull7);
                m[8] = Convert.ToInt32(Nnull8);
                m[9] = Convert.ToInt32(Nnull9);
                m[10] = Convert.ToInt32(Nnull10);
                m[11] = Convert.ToInt32(Nnull11);
                return m;
            }
        }
        public short Nnull0 { get; set; }
        public short Nnull1 { get; set; }
        public short Nnull2 { get; set; }
        public short Nnull3 { get; set; }
        public short Nnull4 { get; set; }
        public short Nnull5 { get; set; }
        public short Nnull6 { get; set; }
        public short Nnull7 { get; set; }
        public short Nnull8 { get; set; }
        public short Nnull9 { get; set; }
        public short Nnull10 { get; set; }
        public short Nnull11 { get; set; }

        public string TimeS0 { get; set; }
        public string TimeS1 { get; set; }
        public string TimeS2 { get; set; }
        public string TimeS3 { get; set; }
        public string TimeS4 { get; set; }
        public string TimeS5 { get; set; }
        public string TimeS6 { get; set; }
        public string TimeS7 { get; set; }
        public string TimeS8 { get; set; }
        public string TimeS9 { get; set; }
        public string TimeS10 { get; set; }
        public string TimeS11 { get; set; }

        public int[] masTime()
        {
            int[] mas = new int[12];
            mas[0] = Convert.ToInt32(TimeS0);
            mas[1] = Convert.ToInt32(TimeS1);
            mas[2] = Convert.ToInt32(TimeS2);
            mas[3] = Convert.ToInt32(TimeS3);
            mas[4] = Convert.ToInt32(TimeS4);
            mas[5] = Convert.ToInt32(TimeS5);
            mas[6] = Convert.ToInt32(TimeS6);
            mas[7] = Convert.ToInt32(TimeS7);
            mas[8] = Convert.ToInt32(TimeS8);
            mas[9] = Convert.ToInt32(TimeS9);
            mas[10] = Convert.ToInt32(TimeS10);
            mas[11] = Convert.ToInt32(TimeS11);

            return mas;
        }
     //  public short[] AmpSum()
       // {
       //  return new short[12] { Amp0, Amp1, Amp2, Amp3, Amp4, Amp5, Amp6, Amp7, Amp8, Amp9, Amp10, Amp11 };
     //  }
       public List<string> ShovSelect()
        {
            List<string> vs = new List<string>();
            vs.Add(vs.Count.ToString() + " " + mAmp[0].ToString() + " " + sig0.ToString());

           return vs;
        }

    

       public List<ClassSobNeutron> classSobNeutronsList { get; set; }







    }
 
}
