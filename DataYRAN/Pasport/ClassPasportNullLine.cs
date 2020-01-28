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
       
        public async Task<List<NullLine>> NullLinevoid(List<ClassSob> classRazvertka1, int cloc)
        {
            List<NullLine> nullLines = new List<NullLine>();
            var orderedNumbers = from ClassSob in classRazvertka1
                                 orderby ClassSob.dateUR.DateTimeString()
                                 select ClassSob;
            DateTime dateTime = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);
            DateTime dateTimeFirst = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);

            DateTime dateTime1 = dateTime; //new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
            DateTime dateTimeEnd = new DateTime(orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.GG, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.MM, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.DD, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.HH, 0, 0, 0);

            dateTime1 = dateTime1.AddHours(cloc);
            int col = 0;
            int[] mas = new int[12];
            int[] masN = new int[12];
            
            for (int i = 0; i <= dateTimeEnd.Subtract(dateTimeFirst).TotalHours; i+=cloc)
            {

                
                foreach (ClassSob classSob in orderedNumbers)
                {
                    try
                    {


                        DateTime dateTimeTec = new DateTime(classSob.dateUR.GG, classSob.dateUR.MM, classSob.dateUR.DD, classSob.dateUR.HH, classSob.dateUR.Min, classSob.dateUR.CC, 0);

                        if (dateTimeTec.Subtract(dateTime).TotalHours >= 0 && dateTimeTec.Subtract(dateTime).TotalHours < cloc)
                        {
                            for (int ii = 0; ii < 12; ii++)
                            {

                                mas[ii] = mas[ii] + classSob.mNull[ii];

                            }

                            col++;
                        }
                        if (dateTimeTec.Subtract(dateTime).TotalHours > cloc)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageDialog messageDialog = new MessageDialog(ex.ToString() + dateTime1.Hour.ToString());
                        messageDialog.ShowAsync();
                    }
                }
                for (int j = 0; j < 12; j++)
                {
                    if (col > 0)
                        mas[j] = mas[j] / col;
                    else
                        mas[j] = 0;
                }
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    nullLines.Add(new NullLine() { dateTime = dateTime, mNullLine = mas, });

});

                col = 0;
                mas = new int[12];
                masN = new int[12];
                dateTime = dateTime1;
                dateTime1 = dateTime1.AddHours(cloc);

            }

            return nullLines;

        }
        public async Task<List<NullLine>> NullLinevoid(List<ClassSob> classRazvertka1, int cloc, bool f)
        {
            List<NullLine> nullLines = new List<NullLine>();
            var orderedNumbers = from ClassSob in classRazvertka1
                                 orderby ClassSob.dateUR.DateTimeString()
                                 select ClassSob;
            DateTime dateTime = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);
            DateTime dateTimeFirst = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);

            DateTime dateTime1 = dateTime; //new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
            DateTime dateTimeEnd = new DateTime(orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.GG, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.MM, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.DD, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.HH, 0, 0, 0);

            dateTime1 = dateTime1.AddHours(cloc);
            int col = 0;
            int[] mas = new int[12];
            int[] masN = new int[12];
            int pox = 0;
            for (int i = 0; i <= dateTimeEnd.Subtract(dateTimeFirst).TotalHours; i += cloc)
            {

                for (int j = pox; j < orderedNumbers.Count(); j++)
                {
                    ClassSob classSob = orderedNumbers.ElementAt(j);
                    try
                    {


                        DateTime dateTimeTec = new DateTime(classSob.dateUR.GG, classSob.dateUR.MM, classSob.dateUR.DD, classSob.dateUR.HH, classSob.dateUR.Min, classSob.dateUR.CC, 0);

                        if (dateTimeTec.Subtract(dateTime).TotalHours >= 0 && dateTimeTec.Subtract(dateTime).TotalHours < cloc)
                        {
                            for (int ii = 0; ii < 12; ii++)
                            {

                                mas[ii] = mas[ii] + classSob.mNull[ii];

                            }

                            col++;
                           
                        }
                        if (dateTimeTec.Subtract(dateTime).TotalHours > cloc)
                        {
                            pox=j;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageDialog messageDialog = new MessageDialog(ex.ToString() + dateTime1.Hour.ToString());
                        messageDialog.ShowAsync();
                    }
                }
              
                for (int j = 0; j < 12; j++)
                {
                    if (col > 0)
                        mas[j] = mas[j] / col;
                    else
                        mas[j] = 0;
                }
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    nullLines.Add(new NullLine() { dateTime = dateTime, mNullLine = mas, });

});

                col = 0;
                mas = new int[12];
                masN = new int[12];
                dateTime = dateTime1;
                dateTime1 = dateTime1.AddHours(cloc);

            }

            return nullLines;

        }
        public double[] sredNull(List<NullLine> classNullLines)
        {
            double[] mas = new double[12];
            foreach (var d in classNullLines)
            {
                for (int i = 0; i < 12; i++)
                {
                    mas[i] += d.mNullLine[i];

                }
            }
            for (int i = 0; i < 12; i++)
            {
                int coun = (from s in classNullLines.AsParallel() where s.mNullLine[i] == 0 select s).Count();
                mas[i] = mas[i] / ((double)classNullLines.Count()-coun);

            }
            return mas;

        }
    }
}
