using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Text;
using Windows.UI.Xaml.Documents;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Text;
using System.Text.RegularExpressions;
using Windows.UI;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using System.Drawing;
using Windows.Graphics.Display;
using Windows.Storage.Pickers;
using Windows.Graphics.Imaging;
using Windows.System.Threading;
using System.Threading;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;






// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace DataYRAN.Pasport
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPagePasport : Page
    {
        public BlankPagePasport()
        {
            this.InitializeComponent(); 
        }
        ObservableCollection<ClassSob> classSobs1 = new ObservableCollection<ClassSob>();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {


            if (e.Parameter is ObservableCollection<ClassSob>)
            {

                try
                {
                    
                    classPasports.Clear();
                    classSobs1 = e.Parameter as ObservableCollection<ClassSob>;
                   // PasportBild(e.Parameter as ObservableCollection<ClassSob>);

                }
                catch (Exception ex)
                {
                    MessageDialog messageDialog = new MessageDialog(ex.ToString());
                   // messageDialog.ShowAsync();
                }

            }
            else
            {

            }

            base.OnNavigatedTo(e);

         

            mDney[0] = 31;
            mDney[1] = 29;
            mDney[2] = 31;
            mDney[3] = 30;
            mDney[4] = 31;
            mDney[5] = 30;
            mDney[6] = 31;
            mDney[7] = 31;
            mDney[8] = 30;
            mDney[9] = 31;
            mDney[10] = 30;
            mDney[11] = 31;
            colors[0] = Colors.Red;
            colors[1] = Colors.Green;
            colors[2] = Colors.Black;
            colors[3] = Colors.Blue;
            colors[4] = Colors.Yellow;
            colors[5] = Colors.Aqua;
            colors[6] = Colors.Thistle;
            colors[7] = Colors.RosyBrown;
            colors[8] = Colors.DarkOliveGreen;
            colors[9] = Colors.DarkBlue;
            colors[10] = Colors.Gray;
            colors[11] = Colors.DarkGoldenrod;
        }
      public  ObservableCollection<ClassPasport> classPasports = new ObservableCollection<ClassPasport>();
        public async void PasportBild(ObservableCollection<ClassSob> classSobs)
        {
            List<string> mecdata = new List<string>();
            List<int> nameKl = new List<int>();
          
            if (classSobs.Count > 0)
            {
                var nameK = from nn in classSobs orderby nn.nameklaster select Convert.ToInt32(nn.nameklaster);//Определяем какие есть кластера

                var orderedNumbers = from ClassSob in classSobs orderby ClassSob.dateUR.DateTimeString() select ClassSob;//Сортируем события по дате
          
                DateTime dateTime = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, 0, 0, 0, 0);//Самая первая дата в событиях
            
                DateTime dateTimeFirst = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, 0, 0, 0, 0);//Самая первая дата в событиях

                DateTime dateTime1 = dateTime; //new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
         
                DateTime dateTimeEnd = new DateTime(orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.GG, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.MM, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.DD, 0, 0, 0, 0);//Самая последняя дата в событиях

                int max = nameK.Max();
                int min = nameK.Min();
                for (int i = min; i <= max; i++)
                {
                    var k = from f in nameK where f == i select f;
                    if (k.Count() > 0)
                    {     
                            nameKl.Add(k.First());
                    }

                }
      
                for (DateTime i= dateTimeFirst; i.Month<= dateTimeEnd.Month; i= i.AddMonths(1))
                {
                    if (i.Year > dateTimeFirst.Year)
                    {
                        break;
                    }


                     
                    
                        var k = from f in orderedNumbers where f.dateUR.GG == i.Year && f.dateUR.MM == i.Month select f;
                  
                        if (k.Count() > 0)
                        {
                            mecdata.Add(k.First().dateUR.MM.ToString());//определяем какие есть месяца для паспорта
                        }
                    
                }

                
                try
                {
                    foreach (string t in mecdata)
                    {
                        ClassPasport classPasportObc = new ClassPasport()
                        {
                            name = "Приложение к spravke " + " " + t

                        };
                        classPasports.Add(classPasportObc);
                    
                        classPasportObc.stringRich = ClassШаблон.ШаблонПриложения;
                        foreach (int kl in nameKl)//пройдемся по кластерам
                                {
                           
                            var d = from dd in classSobs where dd.nameklaster == kl.ToString() && dd.dateUR.MM.ToString() == t select dd;
                            if (d.Count() > 0)
                            {

                                ClassPasport classPasport = new ClassPasport()
                                {
                                    name = "Паспорт кластера " + kl.ToString() + " " + d.First().dateUR.MM.ToString() + "." + d.First().dateUR.GG.ToString(),
                                    classSobs = d.ToList<ClassSob>(),
                                };
                               
                                await editor3.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                                    editor3.Text = "Распределение амплитуд"; });
                                classPasport.classRasAmp= AmpRas(d.ToList());
                                
                                await editor3.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
                                    editor3.Text = "сигма пьедестала"; });

                                classPasport.classSig = await SigLine(d.ToList(), 1);

                                classPasport.mas= sredSig(classPasport.classSig);
                                // var ll = classPasport.classSig6();
                                await editor3.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => 
                                { editor3.Text = "Пьедестал"; });

                                classPasport.classNullLine = await NullLinevoid(d.ToList(), 1);
                                classPasport.masSredNull = sredNull(classPasport.classNullLine);
                                await editor3.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                { editor3.Text = "Темп события"; });
                                classPasport.classTemps = await TempSob(d.ToList(), 1);
                                classPasport.classTempsNoNorm = await TempSobNoNormirovka(d.ToList(), 1);
                                classPasport.masSredTemp = sredTemp(classPasport.classTemps);
                                await editor3.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                { editor3.Text = "Темп нейтрона"; });
                                classPasport.classTempsN= await TempNeutron(d.ToList(), 1);
                                classPasport.masSredTempN = sredTempN(classPasport.classTempsN);
                                
                                    classPasport.stringRich = ClassШаблон.ШаблонПаспорт;
                                    var hh = classPasport.Rabota(classPasport.classTemps);
                                            var g = from j in hh where j.colSob > 0 select j;
                                            classPasport.stringRich = Zamenatext("textH", g.Count().ToString(), classPasport.stringRich);
                                            int pro = (int)((double)g.Count() /(double)(((double)mDney[d.First().dateUR.MM-1]*24) / 100.0));
                                            classPasport.stringRich = Zamenatext("textP", pro.ToString(), classPasport.stringRich);

                                            DateTime dateTime2 = new DateTime(d.First().dateUR.GG, d.First().dateUR.MM, d.First().dateUR.DD);
                                            classPasport.stringRich = Zamenatext("Заголовок", "Паспорт установки УРАН кластер №"+ kl.ToString()+
                                              " за "+ dateTime2.Date.ToString("MMMM yyyy")+ " года.", classPasport.stringRich);

                                      
                                            StorageFile storageFile1 = await creatPicRasAmp(classPasport.classRasAmp, g.Count());
                                            classPasport.stringRich = await ZamenaPic("Pic1", classPasport.stringRich, storageFile1);
                                           // classPasport.image = await GetPic(storageFile1);
                                            classPasportObc.stringRich = await ZamenaPicObc("Pic3_"+ kl.ToString(), classPasportObc.stringRich, storageFile1);

                                            string saa = "кластера №" + kl.ToString();
                                            classPasport.stringRich =  Zamenatext("KL", saa, classPasport.stringRich);
                                            for (int i = 0; i < 12; i++)
                                            {
                                                classPasport.stringRich =  Zamenatext("Tabsig"+(i+1).ToString(), classPasport.mas[i].ToString("0.00"), classPasport.stringRich);
                                                classPasportObc.stringRich= Zamenatext("Tab2-" + kl.ToString()+"_"+ (i + 1).ToString(), classPasport.mas[i].ToString("0.00"), classPasportObc.stringRich);
                                            }

                                     
                                            for (int i = 0; i < 12; i++)
                                            {
                                                classPasport.stringRich =  Zamenatext("Tabnull" + (i + 1).ToString(), classPasport.masSredNull[i].ToString("0000"), classPasport.stringRich);
                                    classPasportObc.stringRich = Zamenatext("Tab1-" + kl.ToString() + "_" + (i + 1).ToString(), classPasport.masSredNull[i].ToString("0000"), classPasportObc.stringRich);

                                            }

                                          
                                            for (int i = 0; i < 12; i++)
                                            {
                                                classPasport.stringRich = Zamenatext("TabT" + (i + 1).ToString(), classPasport.masSredTemp[i].ToString("0.00"), classPasport.stringRich);
                                    classPasportObc.stringRich = Zamenatext("Tab3-" + kl.ToString() + "_" + (i + 1).ToString(), classPasport.masSredTemp[i].ToString("0.00"), classPasportObc.stringRich);
                                }

                                           
                                            for (int i = 0; i < 12; i++)
                                            {
                                                classPasport.stringRich =  Zamenatext("TabTN" + (i + 1).ToString(), classPasport.masSredTempN[i].ToString("0.00"), classPasport.stringRich);
                                    classPasportObc.stringRich = Zamenatext("Tab4-" + kl.ToString() + "_" + (i + 1).ToString(), classPasport.masSredTempN[i].ToString("0.00"), classPasportObc.stringRich);
                                }


                                            StorageFile storageFile2 = await creatPicSig(await SigLine(d.ToList(), 6));
                                         
                                            classPasport.stringRich = await ZamenaPic("P2ic", classPasport.stringRich, storageFile2);
                                            classPasportObc.stringRich = await ZamenaPicObc("Pic2_" + kl.ToString(), classPasportObc.stringRich, storageFile2);


                                            StorageFile storageFile3 = await creatPicNullLine(await NullLinevoid(d.ToList(), 6));
                                          
                                            classPasport.stringRich = await ZamenaPic("Pic3", classPasport.stringRich, storageFile3);
                                            classPasportObc.stringRich = await ZamenaPicObc("Pic1_" + kl.ToString(), classPasportObc.stringRich, storageFile3);

                                            StorageFile storageFile4 = await creatPicTempSob(await TempSob(d.ToList(), 24));

                                            classPasport.stringRich = await ZamenaPic("Pic4", classPasport.stringRich, storageFile4);
                                            classPasportObc.stringRich = await ZamenaPicObc("Pic4_" + kl.ToString(), classPasportObc.stringRich, storageFile4);

                                StorageFile storageFile44 = await creatPicTempSobNoNormirovka(await TempSobNoNormirovka(d.ToList(), 1));

                                //classPasport.stringRich = await ZamenaPic("Pic4", classPasport.stringRich, storageFile4);
                                classPasportObc.stringRich = await ZamenaPicObc("Pic5_" + kl.ToString(), classPasportObc.stringRich, storageFile44);

                                StorageFile storageFile5 = await creatPicTempSobN(await TempNeutron(d.ToList(), 24));

                                            classPasport.stringRich = await ZamenaPic("Pic5", classPasport.stringRich, storageFile5);
                                            classPasportObc.stringRich = await ZamenaPicObc("Pic6_" + kl.ToString(), classPasportObc.stringRich, storageFile5);

                                            classPasports.Add(classPasport);
                                           
                            }
                        }
                        
                    }
                    grid.Visibility = Visibility.Collapsed;

                }
                catch (Exception ex)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = ex.Message,
                        Content = ex.ToString(),
                        PrimaryButtonText = "Ok"
                    };

                    await errorDialog.ShowAsync();
                }
            }
         
        }
        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a text file.
            Windows.Storage.Pickers.FileOpenPicker open =
                new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add("*");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                try
                {
                    Windows.Storage.Streams.IRandomAccessStream randAccStream =
                await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    // Load the file into the Document property of the RichEditBox.
                    editor.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                    editor.TextDocument.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out string s);
                    // editor2.Text = s;

                    classPasports.Add(new ClassPasport() { name = file.DisplayName, stringRich=s });

                }
                catch (Exception)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "File open error",
                        Content = "Sorry, I couldn't open the file.",
                        PrimaryButtonText = "Ok"
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                editor.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);

                // Let Windows know that we're finished changing the file so the
                // other app can update the remote version of the file.
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status != Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Windows.UI.Popups.MessageDialog errorBox =
                    new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
            }
        }

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            
            if (selectedText != null)
            {
                 Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                 charFormatting.Italic = Windows.UI.Text.FormatEffect.Toggle;
                 selectedText.CharacterFormat = charFormatting;
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editor.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.Single;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileOpenPicker open = new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".png");
            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))

                {
                    BitmapImage image = new BitmapImage();
                    await image.SetSourceAsync(fileStream);
                    editor.Document.Selection.InsertImage(image.PixelWidth, image.PixelHeight, 0, VerticalCharacterAlignment.Baseline, "img", fileStream);
                }
            }
        }
        private async void GetImage(object sender, RoutedEventArgs e)
        {
            string rtf = "";
            editor.Document.Selection.GetText(TextGetOptions.FormatRtf, out rtf);
            string imageDataHex = "";
            var r = new Regex(@"pict[\s\S]+?[\r\n](?<imagedata>[\s\S]+)[\r\n]\}\\par", RegexOptions.None);
            var m = r.Match(rtf);
            if (m.Success)
            {
                imageDataHex = m.Groups["imagedata"].Value;
            }
            byte[] imageBuffer = ToBinary(imageDataHex);
            StorageFile tempfile = await ApplicationData.Current.LocalFolder.CreateFileAsync("temppic.png", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBufferAsync(tempfile, imageBuffer.AsBuffer());
        }

        public static byte[] ToBinary(string imageDataHex)
        {
            //this function taken entirely from:
           
            if (imageDataHex == null)
            {
                throw new ArgumentNullException("imageDataHex");
            }

            int hexDigits = imageDataHex.Length;
            int dataSize = hexDigits / 2;
            byte[] imageDataBinary = new byte[dataSize];

            StringBuilder hex = new StringBuilder(2);

            int dataPos = 0;
            for (int i = 0; i < hexDigits; i++)
            {
                char c = imageDataHex[i];
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                hex.Append(imageDataHex[i]);
                if (hex.Length == 2)
                {
                    imageDataBinary[dataPos] = byte.Parse(hex.ToString(), System.Globalization.NumberStyles.HexNumber);
                    dataPos++;
                    hex.Remove(0, 2);
                }
            }
            return imageDataBinary;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = (ListView)sender;
            ClassPasport classPasport = (ClassPasport)listView.SelectedItem;
           // classPasport.stringRich1.TextDocument.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out string s);
            editor.Document.SetText(TextSetOptions.FormatRtf, classPasport.stringRich);
        }
        public void Zamenatext(string textNaZamenu1, string zamena )
        {
          
            string textToFind = textNaZamenu1;
            
            if (textToFind != null)
            {

             ITextRange searchRange = editor.Document.GetRange(0, 0);
                int x = 0;
             while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.None) > 0)
             {

                    searchRange.SetText(TextSetOptions.ApplyRtfDocumentDefaults, zamena);
              
                    x++;

             }

            }
        }
        public  string Zamenatext(string textNaZamenu1, string zamena, string stringEditBox)
        {

            string textToFind = textNaZamenu1;

           // stringRich1.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
           // stringRich1.TextDocument.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out string s2);
            RichEditBox richEditBox = new RichEditBox();
            richEditBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, stringEditBox);
            if (textToFind != null)
            {
              
                ITextRange searchRange = richEditBox.Document.GetRange(0, 0);
                while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.Word) > 0)
                {

                    searchRange.SetText(TextSetOptions.FormatRtf, zamena);
                 
                
                }
              

            }
            string ss = String.Empty;
            richEditBox.TextDocument.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out ss);
            return ss;
        }
        public async Task<string> ZamenaPic(string textNaZamenu1, string stringEditBox, StorageFile file)
        {
            RichEditBox richEditBox = new RichEditBox();
            richEditBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, stringEditBox);
            if (file != null)
            {
              
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))

                {
                    string textToFind = textNaZamenu1;
                    
                    if (textToFind != null)
                    {
                       

                        ITextRange searchRange = richEditBox.Document.GetRange(0, 0);
                       if(searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.Word)>0)
                        {

                           // await new MessageDialog(searchRange.EndPosition.ToString()).ShowAsync();
                            // searchRange.SetText(TextSetOptions.FormatRtf, zamena);
                            BitmapImage image = new BitmapImage();

                            await image.SetSourceAsync(fileStream);
                            
                            searchRange.InsertImage(image.PixelWidth/3, image.PixelHeight/3, 0, VerticalCharacterAlignment.Baseline, "img", fileStream);

                        }


                        


                    }
                  
                }
            }
            string ss = String.Empty;
            richEditBox.TextDocument.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out ss);
            return ss;


        }
        public async Task<string> ZamenaPicObc(string textNaZamenu1, string stringEditBox, StorageFile file)
        {
            RichEditBox richEditBox = new RichEditBox();
            richEditBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, stringEditBox);
            if (file != null)
            {

                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))

                {
                    string textToFind = textNaZamenu1;

                    if (textToFind != null)
                    {


                        ITextRange searchRange = richEditBox.Document.GetRange(0, 0);
                        if (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.Word) > 0)
                        {

                            // await new MessageDialog(searchRange.EndPosition.ToString()).ShowAsync();
                            // searchRange.SetText(TextSetOptions.FormatRtf, zamena);
                            BitmapImage image = new BitmapImage();

                            await image.SetSourceAsync(fileStream);
                            searchRange.InsertImage(317, 198, 0, VerticalCharacterAlignment.Baseline, "img", fileStream);

                        }





                    }

                }
            }
            string ss = String.Empty;
            richEditBox.TextDocument.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out ss);
            return ss;


        }
        public async Task<BitmapImage> GetPic(StorageFile file)
        {
            BitmapImage image = new BitmapImage();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {                 
                        await image.SetSourceAsync(fileStream);    
                }
            }
            return image;


        }
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
           
          
        }
        public class Data
        {
            public double Category { get; set; }

            public double Value { get; set; }
        }
        public void dd()
        {
            Thread.Sleep(3000);
           
        }
        int[] mDney = new int[12];
        public class CustomPalettes
        {
            static CustomPalettes()
            {
                CreateCustomDarkPalette();
            }
           static public SolidColorBrush[] Color1 = new SolidColorBrush[12];

            
            private static void CreateCustomDarkPalette()
            {
               // ChartPalette palette = new ChartPalette() { Name = "CustomDark" };

                // fill
               // palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 40, 152, 228)));//1
                Color1[0] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 40, 152, 228));
              //  palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 205, 0)));//2
                Color1[1] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 205, 0));
              //  palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 60, 0)));//3
                Color1[2] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 60, 0));
              //  palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 210, 202, 202)));//4
                Color1[3] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 210, 202, 202));
               // palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 67, 67, 67)));//5
                Color1[4] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 67, 67, 67));
              //  palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 31, 234, 149)));//6
                Color1[5] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 31, 234, 149));
               // palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 109, 49, 255)));//7
                Color1[6] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 109, 49, 255));
              //  palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 178, 161)));//8
                Color1[7] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 178, 161));
              //  palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 109, 255, 0)));//9
                Color1[8] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 109, 255, 0));
              //  palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 128, 0)));//10
                Color1[9] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 128, 0));
              //  palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 109, 0, 0)));//11
                Color1[10] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 109, 0, 0));
               // palette.FillEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 205, 149, 117)));//12
                Color1[11] = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 205, 149, 117));
             
               

                // stroke
              /*  palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
                palette.StrokeEntries.Brushes.Add(new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0)));
               */
               // CustomDark = palette;
            }

            //public static ChartPalette CustomDark { get; private set; }

        }
        public async Task<StorageFile>  creatPicRasAmp(List<RasAmp> rasAmp, int h )
        {
            //radChart.Series.Clear();
          
           // var df = radChart.VerticalAxis.TitleTemplate;
           // var dH = radChart.HorizontalAxis.TitleTemplate;
           // var ST = radChart.VerticalAxis.LineStyle;
           // var LS = radChart.VerticalAxis.LabelStyle;
           // var LT = radChart.VerticalAxis.MajorTickStyle;
           // Telerik.UI.Xaml.Controls.Chart.LogarithmicAxis logarithmicAxisVertical = new Telerik.UI.Xaml.Controls.Chart.LogarithmicAxis() { TitleTemplate=df, ExponentStep=0.1, LineStyle=ST, LabelStyle=LS, MajorTickStyle=LT};
           // Telerik.UI.Xaml.Controls.Chart.LogarithmicAxis logarithmicAxis1 = new Telerik.UI.Xaml.Controls.Chart.LogarithmicAxis() { TitleTemplate = dH, Maximum=100, ExponentStep=0.1, LineStyle = ST, LabelStyle = LS, MajorTickStyle = LT };
           // radChart.VerticalAxis = logarithmicAxisVertical;
           // radChart.HorizontalAxis = logarithmicAxis1;
          //  radChart.VerticalAxis.Title = "Lg(N)";
          //  radChart.HorizontalAxis.Title = "Lg(A), Код АЦП";
            _MainViewModel = new MainViewModel("1", rasAmp, h);
            oxi.Model = _MainViewModel.MyModel;
           
       
       
           await Task.Run(()=> { Thread.Sleep(2000); });
          
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(grid);
            var pixelBuffer = await rtb.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();
              var displayInformation = DisplayInformation.GetForCurrentView();
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("testImage" + ".png", CreationCollisionOption.ReplaceExisting);
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                         BitmapAlphaMode.Premultiplied,
                                         (uint)rtb.PixelWidth,
                                         (uint)rtb.PixelHeight,
                                         displayInformation.RawDpiX,
                                         displayInformation.RawDpiY,
                                         pixels);
                    await encoder.FlushAsync();
                }
            return file;
        }
        public class DataSig
        {
            public string DateTime { get; set; }

            public double Value { get; set; }
        }
        public class DataSig2
        {
            public DateTime DateTime { get; set; }

            public double Value { get; set; }
        }
        public async Task<StorageFile> creatPicSig(List<ClassRasSig> SigM)
        {
           // radChart.Series.Clear();
          //  int d = 1;
            //if (SigM.Count() > 7)
            {
          //      d = SigM.Count() / 5;
            }
         //   var df = radChart.VerticalAxis.TitleTemplate;
         //   var dH = radChart.HorizontalAxis.TitleTemplate;
         //   var ST = radChart.VerticalAxis.LineStyle;
         //   var LS = radChart.VerticalAxis.LabelStyle;
         //   var LT = radChart.VerticalAxis.MajorTickStyle;

            _MainViewModel = new MainViewModel(SigM);
            oxi.Model = _MainViewModel.MyModel;
            //radChart.VerticalAxis = new Telerik.UI.Xaml.Controls.Chart.LinearAxis() { Maximum = 1.5, LabelStyle=LS, LineStyle=ST, MajorTickStyle=LT, Background=new SolidColorBrush(Colors.Black), MajorTickOffset=1, Minimum=0, Foreground=new SolidColorBrush(Colors.Black), TitleTemplate=df  };
            //radChart.HorizontalAxis =  new DateTimeCategoricalAxis() { MajorTickInterval= d, MajorTickStyle = LT, LabelStyle = LS, LineStyle = ST, LabelFormat = "{0,0:dd.MM.yy}", DateTimeComponent=Telerik.Charting.DateTimeComponent.Hour, GapLength =0.5,  Foreground = new SolidColorBrush(Colors.Black), TitleTemplate=dH };



           // await new MessageDialog("hf").ShowAsync();
            // radChart.VerticalAxis.Title = "Сигма";

            // radChart.HorizontalAxis.Title = "Дата";
            // radChart.Palette = CustomPalettes.CustomDark;
            // grid.Children.Add(textBlock);

          //  List<DataSig2> datas;

          //  for (int i = 0; i < 12; i++)
            {
              //  datas = new List<DataSig2>();
              //  foreach (var dq in SigM)
                {
                 //   double v = (double)dq.mNullLine[i];
                 //   if ((double)dq.mNullLine[i] >= 1.5)
                    {
                  //      v = 1.5;
                    }
                  //  if ((double)dq.mNullLine[i] <= 0.0)
                    {
                  //      v = -1;
                    }
                    //datas.Add(new DataSig() { DateTime = dq.dateTime.ToString("dd.MM.yyyy HH:mm"), Value = v });
                  //  datas.Add(new DataSig2() { DateTime = dq.dateTime, Value = v });

                }
              //  this.radChart.Series.Add(new PointSeries { ItemsSource = datas, ValueBinding = new PropertyNameDataPointBinding("Value"),  PaletteMode = SeriesPaletteMode.Series, CompositeMode = ElementCompositeMode.SourceOver,  CategoryBinding = new PropertyNameDataPointBinding("DateTime"), LegendTitle = "D" + (i + 1).ToString() });
                //  radChart.Series.Add(new LineSeries() { LegendTitle = "D" + (i + 1).ToString(), CompositeMode = ElementCompositeMode.SourceOver, ValueBinding = new PropertyNameDataPointBinding("Value"), CategoryBinding = new PropertyNameDataPointBinding("DateTime"), ItemsSource = datas, Stroke = new SolidColorBrush(colors[i]), Foreground = new SolidColorBrush(colors[i]) });

            }
             await Task.Run(() => { Thread.Sleep(2000); });
            // await new MessageDialog("aaa").ShowAsync();
            RenderTargetBitmap rtb = new RenderTargetBitmap();
             await rtb.RenderAsync(grid);
             var pixelBuffer = await rtb.GetPixelsAsync();
             var pixels = pixelBuffer.ToArray();
             var displayInformation = DisplayInformation.GetForCurrentView();
             
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("testImage2" + ".png", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Premultiplied,
                                     (uint)rtb.PixelWidth,
                                     (uint)rtb.PixelHeight,
                                     displayInformation.RawDpiX,
                                     displayInformation.RawDpiY,
                                     pixels);
                await encoder.FlushAsync();
            }
            return file;
        }
        public async Task<StorageFile> creatPicNullLine(List<NullLine> SigM)
        {
         //   radChart.Series.Clear();
          //  int d = 1;
           // if(SigM.Count()>7)
            {
            //    d = SigM.Count() / 5;
            }
           // var df = radChart.VerticalAxis.TitleTemplate;
           // var dH = radChart.HorizontalAxis.TitleTemplate;
          //  var ST = radChart.VerticalAxis.LineStyle;
          //  var LS = radChart.VerticalAxis.LabelStyle;
          //  var LT = radChart.VerticalAxis.MajorTickStyle;

            // radChart.VerticalAxis = new Telerik.UI.Xaml.Controls.Chart.LinearAxis() { Maximum = 2055, LineStyle=ST, MajorTickStyle = LT, LabelStyle = LS, Minimum =2030, Foreground = new SolidColorBrush(Colors.Black), TitleTemplate= df };
            // radChart.HorizontalAxis = new DateTimeCategoricalAxis() {LineStyle=ST, MajorTickStyle = LT, LabelStyle = LS, MajorTickInterval = d, DateTimeComponent = Telerik.Charting.DateTimeComponent.Hour, LabelFormat = "{0,0:dd.MM.yy}", Foreground = new SolidColorBrush(Colors.Black), TitleTemplate=dH };
            // radChart.VerticalAxis.Title = "Значение нулевой линиии, Код АЦП";
            // radChart.HorizontalAxis.Title = "Дата";

            //radChart.Palette = CustomPalettes.CustomDark;
            // grid.Children.Add(textBlock);
            _MainViewModel = new MainViewModel(SigM);
            oxi.Model = _MainViewModel.MyModel;
          //  List<DataSig2> datas;

           // for (int i = 0; i < 12; i++)
            {
              //  datas = new List<DataSig2>();
              //  foreach (var dq in SigM)
                {
                 //   double v = (double)dq.mNullLine[i];
                  //  if ((double)dq.mNullLine[i] >= 2055)
                    {
                   //     v = 2055;
                    }
                  //  if ((double)dq.mNullLine[i] <= 2030)
                    {
                   //     v = 2030;
                    }
                   // if ((double)dq.mNullLine[i] == 0)
                    {
                    //    v = -1.0;
                    }
                   // datas.Add(new DataSig2() { DateTime = dq.dateTime, Value = v });

                }
              //  this.radChart.Series.Add(new PointSeries { ItemsSource = datas, ValueBinding = new PropertyNameDataPointBinding("Value"), PaletteMode = SeriesPaletteMode.Series, CompositeMode = ElementCompositeMode.SourceOver, CategoryBinding = new PropertyNameDataPointBinding("DateTime"), LegendTitle = "D" + (i + 1).ToString() });

            }
            await Task.Run(() => { Thread.Sleep(2000); });
          
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(grid);
            var pixelBuffer = await rtb.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();
            var displayInformation = DisplayInformation.GetForCurrentView();

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("testImageNullLine" + ".png", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Premultiplied,
                                     (uint)rtb.PixelWidth,
                                     (uint)rtb.PixelHeight,
                                     displayInformation.RawDpiX,
                                     displayInformation.RawDpiY,
                                     pixels);
                await encoder.FlushAsync();
            }
            return file;
        }
        public async Task<StorageFile> creatPicTempSob(List<ClassTemp> SigM)
        {
          
            _MainViewModel = new MainViewModel(SigM);
            oxi.Model = _MainViewModel.MyModel;
       

            await Task.Run(() => { Thread.Sleep(2000); });
            //  await new MessageDialog("aaa").ShowAsync();
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(grid);
            var pixelBuffer = await rtb.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();
            var displayInformation = DisplayInformation.GetForCurrentView();

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("testImageTempSob" + ".png", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Premultiplied,
                                     (uint)rtb.PixelWidth,
                                     (uint)rtb.PixelHeight,
                                     displayInformation.RawDpiX,
                                     displayInformation.RawDpiY,
                                     pixels);
                await encoder.FlushAsync();
            }
            return file;
        }
        public async Task<StorageFile> creatPicTempSobNoNormirovka(List<ClassTemp> SigM)
        {
            
            _MainViewModel = new MainViewModel(SigM, 1);
            oxi.Model = _MainViewModel.MyModel;


            await Task.Run(() => { Thread.Sleep(2000); });
    
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(grid);
            var pixelBuffer = await rtb.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();
            var displayInformation = DisplayInformation.GetForCurrentView();

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("testImageTempSob" + ".png", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Premultiplied,
                                     (uint)rtb.PixelWidth,
                                     (uint)rtb.PixelHeight,
                                     displayInformation.RawDpiX,
                                     displayInformation.RawDpiY,
                                     pixels);
                await encoder.FlushAsync();
            }
            return file;
        }
        Windows.UI.Color[] colors = new Windows.UI.Color[12];
        public async Task<StorageFile> creatPicTempSobN(List<ClassTemp> SigM)
        {
           
            _MainViewModel = new MainViewModel(SigM, "d");
            oxi.Model = _MainViewModel.MyModel;
            await Task.Run(() => { Thread.Sleep(2000); });
            //  await new MessageDialog("aaa").ShowAsync();
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(grid);
            var pixelBuffer = await rtb.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();
            var displayInformation = DisplayInformation.GetForCurrentView();

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("testImageTempSobN" + ".png", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Premultiplied,
                                     (uint)rtb.PixelWidth,
                                     (uint)rtb.PixelHeight,
                                     displayInformation.RawDpiX,
                                     displayInformation.RawDpiY,
                                     pixels);
                await encoder.FlushAsync();
            }
            return file;
        }

        MainViewModel _MainViewModel;
        public class MainViewModel
        {
            public MainViewModel(string log, List<RasAmp> rasAmp, int time)
            {
                
                   this.MyModel = new PlotModel {PlotAreaBorderColor=OxyColors.Transparent, TitleFontSize = 20, LegendFontSize = 26, LegendPlacement=LegendPlacement.Outside };
                if(log=="1")
                {

                    List<Data> datas;
                MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Bottom, MaximumPadding = 0.1, Maximum=110, MinimumPadding = 0.1, Title="A, код АЦП", AxislineThickness = 2, FontSize = 26, TitleFontSize=26, AxislineStyle = LineStyle.Solid, Minimum=1 });
                  MyModel.Axes.Add(new OxyPlot.Axes.LogarithmicAxis { Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Title= "N", AxislineThickness = 2, FontSize = 26, AxislineStyle = LineStyle.Solid, TitleFontSize = 26, Minimum=0.003, Maximum=10});
                    for (int i = 0; i < 12; i++)
                    {

                        datas = new List<Data>();



                        foreach (var dq in rasAmp)
                        {

                            {

                              //  double vv = 0;
                                if ((double)dq.MAmp[i] == 0)
                                {
                                    //   vv = Math.Log10(0.000001); 

                                }
                                else
                                {
                                    //  vv = Math.Log10((double)dq.MAmp[i]);
                                }


                                //  datas.Add(new Data() { Category = Math.Log10((double)dq.znacRas), Value = vv });
                                datas.Add(new Data() { Category = (double)dq.znacRas, Value = (double)dq.MAmp[i] });


                            }
                        }



                        OxyPlot.Series.LineSeries linearAxis = new OxyPlot.Series.LineSeries() { Title = "D"+(i+1).ToString(),  Color = OxyColor.FromRgb(CustomPalettes.Color1[i].Color.R, CustomPalettes.Color1[i].Color.G, CustomPalettes.Color1[i].Color.B) };

                        foreach (var d in datas)
                        {

                           
                            linearAxis.Points.Add(new DataPoint(d.Category, d.Value/ time));
                        }
                        this.MyModel.Series.Add(linearAxis);
                        // radChart.Series.Add(new ScatterLineSeries {Stroke= CustomPalettes.CustomDark.FillEntries.Brushes.ElementAt(i),  CompositeMode = ElementCompositeMode.SourceOver, YValueBinding = new PropertyNameDataPointBinding("Value"), XValueBinding = new PropertyNameDataPointBinding("Category"), StrokeThickness = 2, ItemsSource = datas, FontSize = 10, LegendTitle = "D"+(i+1).ToString() });

                    }
                  
                }
               
            }
            public MainViewModel(List<ClassRasSig> SigM)
            {

                this.MyModel = new PlotModel {PlotAreaBorderColor=OxyColors.Transparent, LegendPlacement = LegendPlacement.Outside, TitleFontSize = 26, LegendFontSize = 22 };
                var startDate = SigM.ElementAt(0).dateTime;
             
                var endDate = SigM.ElementAt(SigM.Count()-1).dateTime;
                double mag = 1;

              //  double minor = 4;
                
                if (SigM.Count() > 7)
                {
                    mag = 6;
                }
                var minValue = DateTimeAxis.ToDouble(startDate);
                var maxValue = DateTimeAxis.ToDouble(endDate);
                MyModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, MinorStep = 1, StringFormat = "dd.MM.yyyy", Minimum = minValue, Maximum = maxValue, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, Title="Дата", MajorStep = mag});
                MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Maximum=1.5, Minimum=0, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, Title = "Сигма", AxislineStyle = LineStyle.Solid, });

                List<DataSig2> datas;

                for (int i = 0; i < 12; i++)
                {
                    ScatterSeries linearAxis = new ScatterSeries() { Title = "D"+(i+1).ToString(), MarkerType = OxyPlot.MarkerType.Circle, MarkerFill = OxyColor.FromRgb(CustomPalettes.Color1[i].Color.R, CustomPalettes.Color1[i].Color.G, CustomPalettes.Color1[i].Color.B) };
                    datas = new List<DataSig2>();
                    foreach (var dq in SigM)
                    {
                        double v = (double)dq.mNullLine[i];
                        if ((double)dq.mNullLine[i] >= 1.5)
                        {
                            v = 1.5;
                        }
                        if ((double)dq.mNullLine[i] <= 0.0)
                        {
                            v = -1;
                        }
                        
                        datas.Add(new DataSig2() { DateTime = dq.dateTime, Value = v });

                    }
                    foreach (var d in datas)
                    {


                
                        linearAxis.Points.Add(new ScatterPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value));
                    }
                    this.MyModel.Series.Add(linearAxis);
                }

            }
            public MainViewModel(List<NullLine> NullM)
            {

                this.MyModel = new PlotModel { LegendPlacement = LegendPlacement.Outside, PlotAreaBorderColor =OxyColors.Transparent, TitleFontSize = 26, LegendFontSize =26 };
               // var startDate = SigM.ElementAt(0).dateTime.AddDays(-1);
                var startDate = NullM.ElementAt(0).dateTime;
                //var endDate = SigM.ElementAt(SigM.Count() - 1).dateTime.AddDays(1);
                var endDate = NullM.ElementAt(NullM.Count() - 1).dateTime;
                double mag = 1;

               // double minor = 4;

                if (NullM.Count() > 7)
                {
                    mag = 6;
                }
                var minValue = DateTimeAxis.ToDouble(startDate);
                var maxValue = DateTimeAxis.ToDouble(endDate);
                MyModel.Axes.Add(new DateTimeAxis {Title="Дата", MajorStep =mag, MinorStep=1,   StringFormat = "dd.MM.yyyy", Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, });
                MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis {Title="Пьедестал, код АЦП", Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Maximum = 2055, Minimum = 2030, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, });

                List<DataSig2> datas;

                for (int i = 0; i < 12; i++)
                {
                    ScatterSeries linearAxis = new ScatterSeries() { MarkerFill = OxyColor.FromRgb(CustomPalettes.Color1[i].Color.R, CustomPalettes.Color1[i].Color.G, CustomPalettes.Color1[i].Color.B), Title = "D" + (i + 1).ToString(), MarkerType = OxyPlot.MarkerType.Circle };
                    datas = new List<DataSig2>();
                    foreach (var dq in NullM)
                    {
                        double v = (double)dq.mNullLine[i];
                        if((double)dq.mNullLine[i] >= 2055)
                        {
                            v = 2055;
                        }
                        if ((double)dq.mNullLine[i] <= 2030)
                        {
                            v = 2030;
                        }
                        if ((double)dq.mNullLine[i] == 0)
                        {
                            v = -1.0;
                        }

                        datas.Add(new DataSig2() { DateTime = dq.dateTime, Value = v });

                    }
                    foreach (var d in datas)
                    {



                        linearAxis.Points.Add(new ScatterPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value));
                    }
                    this.MyModel.Series.Add(linearAxis);
                }

            }
            public MainViewModel(List<ClassTemp> SobM)
            {

                this.MyModel = new PlotModel { LegendPlacement = LegendPlacement.Outside, PlotAreaBorderColor = OxyColors.Transparent, TitleFontSize = 26, LegendFontSize = 26 };
                var startDate = SobM.ElementAt(0).dateTime;

                var endDate = SobM.ElementAt(SobM.Count() - 1).dateTime;
                double mag = 1;

                //double minor = 4;

                if (SobM.Count() > 7)
                {
                    mag = 6;
                }
                var minValue = DateTimeAxis.ToDouble(startDate);
                var maxValue = DateTimeAxis.ToDouble(endDate);
                MyModel.Axes.Add(new DateTimeAxis {Title="Дата", MajorStep=mag, MinorStep = 1, StringFormat = "dd.MM.yyyy", Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid});
                MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis {Title= "Nch/Ns", Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Maximum = 1, Minimum = 0, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, });

                List<DataSig2> datas;

                for (int i = 0; i < 12; i++)
                {
                    ScatterSeries linearAxis = new ScatterSeries() { MarkerFill = OxyColor.FromRgb(CustomPalettes.Color1[i].Color.R, CustomPalettes.Color1[i].Color.G, CustomPalettes.Color1[i].Color.B), Title = "D" + (i + 1).ToString(), MarkerType = OxyPlot.MarkerType.Circle };
                    datas = new List<DataSig2>();
                    foreach (var dq in SobM)
                    {
                        double v = (double)dq.mTemp[i];
                        if ((double)dq.mTemp[i] >= 10)
                        {
                            v = 10;
                        }
                        if ((double)dq.mTemp[i] == 0)
                        {
                            v = 0.0;
                        }
                        if ((double)dq.mTemp[i] == 0 && dq.colSob == 0)
                        {
                            v = -1.0;
                        }

                        datas.Add(new DataSig2() { DateTime = dq.dateTime, Value = v });

                    }
                    foreach (var d in datas)
                    {



                        linearAxis.Points.Add(new ScatterPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value));
                    }
                    this.MyModel.Series.Add(linearAxis);
                }

            }
            public MainViewModel(List<ClassTemp> SobM, int x)
            {

                this.MyModel = new PlotModel { LegendPlacement = LegendPlacement.Outside, PlotAreaBorderColor = OxyColors.Transparent, TitleFontSize = 26, LegendFontSize = 26 };
                var startDate = SobM.ElementAt(0).dateTime;

                var endDate = SobM.ElementAt(SobM.Count() - 1).dateTime;
                double mag = 1;

                //double minor = 4;

                if (SobM.Count() > 7)
                {
                    mag = 6;
                }
                var minValue = DateTimeAxis.ToDouble(startDate);
                var maxValue = DateTimeAxis.ToDouble(endDate);
                MyModel.Axes.Add(new DateTimeAxis { Title = "Дата", MajorStep = mag, MinorStep = 1, StringFormat = "dd.MM.yyyy", Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid });
                MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Title = "N(Ad>10)/ч", Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Maximum = 20, Minimum = 0, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, });

                List<DataSig2> datas;

                for (int i = 0; i < 12; i++)
                {
                    LineSeries linearAxis = new LineSeries() { MarkerFill = OxyColor.FromRgb(CustomPalettes.Color1[i].Color.R, CustomPalettes.Color1[i].Color.G, CustomPalettes.Color1[i].Color.B), Title = "D" + (i + 1).ToString(), MarkerType = OxyPlot.MarkerType.None }; 
                    //ScatterSeries linearAxis = new ScatterSeries() { MarkerFill = OxyColor.FromRgb(CustomPalettes.Color1[i].Color.R, CustomPalettes.Color1[i].Color.G, CustomPalettes.Color1[i].Color.B), Title = "D" + (i + 1).ToString(), MarkerType = OxyPlot.MarkerType.Circle };
                    datas = new List<DataSig2>();
                    foreach (var dq in SobM)
                    {
                        double v = (double)dq.mTemp[i]* (double)dq.colSob;
                        if ((double)dq.mTemp[i] * dq.colSob >= 20)
                        {
                            v = 20;
                        }
                        if ((double)dq.mTemp[i] == 0)
                        {
                            v = 0.0;
                        }
                        if ((double)dq.mTemp[i] == 0 && dq.colSob == 0)
                        {
                            v = -1.0;
                        }

                        datas.Add(new DataSig2() { DateTime = dq.dateTime, Value = v });

                    }
                    foreach (var d in datas)
                    {


                        linearAxis.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value));
                       // linearAxis.Points.Add(new ScatterPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value));
                    }
                    this.MyModel.Series.Add(linearAxis);
                }

            }
            public MainViewModel(List<ClassTemp> SigM, string g)
            {

                this.MyModel = new PlotModel {PlotAreaBorderColor=OxyColors.Transparent, LegendPlacement = LegendPlacement.Outside, TitleFontSize = 26, LegendFontSize = 26 };
                var startDate = SigM.ElementAt(0).dateTime;

                var endDate = SigM.ElementAt(SigM.Count() - 1).dateTime;
                double mag = 1;

              

                if (SigM.Count() > 7)
                {
                    mag = 6;
                }
                var minValue = DateTimeAxis.ToDouble(startDate);
                var maxValue = DateTimeAxis.ToDouble(endDate);
                MyModel.Axes.Add(new DateTimeAxis {Title="Дата", MajorStep=mag, MinorStep = 1, StringFormat = "dd.MM.yyyy", Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, });
                MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis {Title= "N(nd)/Ns", Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Maximum = 1, Minimum = 0, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, });

                List<DataSig2> datas;

                for (int i = 0; i < 12; i++)
                {
                    ScatterSeries linearAxis = new ScatterSeries() { MarkerFill = OxyColor.FromRgb(CustomPalettes.Color1[i].Color.R, CustomPalettes.Color1[i].Color.G, CustomPalettes.Color1[i].Color.B), Title = "D" + (i + 1).ToString(), MarkerType = OxyPlot.MarkerType.Circle };
                    datas = new List<DataSig2>();
                    foreach (var dq in SigM)
                    {
                        double v = (double)dq.mTemp[i];
                        if ((double)dq.mTemp[i] >= 1.0)
                        {
                            v = 1.0;
                        }
                        if ((double)dq.mTemp[i] == 0)
                        {
                            v = 0.0;
                        }
                        if ((double)dq.mTemp[i] == 0 && dq.colSob == 0)
                        {
                            v = -1.0;
                        }

                        datas.Add(new DataSig2() { DateTime = dq.dateTime, Value = v });

                    }
                    foreach (var d in datas)
                    {



                        linearAxis.Points.Add(new ScatterPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value));
                    }
                    this.MyModel.Series.Add(linearAxis);
                }

            }
            public void CreatLogaritm()
            {
                MyModel.Axes.Add(new OxyPlot.Axes.LogarithmicAxis { Position = AxisPosition.Bottom, MaximumPadding = 0.1, Maximum = 10, MinimumPadding = 0.1, Title="dsfs" });
                MyModel.Axes.Add(new OxyPlot.Axes.LogarithmicAxis { Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Title="xcvxvx" });


            }
            public void AddLineSerias(string name, List<Data> datas)
            {
                OxyPlot.Series.LineSeries linearAxis = new OxyPlot.Series.LineSeries() { Title = name };
                foreach (var d in datas)
                {

                    this.MyModel.Series.Add(linearAxis);
                    linearAxis.Points.Add(new DataPoint(d.Category, d.Value));
                }
                this.MyModel.Series.Add(linearAxis);
            }
            public void CleaSerias()
            {
                this.MyModel.Series.Clear();


            }
            public PlotModel MyModel { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gridtext.Visibility = Visibility.Collapsed;
            PasportBild(classSobs1);
           
            gridtext.Visibility = Visibility.Visible;
        }
        private async void AppBarButton(object sender, RoutedEventArgs e)
        {

            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {


                foreach (var d in classPasports)
                {
                    StorageFile file = await folder.CreateFileAsync(d.name + ".rtf", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                    if (file != null)
                    {

                        Windows.Storage.CachedFileManager.DeferUpdates(file);

                        Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                        RichEditBox richEditBox = new RichEditBox();
                        richEditBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, d.stringRich);
                        richEditBox.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);

                        Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                        if (status != Windows.Storage.Provider.FileUpdateStatus.Complete)
                        {
                            Windows.UI.Popups.MessageDialog errorBox =
                                new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                            await errorBox.ShowAsync();
                        }
                    }

                    d.save(folder);
                }

                MessageDialog messageDialog = new MessageDialog("Статистика нулевых линий сохранена сохранен");
                await messageDialog.ShowAsync();

            }
            else
            {

            }
        }
    }

}
