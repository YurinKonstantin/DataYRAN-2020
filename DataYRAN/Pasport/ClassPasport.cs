using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DataYRAN.Pasport
{
   public class ClassPasport
    {
        public string name { get; set; }
        public string stringRich { get; set; }
      
       public List<ClassSob> classSobs = new List<ClassSob>();
       public List<RasAmp> classRasAmp = new List<RasAmp>();
        public List<ClassRasSig> classSig = new List<ClassRasSig>();
        public List<ClassRasSig> classSig6()
        { List<ClassRasSig> classSig6 = new List<ClassRasSig>();
            try
            {
                 
                    int x = 1;
                    double[] mas = new double[12];
                    foreach (var d in classSig)
                {
                        x++;
                        for (int i = 0; i < 12; i++)
                        {
                            mas[i] += d.mNullLine[i];
                        }
                        if (x == 7)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                mas[i] = mas[i] / 6;
                            }
                            x = 1;
                        classSig6.Add(new ClassRasSig() { dateTime = d.dateTime.AddHours(-6), mNullLine = mas });
                            mas = new double[12];
                        }
                    }
                   
                    if (x != 1)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            mas[i] = mas[i] / (x - 1);
                        }

                    classSig6.Add(new ClassRasSig() { dateTime = classSig6.ElementAt(classSig6.Count() - 1).dateTime.AddHours(6), mNullLine = mas });
                    }
                 
            }
            catch (Exception ex)
            {
                
            }
            return classSig6;

        }
        public List<NullLine> classNullLine = new List<NullLine>();
       
        public double[] mas = new double[12];

        public double[] masSredNull = new double[12];
      public  List<ClassTemp> classTemps = new List<ClassTemp>();
        public List<ClassTemp> classTempsNoNorm = new List<ClassTemp>();
        public List<ClassTemp> classTempSob6()
        {
            List<ClassTemp> classSig6 = new List<ClassTemp>();
            try
            {

                int x = 1;
                double[] mas = new double[12];
                foreach (var d in classTemps)
                {
                    x++;
                    for (int i = 0; i < 12; i++)
                    {
                        mas[i] += (int)d.mTemp[i];
                    }
                    if (x == 7)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            mas[i] = mas[i] / 6;
                        }
                        x = 1;
                        classSig6.Add(new ClassTemp() { dateTime = d.dateTime.AddHours(-5), mTemp = mas });
                        mas = new double[12];
                    }
                }

                if (x != 1)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        mas[i] = mas[i] / (x - 1);
                    }

                    classSig6.Add(new ClassTemp() { dateTime = classSig6.ElementAt(classSig6.Count() - 1).dateTime.AddHours(5), mTemp = mas });
                }

            }
            catch (Exception ex)
            {

            }
            return classSig6;

        }
        public double[] masSredTemp = new double[12];
        public List<ClassTemp> classTempsN = new List<ClassTemp>();
        public List<ClassTemp> classTempSobN6()
        {
            List<ClassTemp> classSig6 = new List<ClassTemp>();
            try
            {

                int x = 1;
                double[] mas = new double[12];
                foreach (var d in classTempsN)
                {
                    x++;
                    for (int i = 0; i < 12; i++)
                    {
                        mas[i] += d.mTemp[i];
                    }
                    if (x == 7)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            mas[i] = mas[i] / 6;
                        }
                        x = 1;
                        classSig6.Add(new ClassTemp() { dateTime = d.dateTime.AddHours(-5), mTemp = mas });
                        mas = new double[12];
                    }
                }

                if (x != 1)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        mas[i] = mas[i] / (x - 1);
                    }

                    classSig6.Add(new ClassTemp() { dateTime = classSig6.ElementAt(classSig6.Count() - 1).dateTime.AddHours(5), mTemp = mas });
                }

            }
            catch (Exception ex)
            {

            }
            return classSig6;

        }
        public double[] masSredTempN = new double[12];
       public BitmapImage image { get; set; }

        public async void saveAmpRas(StorageFolder folder)
        {
            try
            {


                if (folder != null)
                {




                    if (classRasAmp.Count != 0)
                    {
                        using (StreamWriter writer =
                       new StreamWriter(await folder.OpenStreamForWriteAsync(
                       name+" "+"Распределение Amp" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                        {
                            string sSob = "Код АЦП" + "\t" + "D1" + "\t" + "D2" + "\t" + "D3" + "\t" + "D4" + "\t" + "D5" + "\t" + "D6" + "\t" + "D7"
                                + "\t" + "D8" + "\t" + "D9" + "\t" + "D10" + "\t" + "D11" + "\t" + "D12";


                            await writer.WriteLineAsync(sSob);
                            foreach (RasAmp sob in classRasAmp)
                            {

                                string Sob = sob.znacRas.ToString() + "\t" + sob.MAmp[0].ToString() + "\t" + sob.MAmp[1].ToString() + "\t" +
                                sob.MAmp[2].ToString() + "\t" + sob.MAmp[3].ToString() + "\t" + sob.MAmp[4].ToString() + "\t" + sob.MAmp[5].ToString() + "\t" + sob.MAmp[6].ToString() + "\t" + sob.MAmp[7].ToString() +
                                "\t" + sob.MAmp[8].ToString() + "\t" + sob.MAmp[9].ToString() + "\t" + sob.MAmp[10].ToString() + "\t" + sob.MAmp[11].ToString();
                                await writer.WriteLineAsync(Sob);
                            }
                        }
                    }



                }
                else
                {

                }
            }
            catch(Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message+"\n"+ex.ToString());
                await messageDialog.ShowAsync();
            }
        }
        public async void saveSig(StorageFolder folder)
        {
            try
            {


                if (folder != null)
                {




                    if (classSig.Count != 0)
                    {
                        using (StreamWriter writer =
                       new StreamWriter(await folder.OpenStreamForWriteAsync(
                       name + " " + "Sig" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                        {
                            string sSob = "Дата" + "\t" + "D1" + "\t" + "D2" + "\t" + "D3" + "\t" + "D4" + "\t" + "D5" + "\t" + "D6" + "\t" + "D7"
                                + "\t" + "D8" + "\t" + "D9" + "\t" + "D10" + "\t" + "D11" + "\t" + "D12";


                            await writer.WriteLineAsync(sSob);
                            foreach (ClassRasSig sob in classSig)
                            {

                                string Sob = sob.date().ToString() + "\t" + sob.mNullLine[0].ToString() + "\t" + sob.mNullLine[1].ToString() + "\t" +
                                sob.mNullLine[2].ToString() + "\t" + sob.mNullLine[3].ToString() + "\t" + sob.mNullLine[4].ToString() + "\t" + sob.mNullLine[5].ToString() + "\t" + sob.mNullLine[6].ToString() + "\t" + sob.mNullLine[7].ToString() +
                                "\t" + sob.mNullLine[8].ToString() + "\t" + sob.mNullLine[9].ToString() + "\t" + sob.mNullLine[10].ToString() + "\t" + sob.mNullLine[11].ToString();
                                await writer.WriteLineAsync(Sob);
                            }
                        }
                    }



                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message + "\n" + ex.ToString());
                await messageDialog.ShowAsync();
            }
        }
        public async void saveSig6(StorageFolder folder)
        {
            try
            {


                if (folder != null)
                {




                    if (classSig.Count != 0)
                    {
                        using (StreamWriter writer =
                       new StreamWriter(await folder.OpenStreamForWriteAsync(
                       name + " " + "Sig6" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                        {
                            string sSob = "Дата" + "\t" + "D1" + "\t" + "D2" + "\t" + "D3" + "\t" + "D4" + "\t" + "D5" + "\t" + "D6" + "\t" + "D7"
                                + "\t" + "D8" + "\t" + "D9" + "\t" + "D10" + "\t" + "D11" + "\t" + "D12";


                            await writer.WriteLineAsync(sSob);
                            foreach (ClassRasSig sob in classSig)
                            {

                                string Sob = sob.date().ToString() + "\t" + sob.mNullLine[0].ToString() + "\t" + sob.mNullLine[1].ToString() + "\t" +
                                sob.mNullLine[2].ToString() + "\t" + sob.mNullLine[3].ToString() + "\t" + sob.mNullLine[4].ToString() + "\t" + sob.mNullLine[5].ToString() + "\t" + sob.mNullLine[6].ToString() + "\t" + sob.mNullLine[7].ToString() +
                                "\t" + sob.mNullLine[8].ToString() + "\t" + sob.mNullLine[9].ToString() + "\t" + sob.mNullLine[10].ToString() + "\t" + sob.mNullLine[11].ToString();
                                await writer.WriteLineAsync(Sob);
                            }
                        }
                    }



                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message + "\n" + ex.ToString());
                await messageDialog.ShowAsync();
            }
        }
        public async void saveNull(StorageFolder folder)
        {
            try
            {


                if (folder != null)
                {




                    if (classNullLine.Count != 0)
                    {
                        using (StreamWriter writer =
                       new StreamWriter(await folder.OpenStreamForWriteAsync(
                       name + " " + "NullLine" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                        {
                            string sSob = "Дата" + "\t" + "D1" + "\t" + "D2" + "\t" + "D3" + "\t" + "D4" + "\t" + "D5" + "\t" + "D6" + "\t" + "D7"
                                + "\t" + "D8" + "\t" + "D9" + "\t" + "D10" + "\t" + "D11" + "\t" + "D12";


                            await writer.WriteLineAsync(sSob);
                            foreach (NullLine sob in classNullLine)
                            {

                                string Sob = sob.dateTime.ToString() + "\t" + sob.mNullLine[0].ToString() + "\t" + sob.mNullLine[1].ToString() + "\t" +
                                sob.mNullLine[2].ToString() + "\t" + sob.mNullLine[3].ToString() + "\t" + sob.mNullLine[4].ToString() + "\t" + sob.mNullLine[5].ToString() + "\t" + sob.mNullLine[6].ToString() + "\t" + sob.mNullLine[7].ToString() +
                                "\t" + sob.mNullLine[8].ToString() + "\t" + sob.mNullLine[9].ToString() + "\t" + sob.mNullLine[10].ToString() + "\t" + sob.mNullLine[11].ToString();
                                await writer.WriteLineAsync(Sob);
                            }
                        }
                    }



                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message + "\n" + ex.ToString());
                await messageDialog.ShowAsync();
            }
        }
        public async void saveNull6(StorageFolder folder)
        {
            try
            {


                if (folder != null)
                {

                    List<NullLine> lines = new List<NullLine>();
                    int x = 1;
                    int[] mas = new int[12];
                    foreach(var d in classNullLine)
                    {
                        x++;
                        for(int i=0; i<12; i++)
                        {
                            mas[i] += d.mNullLine[i];
                        }
                        if(x==7)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                mas[i] = mas[i]/6;
                            }
                            x = 1;
                            lines.Add(new NullLine() { dateTime=d.dateTime.AddHours(-5), mNullLine=mas});
                            mas = new int[12];
                        }
                    }
                    Debug.WriteLine(x.ToString());
                    if(x!=1)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            mas[i] = mas[i] / (x-1);
                        }
                        Debug.WriteLine(lines.ElementAt(lines.Count() - 1).dateTime.AddHours(5).ToString());
                        lines.Add(new NullLine() { dateTime = lines.ElementAt(lines.Count()-1).dateTime.AddHours(5), mNullLine = mas });
                    }
                    if (lines.Count != 0)
                    {
                        using (StreamWriter writer =
                       new StreamWriter(await folder.OpenStreamForWriteAsync(
                       name + " " + "NullLine6" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                        {
                            string sSob = "Дата" + "\t" + "D1" + "\t" + "D2" + "\t" + "D3" + "\t" + "D4" + "\t" + "D5" + "\t" + "D6" + "\t" + "D7"
                                + "\t" + "D8" + "\t" + "D9" + "\t" + "D10" + "\t" + "D11" + "\t" + "D12";


                            await writer.WriteLineAsync(sSob);
                            foreach (NullLine sob in lines)
                            {

                                string Sob = sob.dateTime.ToString() + "\t" + sob.mNullLine[0].ToString() + "\t" + sob.mNullLine[1].ToString() + "\t" +
                                sob.mNullLine[2].ToString() + "\t" + sob.mNullLine[3].ToString() + "\t" + sob.mNullLine[4].ToString() + "\t" + sob.mNullLine[5].ToString() + "\t" + sob.mNullLine[6].ToString() + "\t" + sob.mNullLine[7].ToString() +
                                "\t" + sob.mNullLine[8].ToString() + "\t" + sob.mNullLine[9].ToString() + "\t" + sob.mNullLine[10].ToString() + "\t" + sob.mNullLine[11].ToString();
                                await writer.WriteLineAsync(Sob);
                            }
                        }
                    }



                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message + "\n" + ex.ToString());
                await messageDialog.ShowAsync();
            }
        }
        public async void save(StorageFolder folder)
        {
            StorageFolder storageFolder = await folder.CreateFolderAsync("ДопИнфа "+name, CreationCollisionOption.GenerateUniqueName);
            saveAmpRas(storageFolder);
            saveSig(storageFolder);
            saveSig6(storageFolder);
            saveNull(storageFolder);
            saveNull6(storageFolder);
            saveTemp(storageFolder);
            saveTemp6(storageFolder);
            saveTempN(storageFolder);
            saveTempN6(storageFolder);
        }
        public async void saveTemp(StorageFolder folder)
        {
            try
            {


                if (folder != null)
                {




                    if (classTemps.Count != 0)
                    {
                        using (StreamWriter writer =
                       new StreamWriter(await folder.OpenStreamForWriteAsync(
                       name + " " + "TempSob" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                        {
                            string sSob = "Дата" + "\t" + "D1" + "\t" + "D2" + "\t" + "D3" + "\t" + "D4" + "\t" + "D5" + "\t" + "D6" + "\t" + "D7"
                                + "\t" + "D8" + "\t" + "D9" + "\t" + "D10" + "\t" + "D11" + "\t" + "D12";


                            await writer.WriteLineAsync(sSob);
                            foreach (ClassTemp sob in classTemps)
                            {

                                string Sob = sob.dateTime.ToString() + "\t" + sob.mTemp[0].ToString() + "\t" + sob.mTemp[1].ToString() + "\t" +
                                sob.mTemp[2].ToString() + "\t" + sob.mTemp[3].ToString() + "\t" + sob.mTemp[4].ToString() + "\t" + sob.mTemp[5].ToString() + "\t" + sob.mTemp[6].ToString() + "\t" + sob.mTemp[7].ToString() +
                                "\t" + sob.mTemp[8].ToString() + "\t" + sob.mTemp[9].ToString() + "\t" + sob.mTemp[10].ToString() + "\t" + sob.mTemp[11].ToString();
                                await writer.WriteLineAsync(Sob);
                            }
                        }
                    }



                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message + "\n" + ex.ToString());
                await messageDialog.ShowAsync();
            }
        }
        public async void saveTemp6(StorageFolder folder)
        {
            try
            {


                if (folder != null)
                {

                    List<ClassTemp> lines = new List<ClassTemp>();
                    int x = 1;
                    double[] mas = new double[12];
                    foreach (ClassTemp d in classTemps)
                    {
                        x++;
                        for (int i = 0; i < 12; i++)
                        {
                            mas[i] += d.mTemp[i];
                        }
                        if (x == 7)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                mas[i] = mas[i] / 6;
                            }
                            x = 1;
                            lines.Add(new ClassTemp() { dateTime = d.dateTime.AddHours(-5), mTemp = mas });
                            mas = new double[12];
                        }
                    }
                    Debug.WriteLine(x.ToString());
                    if (x != 1)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            mas[i] = mas[i] / (x - 1);
                        }
                        Debug.WriteLine(lines.ElementAt(lines.Count() - 1).dateTime.AddHours(5).ToString());
                        lines.Add(new ClassTemp() { dateTime = lines.ElementAt(lines.Count() - 1).dateTime.AddHours(5), mTemp = mas });
                    }
                    if (lines.Count != 0)
                    {
                        using (StreamWriter writer =
                       new StreamWriter(await folder.OpenStreamForWriteAsync(
                       name + " " + "TempSob6" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                        {
                            string sSob = "Дата" + "\t" + "D1" + "\t" + "D2" + "\t" + "D3" + "\t" + "D4" + "\t" + "D5" + "\t" + "D6" + "\t" + "D7"
                                + "\t" + "D8" + "\t" + "D9" + "\t" + "D10" + "\t" + "D11" + "\t" + "D12";


                            await writer.WriteLineAsync(sSob);
                            foreach (ClassTemp sob in lines)
                            {

                                string Sob = sob.dateTime.ToString() + "\t" + sob.mTemp[0].ToString() + "\t" + sob.mTemp[1].ToString() + "\t" +
                                sob.mTemp[2].ToString() + "\t" + sob.mTemp[3].ToString() + "\t" + sob.mTemp[4].ToString() + "\t" + sob.mTemp[5].ToString() + "\t" + sob.mTemp[6].ToString() + "\t" + sob.mTemp[7].ToString() +
                                "\t" + sob.mTemp[8].ToString() + "\t" + sob.mTemp[9].ToString() + "\t" + sob.mTemp[10].ToString() + "\t" + sob.mTemp[11].ToString();
                                await writer.WriteLineAsync(Sob);
                            }
                        }
                    }



                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message + "\n" + ex.ToString());
                await messageDialog.ShowAsync();
            }
        }
        public async void saveTempN(StorageFolder folder)
        {
            try
            {


                if (folder != null)
                {




                    if (classTempsN.Count != 0)
                    {
                        using (StreamWriter writer =
                       new StreamWriter(await folder.OpenStreamForWriteAsync(
                       name + " " + "TempSobN" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                        {
                            string sSob = "Дата" + "\t" + "D1" + "\t" + "D2" + "\t" + "D3" + "\t" + "D4" + "\t" + "D5" + "\t" + "D6" + "\t" + "D7"
                                + "\t" + "D8" + "\t" + "D9" + "\t" + "D10" + "\t" + "D11" + "\t" + "D12";


                            await writer.WriteLineAsync(sSob);
                            foreach (ClassTemp sob in classTempsN)
                            {

                                string Sob = sob.dateTime.ToString() + "\t" + sob.mTemp[0].ToString() + "\t" + sob.mTemp[1].ToString() + "\t" +
                                sob.mTemp[2].ToString() + "\t" + sob.mTemp[3].ToString() + "\t" + sob.mTemp[4].ToString() + "\t" + sob.mTemp[5].ToString() + "\t" + sob.mTemp[6].ToString() + "\t" + sob.mTemp[7].ToString() +
                                "\t" + sob.mTemp[8].ToString() + "\t" + sob.mTemp[9].ToString() + "\t" + sob.mTemp[10].ToString() + "\t" + sob.mTemp[11].ToString();
                                await writer.WriteLineAsync(Sob);
                            }
                        }
                    }



                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message + "\n" + ex.ToString());
                await messageDialog.ShowAsync();
            }

        }
        public async void saveTempN6(StorageFolder folder)
        {
            try
            {


                if (folder != null)
                {

                    List<ClassTemp> lines = new List<ClassTemp>();
                    int x = 1;
                    double[] mas = new double[12];
                    foreach (ClassTemp d in classTempsN)
                    {
                        x++;
                        for (int i = 0; i < 12; i++)
                        {
                            mas[i] += d.mTemp[i];
                        }
                        if (x == 7)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                mas[i] = mas[i] / 6;
                            }
                            x = 1;
                            lines.Add(new ClassTemp() { dateTime = d.dateTime.AddHours(-5), mTemp = mas });
                            mas = new double[12];
                        }
                    }
                  
                    if (x != 1)
                    {
                        for (int i = 0; i < 12; i++)
                        {
                            mas[i] = mas[i] / (x - 1);
                        }
                      
                        lines.Add(new ClassTemp() { dateTime = lines.ElementAt(lines.Count() - 1).dateTime.AddHours(5), mTemp = mas });
                    }
                    if (lines.Count != 0)
                    {
                        using (StreamWriter writer =
                       new StreamWriter(await folder.OpenStreamForWriteAsync(
                       name + " " + "TempSobN6" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                        {
                            string sSob = "Дата" + "\t" + "D1" + "\t" + "D2" + "\t" + "D3" + "\t" + "D4" + "\t" + "D5" + "\t" + "D6" + "\t" + "D7"
                                + "\t" + "D8" + "\t" + "D9" + "\t" + "D10" + "\t" + "D11" + "\t" + "D12";


                            await writer.WriteLineAsync(sSob);
                            foreach (ClassTemp sob in lines)
                            {

                                string Sob = sob.dateTime.ToString() + "\t" + sob.mTemp[0].ToString() + "\t" + sob.mTemp[1].ToString() + "\t" +
                                sob.mTemp[2].ToString() + "\t" + sob.mTemp[3].ToString() + "\t" + sob.mTemp[4].ToString() + "\t" + sob.mTemp[5].ToString() + "\t" + sob.mTemp[6].ToString() + "\t" + sob.mTemp[7].ToString() +
                                "\t" + sob.mTemp[8].ToString() + "\t" + sob.mTemp[9].ToString() + "\t" + sob.mTemp[10].ToString() + "\t" + sob.mTemp[11].ToString();
                                await writer.WriteLineAsync(Sob);
                            }
                        }
                    }



                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message + "\n" + ex.ToString());
                await messageDialog.ShowAsync();
            }
        }
        public async void ZamenaPic(string textNaZamenu1, StorageFile file)
        {

            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    string textToFind = textNaZamenu1;

                    if (textToFind != null)
                    {

                        RichEditBox richEditBox = new RichEditBox();
                        richEditBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, stringRich);
                        ITextRange searchRange = richEditBox.Document.GetRange(0, 0);
                        if (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.Word) > 0)
                        {

                           await new MessageDialog(searchRange.EndPosition.ToString()).ShowAsync();
                            // searchRange.SetText(TextSetOptions.FormatRtf, zamena);
                            BitmapImage image = new BitmapImage();

                            await image.SetSourceAsync(fileStream);
                            searchRange.InsertImage(100, 100, 0, VerticalCharacterAlignment.Baseline, "img", fileStream);

                        }
                        string ss = String.Empty;
                        richEditBox.TextDocument.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out ss);
                        stringRich = ss;




                    }

                }
            }
         


        }
        public List<ClassTemp> Rabota(List<ClassTemp> classTemps)
        {
            List<ClassTemp> mas = new List<ClassTemp>();
            foreach(var d in classTemps)
            {
                if(d.colSob>0)
                {
                    mas.Add(new ClassTemp() { colSob = 1, dateTime = d.dateTime });
                }
                else
                {
                    mas.Add(new ClassTemp() { colSob = 0, dateTime = d.dateTime });
                }
            }
            return mas;
        }

    }
}
