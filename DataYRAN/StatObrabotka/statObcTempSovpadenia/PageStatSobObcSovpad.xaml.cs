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

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace DataYRAN.StatObrabotka.statObcTempSovpadenia
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PageStatSobObcSovpad : Page
    {
        public PageStatSobObcSovpad()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {


            if (e.Parameter is ObservableCollection<ClassSobColl>)
            {


                ObservableCollection<ClassSobColl> classSobObc = e.Parameter as ObservableCollection<ClassSobColl>;

                await RaspredObcSrab(classSobObc, 1);





            }
            else
            {

            }

            base.OnNavigatedTo(e);
        }
        public async Task RaspredObcSrab(ObservableCollection<ClassSobColl> classSobObc1, int cloc)
        {

            var orderedNumbers = from ClassSob in classSobObc1
                                 orderby ClassSob.dateUR.DateString()
                                 select ClassSob;
            
            DateTime dateTime = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, 0, 0, 0);
            DateTime dateTimeFirst = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, 0, 0, 0);

            DateTime dateTime1 = dateTime; //new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
            DateTime dateTimeEnd = new DateTime(orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.GG, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.MM, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.DD, 0, 0, 0, 0);

            dateTime1 = dateTime1.AddDays(cloc);
           
            for (int i = 0; i <= dateTimeEnd.Subtract(dateTimeFirst).TotalDays; i++)
            {

               int Sob1 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast == 1 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc==true) select sobb).Count();
                int Sob2 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast == 2 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc == true) select sobb).Count();
                int Sob3 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast == 3 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc == true) select sobb).Count();
                int Sob4 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast == 4 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc == true) select sobb).Count();
                int Sob5 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast == 5 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc == true) select sobb).Count();
                int Sob6 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast == 6 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc == true) select sobb).Count();

                int Sob12 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast == 2 && sobb.klSob.Contains("1") && sobb.klSob.Contains("2") && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc == true) select sobb).Count();
                int Sob13 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast ==2 && sobb.klSob.Contains("1") && sobb.klSob.Contains("3") && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc == true) select sobb).Count();
                int Sob23 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast ==2 && sobb.klSob.Contains("2") && sobb.klSob.Contains("3") && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc == true) select sobb).Count();
                int Sob15 = (from sobb in orderedNumbers.AsParallel() where (sobb.SumClast ==2 && sobb.klSob.Contains("1") && sobb.klSob.Contains("5") && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays >= 0 && new DateTime(sobb.dateUR.GG, sobb.dateUR.MM, sobb.dateUR.DD, sobb.dateUR.HH, sobb.dateUR.Min, sobb.dateUR.CC, 0).Subtract(dateTime).TotalDays < cloc == true) select sobb).Count();
        
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    DataColec.Add(new ClassSovpadSob() { dateTime = dateTime, colSob1= Sob1, colSob2= Sob2, colSob3= Sob3, colSob4= Sob4, colSob5= Sob5, colSob6= Sob6, colSob12= Sob12, colSob13= Sob13, colSob15= Sob15, colSob23= Sob23 });
  
});

               
              
               
                dateTime = dateTime1;
                dateTime1 = dateTime1.AddDays(cloc);

            }

            MessageDialog messageDialog = new MessageDialog("Конец обработки общих событий", "Обработка общих события");
            await messageDialog.ShowAsync();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPageObrData));
        }
        ObservableCollection<ClassSovpadSob> _DataColec = new ObservableCollection<ClassSovpadSob>();
        public ObservableCollection<ClassSovpadSob> DataColec
        {
            get
            {
                return _DataColec;
            }
            set
            {
                _DataColec = value;

            }
        }



        private async void AppBarButton(object sender, RoutedEventArgs e)
        {

            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {



                if (DataColec.Count != 0)
                {
                    using (StreamWriter writer =
                   new StreamWriter(await folder.OpenStreamForWriteAsync(
                   "TempSob" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                    {
                        string sSob = "DateTime" + "\t" + "Sob1" + "\t" + "Sob2" + "\t" + "Sob3" + "\t" + "Sob4" + "\t" + "Sob5" + "\t" + "Sob6" + "\t" + "Sob12" + "\t" + "Sob13" + "\t" + "Sob23" + "\t" + "Sob15";


                        await writer.WriteLineAsync(sSob);
                        foreach (ClassSovpadSob sob in DataColec)
                        {

                            string Sob = sob.date() + "\t" + sob.colSob1 + "\t" + sob.colSob2 + "\t" + sob.colSob3 + "\t" + sob.colSob4 + "\t" + sob.colSob5 + "\t" + sob.colSob6 + "\t" + sob.colSob12
                                 + "\t" + sob.colSob13
                                 + "\t" + sob.colSob13
                                 + "\t" + sob.colSob15;
                            await writer.WriteLineAsync(Sob);
                        }
                    }
                }


                MessageDialog messageDialog = new MessageDialog("Cохранен");
                await messageDialog.ShowAsync();

            }
            else
            {

            }
        }
        private void rankLowFilter_Click(object sender, RoutedEventArgs e)
        {
            int x = 100;
          //  EmployeeGrid.ItemsSource = new ObservableCollection<ClassSovpadSob>(from item in DataColec
                                                                             // where item.Temp > x
                                                                              //select item);
        }
        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeGrid != null)
            {
                EmployeeGrid.ItemsSource = DataColec;
            }
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {



                if (DataColec.Count != 0)
                {
                    using (StreamWriter writer =
                   new StreamWriter(await folder.OpenStreamForWriteAsync(
                   "TempSobFil50" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                    {
                        string sSob = "DateTime" + "\t" + "SobTemp";


                        await writer.WriteLineAsync(sSob);
                        foreach (ClassSovpadSob sob in EmployeeGrid.ItemsSource)
                        {

                            string Sob = sob.dateTime.ToString() + "\t" + sob.Temp.ToString();
                            await writer.WriteLineAsync(Sob);
                        }
                    }
                }


                MessageDialog messageDialog = new MessageDialog("Темп сохранен");
                await messageDialog.ShowAsync();

            }
            else
            {

            }
        }
    }
}
