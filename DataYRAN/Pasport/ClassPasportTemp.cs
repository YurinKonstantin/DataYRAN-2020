using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace DataYRAN.Pasport
{
    public partial class BlankPagePasport
    {
        public async  Task<List<ClassTemp>> TempSob(List<ClassSob> classRazvertka1, int cloc)
        {
            List<ClassTemp> classTemps = new List<ClassTemp>();
            var orderedNumbers = from ClassSob in classRazvertka1
                                 orderby ClassSob.dateUR.DateTimeString()
                                 select ClassSob;
            DateTime dateTime = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);
            DateTime dateTimeFirst = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);

            DateTime dateTime1 = dateTime; //new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
            DateTime dateTimeEnd = new DateTime(orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.GG, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.MM, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.DD, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.HH, 0, 0, 0);

            dateTime1 = dateTime1.AddHours(cloc);
            int col = 0;
            double[] mas = new double[12];
           
            for (int i = 0; i <= dateTimeEnd.Subtract(dateTimeFirst).TotalHours; i+=cloc)
            {
                int xx = 1;

                foreach (ClassSob classSob in orderedNumbers)
                {
                    DateTime dateTimeTec = new DateTime(classSob.dateUR.GG, classSob.dateUR.MM, classSob.dateUR.DD, classSob.dateUR.HH, classSob.dateUR.Min, classSob.dateUR.CC, 0);

                    if (dateTimeTec.Subtract(dateTime).TotalHours >= 0 && dateTimeTec.Subtract(dateTime).TotalHours < cloc)
                    {
                        for (int ii = 0; ii < 12; ii++)
                        {
                            if (classSob.mAmp[ii] >= 10)
                            {
                                mas[ii] = mas[ii] + 1;
                            }

                        }
                        xx = (int)dateTimeTec.Subtract(dateTime).TotalHours+1;
                        col++;
                    }
                    if (dateTimeTec.Subtract(dateTime).TotalHours > cloc)
                    {
                        break;
                    }
                }
                //await new MessageDialog(xx.ToString()).ShowAsync();
                if(col > 0)
                {
                    //col = col / xx;
                    for (int ii = 0; ii < 12; ii++)
                    {
                      
                            mas[ii] = mas[ii]/col;
                        

                    }

                }
                classTemps.Add(new ClassTemp() { dateTime = dateTime, mTemp = mas, colSob = col });
                //DataColecN.Add(new ClassTemp() { dateTime = dateTime, mTemp = masN });


                col = 0;
                mas = new double[12];
              
                dateTime = dateTime1;
                dateTime1 = dateTime1.AddHours(cloc);

            }

            return classTemps;

        }
        
        public async Task<List<ClassTemp>> TempSobNoNormirovka(List<ClassSob> classRazvertka1, int cloc)
        {
            List<ClassTemp> classTemps = new List<ClassTemp>();
            var orderedNumbers = from ClassSob in classRazvertka1
                                 orderby ClassSob.dateUR.DateTimeString()
                                 select ClassSob;
            DateTime dateTime = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);
            DateTime dateTimeFirst = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);

            DateTime dateTime1 = dateTime; //new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
            DateTime dateTimeEnd = new DateTime(orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.GG, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.MM, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.DD, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.HH, 0, 0, 0);

            dateTime1 = dateTime1.AddHours(cloc);
            int col = 0;
            double[] mas = new double[12];

            for (int i = 0; i <= dateTimeEnd.Subtract(dateTimeFirst).TotalHours; i += cloc)
            {
                int xx = 1;

                foreach (ClassSob classSob in orderedNumbers)
                {
                    DateTime dateTimeTec = new DateTime(classSob.dateUR.GG, classSob.dateUR.MM, classSob.dateUR.DD, classSob.dateUR.HH, classSob.dateUR.Min, classSob.dateUR.CC, 0);

                    if (dateTimeTec.Subtract(dateTime).TotalHours >= 0 && dateTimeTec.Subtract(dateTime).TotalHours < cloc)
                    {
                        for (int ii = 0; ii < 12; ii++)
                        {
                            if (classSob.mAmp[ii] >= 10)
                            {
                                mas[ii] = mas[ii] + 1;
                            }

                        }
                        xx = (int)dateTimeTec.Subtract(dateTime).TotalHours + 1;
                        col++;
                    }
                    if (dateTimeTec.Subtract(dateTime).TotalHours > cloc)
                    {
                        break;
                    }
                }
               
                classTemps.Add(new ClassTemp() { dateTime = dateTime, mTemp = mas, colSob = col });
                //DataColecN.Add(new ClassTemp() { dateTime = dateTime, mTemp = masN });


                col = 0;
                mas = new double[12];

                dateTime = dateTime1;
                dateTime1 = dateTime1.AddHours(cloc);

            }

            return classTemps;

        }
        public async Task<List<ClassTemp>> TempNeutron(List<ClassSob> classRazvertka1, int cloc)
        {
            List<ClassTemp> classTemps = new List<ClassTemp>();
            var orderedNumbers = from ClassSob in classRazvertka1
                                 orderby ClassSob.dateUR.DateTimeString()
                                 select ClassSob;
            DateTime dateTime = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);
            DateTime dateTimeFirst = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);

            DateTime dateTime1 = dateTime; //new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
            DateTime dateTimeEnd = new DateTime(orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.GG, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.MM, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.DD, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.HH, 0, 0, 0);

            dateTime1 = dateTime1.AddHours(cloc);
            int col = 0;
            double[] mas = new double[12];
            double[] masN = new double[12];
            for (int i = 0; i <= dateTimeEnd.Subtract(dateTimeFirst).TotalHours; i+=cloc)
            {


                foreach (ClassSob classSob in orderedNumbers)
                {
                    DateTime dateTimeTec = new DateTime(classSob.dateUR.GG, classSob.dateUR.MM, classSob.dateUR.DD, classSob.dateUR.HH, classSob.dateUR.Min, classSob.dateUR.CC, 0);

                    if (dateTimeTec.Subtract(dateTime).TotalHours >= 0 && dateTimeTec.Subtract(dateTime).TotalHours < cloc)
                    {
                        for (int ii = 0; ii < 12; ii++)
                        {
                            if (classSob.mAmp[ii] >= 10)
                            {
                                mas[ii] = mas[ii] + 1;
                            }

                        }
                        masN[0] = masN[0] + classSob.Nnut0;
                        masN[1] = masN[1] + classSob.Nnut1;
                        masN[2] = masN[2] + classSob.Nnut2;
                        masN[3] = masN[3] + classSob.Nnut3;
                        masN[4] = masN[4] + classSob.Nnut4;
                        masN[5] = masN[5] + classSob.Nnut5;
                        masN[6] = masN[6] + classSob.Nnut6;
                        masN[7] = masN[7] + classSob.Nnut7;
                        masN[8] = masN[8] + classSob.Nnut8;
                        masN[9] = masN[9] + classSob.Nnut9;
                        masN[10] = masN[10] + classSob.Nnut10;
                        masN[11] = masN[11] + classSob.Nnut11;
                        col++;
                    }
                    if (dateTimeTec.Subtract(dateTime).TotalHours > cloc)
                    {
                        break;
                    }
                }

                for (int ii = 0; ii < 12; ii++)
                {
                    if(col>0)
                    {
                        masN[ii] = masN[ii] / col;
                    }
                    else
                    {
                        masN[ii] = 0;
                    }
                   

                }
                classTemps.Add(new ClassTemp() { dateTime = dateTime, mTemp = masN, colSob=col });


                col = 0;
                mas = new double[12];
                masN = new double[12];
                dateTime = dateTime1;
                dateTime1 = dateTime1.AddHours(cloc);

            }
            return classTemps;


        }
        public double[] sredTemp(List<ClassTemp> classTempSred)
        {
            double[] mas = new double[12];
            foreach (var d in classTempSred)
            {
                for (int i = 0; i < 12; i++)
                {
                    mas[i] += d.mTemp[i];

                }
            }
            for (int i = 0; i < 12; i++)
            {
                int coun = (from ss in classTempSred.AsParallel() where ss.mTemp[i] == 0 select ss).Count();
                mas[i] = mas[i] / ((double)classTempSred.Count()-coun);

            }
            return mas;

        }
        public double[] sredTempN(List<ClassTemp> classTempSred)
        {
            double[] mas = new double[12];
            foreach (var d in classTempSred)
            {
                for (int i = 0; i < 12; i++)
                {
                    mas[i] += d.mTemp[i];

                }
            }
            for (int i = 0; i < 12; i++)
            {
                int coun = (from ss in classTempSred.AsParallel() where ss.mTemp[i] == 0 select ss).Count();
                
                mas[i] = mas[i] / ((double)classTempSred.Count()-coun);

            }
            return mas;

        }
    }
}
