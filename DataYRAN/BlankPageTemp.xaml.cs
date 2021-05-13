using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
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
using static DataYRAN.Pasport.BlankPagePasport;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace DataYRAN
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPageTemp : Page
    {
        public BlankPageTemp()
        {
            this.InitializeComponent();
        }
        ObservableCollection<ClassSob> classRazvertka = new ObservableCollection<ClassSob>();
        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {

            
            if (e.Parameter is ObservableCollection<ClassSob>)
            {
               

               classRazvertka = e.Parameter as ObservableCollection<ClassSob>;

              





            }
            else
            {
               
            }

            base.OnNavigatedTo(e);
        }
        public async Task Temp(ObservableCollection<ClassSob> classRazvertka1, int cloc)
        {
           
            var orderedNumbers = from ClassSob in classRazvertka1
                                 orderby ClassSob.dateUR.DateTimeString()
                                 select ClassSob;
            DateTime dateTime = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);
            DateTime dateTimeFirst = new DateTime(orderedNumbers.ElementAt(0).dateUR.GG, orderedNumbers.ElementAt(0).dateUR.MM, orderedNumbers.ElementAt(0).dateUR.DD, orderedNumbers.ElementAt(0).dateUR.HH, 0, 0, 0);

            DateTime dateTime1 = dateTime; //new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
            DateTime dateTimeEnd = new DateTime(orderedNumbers.ElementAt(orderedNumbers.Count()-1).dateUR.GG, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.MM, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.DD, orderedNumbers.ElementAt(orderedNumbers.Count() - 1).dateUR.HH, 0, 0, 0);

            dateTime1 = dateTime1.AddHours(cloc);
            int col = 0;
            double[] mas = new double[12];
            double[] masN = new double[12];
            for (int i = 0; i <= dateTimeEnd.Subtract(dateTimeFirst).TotalHours; i++)
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
                }
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    DataColec.Add(new ClassTemp() { dateTime = dateTime, mTemp = mas, colSob = col });
    DataColecN.Add(new ClassTemp() { dateTime = dateTime, mTemp = masN });
});
                
                col = 0;
                mas = new double[12];
                masN = new double[12];
                dateTime = dateTime1;
                dateTime1 = dateTime1.AddHours(cloc);
           
            }

            MessageDialog messageDialog = new MessageDialog("Конец");
            await messageDialog.ShowAsync();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPageObrData));
        }
        ObservableCollection<ClassTemp> _DataColec = new ObservableCollection<ClassTemp>();
        public ObservableCollection<ClassTemp> DataColec
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

        ObservableCollection<ClassTemp> _DataColecN = new ObservableCollection<ClassTemp>();
        public ObservableCollection<ClassTemp> DataColecN
        {
            get
            {
                return _DataColecN;
            }
            set
            {
                _DataColecN = value;

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
                        string sSob = "DateTime"  + "\t" + "SobTemp" + "\t" + "SumDTemp"  + "\t" + "TempD1" + "\t" + "TempD2" + "\t" + "TempD3" + "\t" + "TempD4" + "\t" + "TempD5" + "\t" + "TempD6" + "\t" + "TempD7"
                            + "\t" + "TempD8" + "\t" + "TempD9" + "\t" + "TempD10" + "\t" + "TempD11" + "\t" + "TempD12";

                        await writer.WriteLineAsync(sSob);
                        foreach (ClassTemp sob in DataColec)
                        {
                            string Sob = sob.dateTime.ToString() + "\t" + sob.colSob.ToString() + "\t" + sob.Temp.ToString()+ "\t" + sob.mTemp[0].ToString() + "\t" + sob.mTemp[1].ToString() + "\t" +
                            sob.mTemp[2].ToString() + "\t" + sob.mTemp[3].ToString() + "\t" + sob.mTemp[4].ToString() + "\t" + sob.mTemp[5].ToString() + "\t" + sob.mTemp[6].ToString() + "\t" + sob.mTemp[7].ToString() +
                            "\t" + sob.mTemp[8].ToString() + "\t" + sob.mTemp[9].ToString() + "\t" + sob.mTemp[10].ToString() + "\t" + sob.mTemp[11].ToString();
                            await writer.WriteLineAsync(Sob);
                        }
                    }
                }
                if (DataColecN.Count != 0)
                {
                    using (StreamWriter writer =
                   new StreamWriter(await folder.OpenStreamForWriteAsync(
                   "TempSobN" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                    {
                        string sSob = "DateTime" + "\t" + "STemp" + "\t" + "TempD1" + "\t" + "TempD2" + "\t" + "TempD3" + "\t" + "TempD4" + "\t" + "TempD5" + "\t" + "TempD6" + "\t" + "TempD7"
                            + "\t" + "TempD8" + "\t" + "TempD9" + "\t" + "TempD10" + "\t" + "TempD11" + "\t" + "TempD12";


                        await writer.WriteLineAsync(sSob);
                        foreach (ClassTemp sob in DataColecN)
                        {

                            string Sob = sob.dateTime.ToString() + "\t" + sob.Temp.ToString() + "\t" + sob.mTemp[0].ToString() + "\t" + sob.mTemp[1].ToString() + "\t" +
                            sob.mTemp[2].ToString() + "\t" + sob.mTemp[3].ToString() + "\t" + sob.mTemp[4].ToString() + "\t" + sob.mTemp[5].ToString() + "\t" + sob.mTemp[6].ToString() + "\t" + sob.mTemp[7].ToString() +
                            "\t" + sob.mTemp[8].ToString() + "\t" + sob.mTemp[9].ToString() + "\t" + sob.mTemp[10].ToString() + "\t" + sob.mTemp[11].ToString();
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
        private void rankLowFilter_Click(object sender, RoutedEventArgs e)
        {
            int x = 50;
            EmployeeGrid.ItemsSource = new ObservableCollection<ClassTemp>(from item in DataColec
                                                                              where item.colSob > x
                                                                              select item);
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
                        foreach (ClassTemp sob in EmployeeGrid.ItemsSource)
                        {

                            string Sob = sob.dateTime.ToString() + "\t" + sob.colSob.ToString();
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


        private void rankLowFilterN_Click(object sender, RoutedEventArgs e)
        {
            int x = 50;
           NeutronGrid.ItemsSource = new ObservableCollection<ClassTemp>(from item in DataColecN
                                                                           where item.Temp > x
                                                                           select item);
        }
        private void ClearFilterN_Click(object sender, RoutedEventArgs e)
        {
            if (NeutronGrid != null)
            {
                NeutronGrid.ItemsSource = DataColecN;
            }
        }

        private async void AppBarButtonN_Click(object sender, RoutedEventArgs e)
        {

            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            Windows.Storage.StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {



                if (DataColecN.Count != 0)
                {
                    using (StreamWriter writer =
                   new StreamWriter(await folder.OpenStreamForWriteAsync(
                   "TempSobNFil50" + "." + "txt", CreationCollisionOption.GenerateUniqueName)))
                    {
                        string sSob = "DateTime" + "\t" + "SobTempN";


                        await writer.WriteLineAsync(sSob);
                        foreach (ClassTemp sob in NeutronGrid.ItemsSource)
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

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            await Temp(classRazvertka, 1);
            _MainViewModelSob = new MainViewModel(DataColec.ToList());
            oxiSob.Model = _MainViewModelSob.MyModel;
            MessageDialog messageDialog = new MessageDialog("Данные обработаны");
            await messageDialog.ShowAsync();

        }
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
        MainViewModel _MainViewModelSob;
        MainViewModel _MainViewModelN;
        public class Data
        {
            public double Category { get; set; }

            public double Value { get; set; }
        }
        public class MainViewModel
        {
            public async void ss(object sender, EventArgs e)
            {
                MessageDialog messageDialog = new MessageDialog("Ok");
                await messageDialog.ShowAsync();
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
                DateTimeAxis f= new DateTimeAxis { Title = "Дата", MajorStep = mag, MinorStep = 1, StringFormat = "dd.MM.yyyy", Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid };

                LinearAxis d1=  new LinearAxis { Title = "Nch/Ns", Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Minimum = 0, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, };
                MyModel.Axes.Add(f);
                MyModel.Axes.Add(d1);
              


                List<DataSig2> datas;

                for (int i = 0; i < 12; i++)
                {
                    LineSeries linearAxis = new LineSeries() { MarkerFill = OxyColor.FromRgb(CustomPalettes.Color1[i].Color.R, CustomPalettes.Color1[i].Color.G, CustomPalettes.Color1[i].Color.B), Title = "D" + (i + 1).ToString(), MarkerType = OxyPlot.MarkerType.None }; ;
                   // ScatterSeries linearAxis = new ScatterSeries() { MarkerFill = OxyColor.FromRgb(CustomPalettes.Color1[i].Color.R, CustomPalettes.Color1[i].Color.G, CustomPalettes.Color1[i].Color.B), Title = "D" + (i + 1).ToString(), MarkerType = OxyPlot.MarkerType.Circle };
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



                        linearAxis.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value));
                    }
                    this.MyModel.Series.Add(linearAxis);
                }
                d1.MouseUp += ss;
               

            }
            public MainViewModel(List<ClassTemp> SigM, string g)
            {

                this.MyModel = new PlotModel { PlotAreaBorderColor = OxyColors.Transparent, LegendPlacement = LegendPlacement.Outside, TitleFontSize = 26, LegendFontSize = 26 };
                var startDate = SigM.ElementAt(0).dateTime;

                var endDate = SigM.ElementAt(SigM.Count() - 1).dateTime;
                double mag = 1;



                if (SigM.Count() > 7)
                {
                    mag = 6;
                }
                var minValue = DateTimeAxis.ToDouble(startDate);
                var maxValue = DateTimeAxis.ToDouble(endDate);
                MyModel.Axes.Add(new DateTimeAxis { Title = "Дата", MajorStep = mag, MinorStep = 1, StringFormat = "dd.MM.yyyy", Position = AxisPosition.Bottom, Minimum = minValue, Maximum = maxValue, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, });
                MyModel.Axes.Add(new OxyPlot.Axes.LinearAxis { Title = "N(nd)/Ns", Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Minimum = 0, AxislineThickness = 2, FontSize = 26, TitleFontSize = 26, AxislineStyle = LineStyle.Solid, });

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
                MyModel.Axes.Add(new OxyPlot.Axes.LogarithmicAxis { Position = AxisPosition.Bottom, MaximumPadding = 0.1, Maximum = 10, MinimumPadding = 0.1, Title = "dsfs" });
                MyModel.Axes.Add(new OxyPlot.Axes.LogarithmicAxis { Position = AxisPosition.Left, MaximumPadding = 0.1, MinimumPadding = 0.1, Title = "xcvxvx" });


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
    }
}
