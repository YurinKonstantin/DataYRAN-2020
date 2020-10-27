using Microsoft.Data.Sqlite;
using ObrabotcaURAN;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace DataYRAN.BDWork
{
   public static class BDAcces
    {
        
      static  public List<string> Path { get; set; }
        //"SELECT НомерRun, Синхронизация, ОбщийПорог, Порог, Триггер, ЗначениеТаймера, ВремяЗапуска, ВремяСтоп  from Run"
        public static List<ClassTablRun> GetDataRun(string path )
        {
            List<ClassTablRun> entries = new List<ClassTablRun>();

            using (SqliteConnection db = new SqliteConnection($"Filename={path}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand("SELECT НомерRun, Синхронизация, ОбщийПорог, Порог, Триггер, ЗначениеТаймера, ВремяЗапуска, ВремяСтоп  from Run", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    var cl = new ClassTablRun()
                    {
                        НомерRun = query.GetString(0),
                        Синхронизация = query.GetInt32(1),
                        ОбщийПорог = query.GetInt32(2),
                        Порог = query.GetInt32(3),
                        Триггер = query.GetInt32(4),
                        ЗначениеТаймер = query.GetString(5),
                        ВремяЗапуска = query[6] as string,
                    };
                    string st = String.Empty;
                    cl.ВремяСтоп = query[7] as string;
                    Debug.WriteLine(query.GetString(1) + "\n" + query.GetString(2));
                    entries.Add(cl);
                }
                db.Close();
            }

            return entries;
        }
       public async  static Task<List<ClassSob>> GetDataSob(string uslovie, ObservableCollection<ClassSob> classSobs, string path)
        {
            List<ClassSob> entries = new List<ClassSob>();
            Debug.Write("Start"+"\n"+ path);
            using (SqliteConnection db = new SqliteConnection($"Filename={path}"))
            {
                Debug.Write("OpenStart"+"\n");
             await  db.OpenAsync();
                Debug.Write("OpenStartES");
                SqliteCommand selectCommand = new SqliteCommand("SELECT * from События" + uslovie, db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                Debug.Write("ReadStart");
                while (query.Read())
                {
                    List<ClassSobNeutron> cll = new List<ClassSobNeutron>();
                    int[] masAmp = new int[12];
                    for (int i = 7; i < 19; i++)
                    {
                        masAmp[i - 7] = query.GetInt32(i);
                    }
                    int[] masN = new int[12]; 
                    for (int i = 19; i < 31; i++)
                    {
                        masN[i - 19] = query.GetInt32(i);
                        Debug.WriteLine(query.GetInt32(i).ToString());
                    }
                    int[] masNull = new int[12];
                    for (int i = 31; i < 43; i++)
                    {
                        masNull[i - 31] = query.GetInt32(i);
                    }
                    double[] masS = new double[12];

                    for (int i = 43; i < 55; i++)
                    {
                        try
                        {
                            IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                           
                            masS[i - 43] = Convert.ToDouble(query.GetString(i));
                        }
                        catch (Exception ex)
                        {
                            Debug.Write("ReadError");
                            //   await new MessageDialog(ex.ToString() + i.ToString() + "\t" + query.GetString(i) + "\t" + query.GetString(i).Replace(",", ".")).ShowAsync();
                        }
                    }
                    int badint = query.GetInt32(55);
                    bool bad = false;
                    if (badint == 1)
                    {
                        bad = true;
                    }
                    if (masN.Sum()>0)
                    {
                        for(int i=0; i<12; i++)
                        {
                            if(masN[i]>0)
                            {
                                for (int y = 0; y < masN[i]; y++)
                                {


                                    cll.Add(new ClassSobNeutron()
                                    {
                                        D = i+1,
                                        Amp = 5,
                                        TimeAmp = 2,
                                        TimeEnd = 4,
                                        TimeEnd3 = 2,
                                        TimeFirst = 0,
                                        TimeFirst3 = 1
                                    });
                                }
                            }

                        }
                        
                    }
                    var cl = new ClassTablSob()
                    {
                        Time = query.GetString(1),
                        ИмяФайла = query.GetString(2),
                        Плата = query.GetString(3),
                        Кластер = query.GetString(4),
                        СумАмп = query.GetInt32(5),
                        СумN = query.GetInt32(6),
                        АмпCh = masAmp,
                        NCh = masN,
                        Nul = masNull,
                        sig = masS,
                        bad = bad
                    };
                    string[] strTime = query.GetString(1).Split('.');
                    

                    DataTimeUR dataTimeUR = new DataTimeUR(0, 0, Convert.ToInt16(strTime[0]), Convert.ToInt16(strTime[1]), Convert.ToInt16(strTime[2]), Convert.ToInt16(strTime[3]),
                                 Convert.ToInt16(strTime[4]), Convert.ToInt16(strTime[5]), Convert.ToInt16(strTime[6]));
                    if (query.GetString(2).Contains("Test"))
                    {
                        string[] vs = query.GetString(2).Split("_");
                        string gg = vs[0] + "_" + vs[2];
                        dataTimeUR.corectTime(gg);
                    }
                    else
                    {
                        dataTimeUR.corectTime(query.GetString(2));
                    }

                   string time1 = strTime[0] + "." + strTime[1] + "." + (Convert.ToInt32(strTime[2])).ToString("00") + "." + strTime[3] + "." + strTime[4] + "." + strTime[5] + "." + strTime[6];
                    classSobs.Add(new ClassSob()
                    {
                        nameFile = query.GetString(2),
                        nameklaster =query.GetString(4),
                        nameBAAK = query.GetString(3),
                        time = time1,
                        mAmp = masAmp,
                        dateUR= dataTimeUR,
                     //   dateUR = dataTimeUR,
                     //TimeS0 = timeS[0].ToString(),
                     // TimeS1 = timeS[1].ToString(),
                     // TimeS2 = timeS[2].ToString(),
                     // TimeS3 = timeS[3].ToString(),
                     // TimeS4 = timeS[4].ToString(),
                     // TimeS5 = timeS[5].ToString(),
                     // TimeS6 = timeS[6].ToString(),
                     // TimeS7 = timeS[7].ToString(),
                     //  TimeS8 = timeS[8].ToString(),
                     // TimeS9 = timeS[9].ToString(),
                     // TimeS10 = timeS[10].ToString(),
                     // TimeS11 = timeS[11].ToString(),
                     //  mTimeD = timeS,
                        sig0 = masS[0],
                        sig1 = masS[1],
                        sig2 = masS[2],
                        sig3 = masS[3],
                        sig4 = masS[4],
                        sig5 = masS[5],
                        sig6 = masS[6],
                        sig7 = masS[7],
                        sig8 = masS[8],
                        sig9 = masS[9],
                        sig10 = masS[10],
                        sig11 = masS[11],

                        Nnull0 = Convert.ToInt16(masNull[0]),
                        Nnull1 = Convert.ToInt16(masNull[1]),
                        Nnull2 = Convert.ToInt16(masNull[2]),
                        Nnull3 = Convert.ToInt16(masNull[3]),
                        Nnull4 = Convert.ToInt16(masNull[4]),
                        Nnull5 = Convert.ToInt16(masNull[5]),
                        Nnull6 = Convert.ToInt16(masNull[6]),
                        Nnull7 = Convert.ToInt16(masNull[7]),
                        Nnull8 = Convert.ToInt16(masNull[8]),
                        Nnull9 = Convert.ToInt16(masNull[9]),
                        Nnull10 = Convert.ToInt16(masNull[10]),
                        Nnull11 = Convert.ToInt16(masNull[11]),
                        classSobNeutronsList = cll

                    });
                }
                db.Close();
            }
           
       


            return entries;
        }

        public static List<ClassTablSob> GetDataSob(string uslovie, string path)
        {
            List<ClassTablSob> entries = new List<ClassTablSob>();

            SqliteConnection db =
                new SqliteConnection("Data Source = " + path);
           

            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("SELECT * from События", db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                int[] masAmp = new int[12];
                for (int i = 7; i < 19; i++)
                {
                    masAmp[i - 7] = query.GetInt32(i);
                }
                int[] masN = new int[12];
                for (int i = 19; i < 31; i++)
                {
                    masN[i - 19] = query.GetInt32(i);
                }
                int[] masNull = new int[12];
                for (int i = 31; i < 43; i++)
                {
                    masNull[i - 31] = query.GetInt32(i);
                }
                double[] masS = new double[12];

                for (int i = 43; i < 55; i++)
                {
                    try
                    {


                        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                        masS[i - 43] = Convert.ToDouble(query[i], formatter);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString() + i.ToString() + "\t" + query.GetString(i) + "\t" + query.GetString(i).Replace(",", "."));
                    }
                }
                int badint = query.GetInt32(55);
                bool bad = false;
                if (badint == 1)
                {
                    bad = true;
                }

                var cl = new ClassTablSob()
                {
                    Time = query.GetString(1),
                    ИмяФайла = query.GetString(2),
                    Плата = query.GetString(3),
                    Кластер = query.GetString(4),
                    СумАмп = query.GetInt32(5),
                    СумN = query.GetInt32(6),
                    АмпCh = masAmp,
                    NCh = masN,
                    Nul = masNull,
                    sig = masS,
                    bad = bad



                };





                entries.Add(cl);
            }

            db.Close();


            return entries;
        }
        public static List<ClassTablFile> GetDataFile(string uslovie, string path)
        {
            List<ClassTablFile> entries = new List<ClassTablFile>();

            SqliteConnection db =
                new SqliteConnection("Data Source = " + path);


            db.Open();

            SqliteCommand selectCommand = new SqliteCommand("SELECT * from Файлы", db);

            SqliteDataReader query = selectCommand.ExecuteReader();

            while (query.Read())
            {
                var ff = new ClassTablFile() {
                    ИмяФайла= query.GetString(1),
                    Плата= query.GetString(2),
                    TimeCrete= query.GetString(3),
                    TimeClose= query.GetString(4),
                    Run= query.GetString(5)

                };


                entries.Add(ff);
            }

            db.Close();


            return entries;
        }

    }
    public class ClassTablRun
    {
        public string НомерRun { get; set; }
        public int Синхронизация { get; set; }
        public int ОбщийПорог { get; set; }
        public int Порог { get; set; }
        public int Триггер { get; set; }
        public string ЗначениеТаймер { get; set; }
        public string ВремяЗапуска { get; set; }
        public string ВремяСтоп { get; set; }

    }
    public class ClassTablSob
    {
        public string Time { get; set; }
        public string ИмяФайла { get; set; }
        public string Плата { get; set; }
        public string Кластер { get; set; }
        public int СумАмп { get; set; }
        public int СумN { get; set; }
        public int[] АмпCh { get; set; }
        public int[] NCh { get; set; }
        public int[] Nul { get; set; }
        public double[] sig { get; set; }
        public bool bad { get; set; }

    }
    public class ClassTablFile
    {
        /// <summary>
        /// Поле Время
        /// </summary>
        public string TimeCrete { get; set; }
        public string TimeClose { get; set; }
        public string ИмяФайла { get; set; }
        public string Плата { get; set; }
        public string Run { get; set; }
        

    }

}
