﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace DataYRAN
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPageBDMan2 : Page
    {
        public BlankPageBDMan2()
        {
            this.InitializeComponent();
            listsql1 = new List<string>();
            listsql1.Add("SELECT * from События");
            ClassBDMan2 = new ClassBDMan2();

        }
        ClassBDMan2 ClassBDMan2;
        List<string> listsql1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClassBDMan2.Path = Path.Text;
            listsql1.Clear();
            listsql1.Add(textSql.Text);
            ClassBDMan2.listsql = listsql1;
            ClassBDMan2.storageFile = storageFile;
            this.Frame.Navigate(typeof(BlankPageObrData), ClassBDMan2);
        }
   
        private void GridV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView gridView = (GridView)sender;
            GridViewItem gridViewItem = (GridViewItem)gridView.SelectedItem;
            var tag = gridViewItem.Tag;
            if (tag.ToString() == "TabSob")
            {

                textSql.Text = listsql1.ElementAt(0);
            

            }
            if (tag.ToString() == "TabN")
            {
                //textSql.Text = listsql1.ElementAt(1);
            
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string txtsql = textSql.Text;
            //int x = gridV.SelectedIndex;
            listsql1.RemoveAt(0);
            listsql1.Insert(0, txtsql);
        
        }

     


        private async void MyDate1_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            
        }

        private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {

        }
        string aql = "select * from События Where";
        string aqlNull = "select * from События Where";
        string aqlNull1 = "select * from События";
        public void TimeYs()
        {
            if (ChTime.IsChecked == true)
            {


                string sqlWere = " (ИмяФайла Like ";
                DateTime dateTime = new DateTime(2018, 12, 12);
                dateTime = MyDate1.Date.Value.DateTime;

                int x = 0;
                while (dateTime <= MyDate2.Date.Value.DateTime)
                {
                    if (x > 0)
                    {
                        sqlWere += "or ИмяФайла Like ";
                    }
                    sqlWere += "'%" + dateTime.Day.ToString() + "." + dateTime.Month.ToString() + "." + dateTime.Year.ToString() + "%' ";
                    dateTime = dateTime.AddDays(1);

                    x++;
                }
                aql += sqlWere+") ";
            }
        }
        public void TimeKl()
        {
            if (ChklAll.IsChecked == false)
            {
                string sqlWere;

                if (ChTime.IsChecked == true)
                {
                   sqlWere = "and (Кластер = ";
                }
                else
                {
                     sqlWere = " (Кластер = ";
                }
                List<string> vs = new List<string>();
                if(Chkl1.IsChecked==true)
                {
                    vs.Add("1");
                }
                if (Chkl2.IsChecked == true)
                {
                    vs.Add("2");
                }
                if (Chkl3.IsChecked == true)
                {
                    vs.Add("3");
                }
                if (Chkl4.IsChecked == true)
                {
                    vs.Add("4");
                }
                if (Chkl5.IsChecked == true)
                {
                    vs.Add("5");
                }
                if (Chkl6.IsChecked == true)
                {
                    vs.Add("6");
                }
                int x = 0;
                foreach(var dateTime in vs)
                {
                    if (x > 0)
                    {
                        sqlWere += "or Кластер =";
                    }
                    sqlWere += "'" + dateTime+"' ";
                 

                    x++;
                }

                aql += sqlWere+") ";
            }
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            aql = aqlNull;
            if(ChTime.IsChecked==false && ChklAll.IsChecked==true)
            {
                aql = aqlNull1;
                //aql += " order by Код desc";
            }
            else
            {
                TimeYs();
                TimeKl();
                //aql += "order by Код desc";
            }
          
            textSql.Text = aql;

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if (ChklAll != null && Chkl6 !=null)
            {


                if (ChklAll.IsChecked == true)
                {
                    Chkl1.IsChecked = false;
                    Chkl2.IsChecked = false;
                    Chkl3.IsChecked = false;
                    Chkl4.IsChecked = false;
                    Chkl5.IsChecked = false;
                    Chkl6.IsChecked = false;
                }
                else
                {
                    ChklAll.IsChecked = false;
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
        StorageFile storageFile;
        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".db");
            picker.FileTypeFilter.Add(".DB");
            picker.FileTypeFilter.Add(".db3");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            storageFile = file;
            if (file != null)
            {
                Path.Text = file.Path;
            }
            else
            {

            }
        }
    }
}
