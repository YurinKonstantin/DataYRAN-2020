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
      
    
        public async Task<List<ClassRasSig>> SigLine(List<ClassSob> classRazvertka1, int cloc)
        {
            List<ClassRasSig> DataColec = new List<ClassRasSig>();
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
                await editor3.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                    editor3.Text = "сигма пьедестала " + i.ToString()+" из "+ dateTimeEnd.Subtract(dateTimeFirst).TotalHours;
                });
                foreach (ClassSob classSob in orderedNumbers)
                {
                    try
                    {


                        DateTime dateTimeTec = new DateTime(classSob.dateUR.GG, classSob.dateUR.MM, classSob.dateUR.DD, classSob.dateUR.HH, classSob.dateUR.Min, classSob.dateUR.CC, 0);
                        
                        if (dateTimeTec.Subtract(dateTime).TotalHours >= 0 && dateTimeTec.Subtract(dateTime).TotalHours < cloc)
                        {
                            for (int ii = 0; ii < 12; ii++)
                            {
                                mas[ii] = mas[ii] + classSob.mSig[ii];
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
    

    DataColec.Add(new ClassRasSig() { dateTime = dateTime, mNullLine = mas, });

});

                col = 0;
                mas = new double[12];

                dateTime = dateTime.AddHours(cloc);
                dateTime1 = dateTime1.AddHours(cloc);

            }
   

            return DataColec;


        }
        public double[] sredSig(List<ClassRasSig> classRasSigs)
        {
            double[] mas = new double[12];
            foreach(var d in classRasSigs)
            {
                for (int i = 0; i < 12; i++)
                {
                    mas[i] += d.mNullLine[i];

                }
            }
      
            for (int i = 0; i < 12; i++)
            {
                int coun = (from s in classRasSigs where s.mNullLine[i] == 0 select s).Count();
                mas[i] = mas[i]/ ((double)classRasSigs.Count()-coun);

            }
            return mas;

        }
    }
}
