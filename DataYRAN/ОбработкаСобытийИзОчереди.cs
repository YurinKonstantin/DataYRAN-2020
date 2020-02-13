﻿using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.UI.Core;
using Windows.UI.Popups;


namespace DataYRAN
{
    public sealed partial class BlankPageObrData
    {

     
        public async Task WriteInFileIzOcherediAsync(CancellationToken cancellationToken, ClassUserSetUp classUserSetUp, string script)//работа с данными из очереди
        {

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        //Thread.Sleep(50);
                        while (OcherediNaObrab.Count >0)
                        {
                           try
                           {
                            await Obr(classUserSetUp, script);
                           }
                           catch(Exception ex)
                           {

                           }
                        }
                        break;
                    }
                    //if(OcherediNaObrab.Count > 0)
                    {
                        await Obr(classUserSetUp, script);
           
                    }
                   
                
               
                }
            
          
        }
        object locker = new object();
        private async Task<List<ClassSobNeutron>> neutron(int[,] n, double[] masnul, bool bad, double[] sig)//out int[] coutN,
        {
            
            //List<ClassSobNeutron> listNet= new List<ClassSobNeutron>();
            int dlitOtb = (int)ClassUserSetUp.DlitN3;
            int AmpOtbora = (int)ClassUserSetUp.PorogN;
            int[] coutN = new int[12];
           
            int Nu;
            int AmpOtbora1;
            List<ClassSobNeutron> clasNeu = new List<ClassSobNeutron>();
          
           

            for (int i = 0; i < 12; i++)
                {
               
                    if (sig[i] < 4)
                    {
                        Nu = Convert.ToInt32(masnul[i]);
                        AmpOtbora1 = AmpOtbora + Nu;


                        for (int j = 100; j < 20000; j++)
                        {

                            int Amp = n[i, j];
                            if (Amp >= AmpOtbora1)//ищем претендента на нейтрон по порогу
                            {
                                int countmaxtime = j;
                                int countfirsttime = j;
                                int countendtime = j;
                                int countfirsttime3 = j;
                                int countendtime3 = j;


                                int v = Amp;
                                int v1 = Amp;
                                while (v > Nu + 3 && v1 > Nu + 3)//Ищем конец сигнала претендента нейтрона на уровне 3
                                {
                                    countendtime3++;
                                    countfirsttime3--;
                                    if (countendtime3 < 20000)
                                    {
                                        v = n[i, countendtime3];
                                    }
                                    else
                                    {
                                        countendtime3--;
                                    break;

                                    }
                                    if (countfirsttime3 > 100)
                                    {
                                        v1 = n[i, countfirsttime3];

                                    }
                                    else
                                    {
                                        countfirsttime3++;
                                    break;
                                    }
                                }


                                if (countendtime3 - countfirsttime3 >= dlitOtb)
                                {
                                    countendtime = countendtime3;
                                    countfirsttime = countfirsttime3;
                                    v = Amp;
                                    v1 = Amp;
                                    while (v > Nu && v1 > Nu)
                                    {
                                        countendtime++;
                                        countfirsttime--;
                                        if (countendtime > 19999 || countfirsttime < 90)
                                        {
                                            break;
                                        }
                                        v = n[i, countendtime];
                                        v1 = n[i, countfirsttime];
                                    }





                                    for (int v11 = countfirsttime3; v11 <= countendtime3; v11++)//точка максимум и значение максимум
                                    {

                                        if (Amp <= n[i, v11])
                                        {
                                            Amp = n[i, v11];
                                            countmaxtime = v11;

                                        }

                                    }

                                    try
                                    {
                                        int dd = i + 1;
                                        Amp = Amp - Nu;

                                        if (!bad)
                                        {




                                            clasNeu.Add(new ClassSobNeutron()
                                            {
                                                D = dd, 
                                                Amp = Amp, 
                                                TimeAmp = countmaxtime, 
                                                TimeEnd = countendtime, 
                                                TimeEnd3 = countendtime3, 
                                                TimeFirst = countfirsttime, 
                                                TimeFirst3 = countfirsttime3
                                            });

                                        }
                                    }
                                    catch(Exception)
                                    {

                                    }
                                }
                                if ((countendtime + 2) < 19999)
                                {
                                j = countendtime + 2;
                                }
                                  
                            }

                        }
                    }


                }
          

            return clasNeu;

        }
        private int[] ColSigVatias(int[,] raz, double[] masnul)//out int[] coutN,
        {
           
            int AmpOtbora = 5;
            int[] coutN = new int[12];
            int Nu;
            int AmpOtbora1;
           
            for (int i = 0; i < 12; i++)
            {
                Nu = Convert.ToInt32(masnul[i]);
                AmpOtbora1 = AmpOtbora + Nu;
                for (int j = 349; j < 1024; j++)
                {
                   int Amp = raz[i, j];
                   if (Amp >= AmpOtbora1)//ищем претендента на сигнал
                   {
                       int countendtime = j;
                       int v = Amp;
                       while (v > Nu+2)//ищем конец сигнала
                            {
                               countendtime++;
                              
                               if (countendtime >1023)
                               {
                                  break;
                               }
                               v = raz[i, countendtime];
                              
                            }
                       coutN[i] += 1;
                       j = countendtime;
                   }
                }             
            }
            return coutN;
        }
        private int[] ColBin(int[,] raz, double[] masnul)//out int[] coutN,
        {

            int AmpOtbora = 5;
            int[] coutB = new int[12];
            int Nu;
            int AmpOtbora1;

            for (int i = 0; i < 12; i++)
            {
                Nu = Convert.ToInt32(masnul[i]);
                AmpOtbora1 = AmpOtbora + Nu;
                for (int j = 349; j < 1024; j++)
                {
                    int Amp = raz[i, j];
                    if (Amp >= AmpOtbora1)//ищем претендента на сигнал
                    {
                        coutB[i] += 1;
                    }
                }
            }
            return coutB;
        }
        public async Task Obr(ClassUserSetUp classUserSetUp, string script)
        {
           
                    string time1 = String.Empty;
                  bool b=  OcherediNaObrab.TryDequeue(out MyclasDataizFile ObrD);
                    if (b == true)
                    {
                        switch (ObrD.tipName)
                        {
                            case "T":
                                try
                                {
                                    string ss = ParserBAAK12.ParseBinFileBAAK12.ParseBinFileBAAK200H(ObrD.Buf00, out int[,] data1, out time1, out int[,] dataTail1);
                                    if (ss == "1")
                                    {
                                        int t = 0;
                                        string[] strTime = time1.Split('.');
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                                        () =>
                                        {
                                             t = Convert.ToInt32(TimeCorect.Text);
                                        });
                              
                                DataTimeUR dataTimeUR = new DataTimeUR(0, 0, Convert.ToInt16(strTime[0]), Convert.ToInt16(strTime[1]), Convert.ToInt16(strTime[2]) + t, Convert.ToInt16(strTime[3]),
                                             Convert.ToInt16(strTime[4]), Convert.ToInt16(strTime[5]), Convert.ToInt16(strTime[6]));
                                if(ObrD.NameFile.Contains("Test"))
                                {
                                    string[] vs = ObrD.NameFile.Split("_");
                                    string gg = vs[0] + "_" + vs[2];
                                    dataTimeUR.corectTime(gg);
                                }
                                else
                                {
                                    dataTimeUR.corectTime(ObrD.NameFile);
                                }
                              
                                        time1 = strTime[0] + "." + strTime[1] + "." + (Convert.ToInt32(strTime[2]) + t).ToString("00") + "." + strTime[3] + "." + strTime[4] + "." + strTime[5] + "." + strTime[6];
                                        await ObrSigData(ObrD.NameFile, ObrD.NameBaaR12.ToString(), data1, dataTail1, time1, ObrD.tipName, classUserSetUp, dataTimeUR, script);
                                    }
                                    ObrD = null;
                                }
                                catch (Exception ex)
                                {
                       
                                }
                                break;
                            case "N":
                                try
                                {
                                    int[,] dataTail1 = new int[12, 20000];
                                    ParserBAAK12.ParseBinFileBAAK12.ParseBinFileBAAK200(ObrD.Buf00, 1, out int[,] data1, out time1);
                                    try
                                    {
                                        await SaveFileDelegate(data1, dataTail1, time1, ObrD.NameFile, false);
                                    }
                                    catch
                                    {

                                    }
                                    try
                                    {
                                        int t = 0;
                                        string[] strTime = time1.Split('.');
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                                        () =>
                                        {
                                           t = Convert.ToInt32(TimeCorect.Text);
                                        });
                                        time1 = strTime[0] + "." + strTime[1] + "." + (Convert.ToInt32(strTime[2]) + t).ToString() + "." + strTime[3] + "." + strTime[4] + "." + strTime[5] + "." + strTime[6];

                                        await ObrSigData(ObrD.НулеваяЛиния, ObrD.NameFile, ObrD.NameBaaR12.ToString(), data1, time1, ObrD.tipName, classUserSetUp);
                                    }
                                    catch (Exception ex)
                                    {
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                                        () =>
                                        {
                                             var mess = new MessageDialog("dss" + "\n" + ex.Message.ToString() + "\n" + ex.ToString());
                                             mess.ShowAsync();// КолПакетовОчер++;
                                        });
                                    }

                                    ObrD = null;
                                }
                                catch (Exception ex)
                                {

                                }
                                break;

                            case "V":
                                try
                                {
                     
                            int[,] dataTail1 = new int[12, 20000];
                                    ParserBAAK12.ParseBinFileBAAK12.ParseBinFileBAAK200(ObrD.Buf00, 1, out int[,] data1, out time1);
                                    try
                                    {

                                        await SaveFileDelegate(data1, dataTail1, time1, ObrD.NameFile, false);
                                    }
                                    catch(Exception )
                                    {

                                    }
                                    try
                                    {
                                        int t = 0;
                                        string[] strTime = time1.Split('.');
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                                        () =>
                                        {
                                          t = Convert.ToInt32(TimeCorect.Text);
                                        });
                                        time1 = strTime[0] + "." + strTime[1] + "." + (Convert.ToInt32(strTime[2]) + t).ToString() + "." + strTime[3] + "." + strTime[4] + "." + strTime[5] + "." + strTime[6];

                                        await ObrSigData(ObrD.НулеваяЛиния, ObrD.NameFile, ObrD.NameBaaR12.ToString(), data1, time1, ObrD.tipName, classUserSetUp);
                                    }
                                    catch (Exception ex)
                                    {
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                                        () =>
                                        {
                                            var mess = new MessageDialog("dss" + "\n" + ex.Message.ToString() + "\n" + ex.ToString());
                                            mess.ShowAsync();// КолПакетовОчер++;
                                        });
                                    }

                                    ObrD = null;
                                }
                                catch (Exception ex)
                                {

                                }
                                break;
                            default:
                    
                        break;
                        }
                    }
        }

        /// <summary>
        /// Обработка события с хвостом
        /// </summary>
        /// <param name="nameFile"></param>
        /// <param name="nemeBAAK"></param>
        /// <param name="data1"></param>
        /// <param name="dataTail1"></param>
        /// <param name="time1"></param>
        /// <param name="tipN"></param>
        /// <param name="classUserSetUp"></param>
        /// <param name="dataTimeUR"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public async Task ObrSigData(string nameFile, string nemeBAAK,   int[,] data1, int[,] dataTail1, string time1, string tipN, ClassUserSetUp classUserSetUp, DataTimeUR dataTimeUR, string script)
        {
          
            int[] Amp = new int[12];
            double[] Nul = new double[12];
            int[] coutN1 = new int[12];
            double[] sig = new double[12];
            bool bad = false;
            int[] timeS = new int[12];
            List<ClassSobNeutron> cll = new List<ClassSobNeutron>();
            ParserBAAK12.ParseBinFileBAAK12.MaxAmpAndNul(data1, ref sig, ref Amp, ref Nul, ref bad, ClassUserSetUp.ObrNoise, ClassUserSetUp.KoefNoise, classUserSetUp.PorogS);
            if (!bad)
                {
                Stopwatch watch = Stopwatch.StartNew();
                watch.Reset();
                watch.Start();
                cll = await neutron(dataTail1, Nul, bad, sig);
                watch.Stop();
                Debug.WriteLine("dfgd"+watch.ElapsedMilliseconds.ToString());
                timeS = ParserBAAK12.ParseBinFileBAAK12.TimeS(data1, classUserSetUp.PorogS, Amp, Nul);
                }
          
            string[] array = nameFile.Split('_');
            string nameFileNew = String.Empty;
            string nameKlNew = String.Empty;

            if (nameFile.Contains("Test"))
            {
                nameFileNew = nameFile;
                nameKlNew = array[0];
            }
           else
            {
                
              
                if (array.Length > 2)
                {
                    nameFileNew = nameFile;
                    nameKlNew = array[0];
                }
                if (array.Length == 2)
                {
                    if (array[0].Contains("№"))
                    {
                        nameFileNew = array[0].Split("№")[1] + "_" + array[1] + "_" + "T";
                        nameKlNew = array[0].Split("№")[1];
                    }
                    else
                    {
                        nameFileNew = nameFile + "_" + "Т";
                        nameKlNew = array[0];
                    }

                }
            }
           

            if (!bad)
            {
                Debug.WriteLine(Convert.ToInt16(Nul[0]).ToString() + "\t" + Nul[0].ToString(".00"));


                          await  Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High,
     () =>
     {
         ViewModel.ClassSobsT.Add(new ClassSob()
         {
             nameFile = nameFileNew,
             nameklaster = nameKlNew,
             nameBAAK = nemeBAAK,
             time = dataTimeUR.TimeString(),
             mAmp=Amp,
             dateUR=dataTimeUR,
             TimeS0 = timeS[0].ToString(),
             TimeS1 = timeS[1].ToString(),
             TimeS2 = timeS[2].ToString(),
             TimeS3 = timeS[3].ToString(),
             TimeS4 = timeS[4].ToString(),
             TimeS5 = timeS[5].ToString(),
             TimeS6 = timeS[6].ToString(),
             TimeS7 = timeS[7].ToString(),
             TimeS8 = timeS[8].ToString(),
             TimeS9 = timeS[9].ToString(),
             TimeS10 = timeS[10].ToString(),
             TimeS11 = timeS[11].ToString(),
             mTimeD=timeS,
             sig0 = sig[0],
             sig1 = sig[1],
             sig2 = sig[2],
             sig3 = sig[3],
             sig4 = sig[4],
             sig5 = sig[5],
             sig6 = sig[6],
             sig7 = sig[7],
             sig8 = sig[8],
             sig9 = sig[9],
             sig10 = sig[10],
             sig11 = sig[11],
            
             Nnull0 = Convert.ToInt16(Nul[0]),
             Nnull1 = Convert.ToInt16(Nul[1]),
             Nnull2 = Convert.ToInt16(Nul[2]),
             Nnull3 = Convert.ToInt16(Nul[3]),
             Nnull4 = Convert.ToInt16(Nul[4]),
             Nnull5 = Convert.ToInt16(Nul[5]),
             Nnull6 = Convert.ToInt16(Nul[6]),
             Nnull7 = Convert.ToInt16(Nul[7]),
             Nnull8 = Convert.ToInt16(Nul[8]),
             Nnull9 = Convert.ToInt16(Nul[9]),
             Nnull10 = Convert.ToInt16(Nul[10]),
             Nnull11 = Convert.ToInt16(Nul[11]),
             classSobNeutronsList=cll

         });
         ViewModel.CountObrabSob++;

     });
                        
                      
 
            }
            else
            {

               
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
 () => {
     ViewModel._DataColecSobPloxT.Add(new ClassSob()
     {
         nameFile = nameFile,
         nameklaster = array[0],
         nameBAAK = nemeBAAK,
         time = time1,
         mAmp = Amp,
         /* Amp0 = Convert.ToInt16(Amp[0]),
          Amp1 = Convert.ToInt16(Amp[1]),
          Amp2 = Convert.ToInt16(Amp[2]),
          Amp3 = Convert.ToInt16(Amp[3]),
          Amp4 = Convert.ToInt16(Amp[4]),
          Amp5 = Convert.ToInt16(Amp[5]),
          Amp6 = Convert.ToInt16(Amp[6]),
          Amp7 = Convert.ToInt16(Amp[7]),
          Amp8 = Convert.ToInt16(Amp[8]),
          Amp9 = Convert.ToInt16(Amp[9]),
          Amp10 = Convert.ToInt16(Amp[10]),
          Amp11 = Convert.ToInt16(Amp[11]),
          */
       //  Nnut0 = Convert.ToInt16(coutN1[0]),
        // Nnut1 = Convert.ToInt16(coutN1[1]),
        // Nnut2 = Convert.ToInt16(coutN1[2]),
        // Nnut3 = Convert.ToInt16(coutN1[3]),
       //  Nnut4 = Convert.ToInt16(coutN1[4]),
        // Nnut5 = Convert.ToInt16(coutN1[5]),
        //Nnut6 = Convert.ToInt16(coutN1[6]),
       //  Nnut7 = Convert.ToInt16(coutN1[7]),
        // Nnut8 = Convert.ToInt16(coutN1[8]),
       //  Nnut9 = Convert.ToInt16(coutN1[9]),
       //  Nnut10 = Convert.ToInt16(coutN1[10]),
       //  Nnut11 = Convert.ToInt16(coutN1[11]),
         TimeS0 = timeS[0].ToString(),
         TimeS1 = timeS[1].ToString(),
         TimeS2 = timeS[2].ToString(),
         TimeS3 = timeS[3].ToString(),
         TimeS4 = timeS[4].ToString(),
         TimeS5 = timeS[5].ToString(),
         TimeS6 = timeS[6].ToString(),
         TimeS7 = timeS[7].ToString(),
         TimeS8 = timeS[8].ToString(),
         TimeS9 = timeS[9].ToString(),
         TimeS10 = timeS[10].ToString(),
         TimeS11 = timeS[11].ToString(),
         mTimeD=timeS,
         sig0 = sig[0],
         sig1 = sig[1],
         sig2 = sig[2],
         sig3 = sig[3],
         sig4 = sig[4],
         sig5 = sig[5],
         sig6 = sig[6],
         sig7 = sig[7],
         sig8 = sig[8],
         sig9 = sig[9],
         sig10 = sig[10],
         sig11 = sig[11],
        // SumAmp = Convert.ToInt32(Amp.Sum()),
         //SumNeu = coutN1.Sum(),
         Nnull0 = Convert.ToInt16(Nul[0]),
         Nnull1 = Convert.ToInt16(Nul[1]),
         Nnull2 = Convert.ToInt16(Nul[2]),
         Nnull3 = Convert.ToInt16(Nul[3]),
         Nnull4 = Convert.ToInt16(Nul[4]),
         Nnull5 = Convert.ToInt16(Nul[5]),
         Nnull6 = Convert.ToInt16(Nul[6]),
         Nnull7 = Convert.ToInt16(Nul[7]),
         Nnull8 = Convert.ToInt16(Nul[8]),
         Nnull9 = Convert.ToInt16(Nul[9]),
         Nnull10 = Convert.ToInt16(Nul[10]),
         Nnull11 = Convert.ToInt16(Nul[11]),



     });

 });
                      
                  
                   
                
            }
            if (SaveFileDelegate != null)
            {
                if (script != String.Empty)
                {

                    string yNumber = "false";
                    ClassSob classSob = new ClassSob()
                    {
                        nameFile = nameFileNew,
                        nameklaster = nameKlNew,
                        nameBAAK = nemeBAAK,
                        time = dataTimeUR.TimeString(),
                        mAmp = Amp,
                        dateUR = dataTimeUR,


                        TimeS0 = timeS[0].ToString(),
                        TimeS1 = timeS[1].ToString(),
                        TimeS2 = timeS[2].ToString(),
                        TimeS3 = timeS[3].ToString(),
                        TimeS4 = timeS[4].ToString(),
                        TimeS5 = timeS[5].ToString(),
                        TimeS6 = timeS[6].ToString(),
                        TimeS7 = timeS[7].ToString(),
                        TimeS8 = timeS[8].ToString(),
                        TimeS9 = timeS[9].ToString(),
                        TimeS10 = timeS[10].ToString(),
                        TimeS11 = timeS[11].ToString(),
                        mTimeD = timeS,
                        sig0 = sig[0],
                        sig1 = sig[1],
                        sig2 = sig[2],
                        sig3 = sig[3],
                        sig4 = sig[4],
                        sig5 = sig[5],
                        sig6 = sig[6],
                        sig7 = sig[7],
                        sig8 = sig[8],
                        sig9 = sig[9],
                        sig10 = sig[10],
                        sig11 = sig[11],

                        Nnull0 = Convert.ToInt16(Nul[0]),
                        Nnull1 = Convert.ToInt16(Nul[1]),
                        Nnull2 = Convert.ToInt16(Nul[2]),
                        Nnull3 = Convert.ToInt16(Nul[3]),
                        Nnull4 = Convert.ToInt16(Nul[4]),
                        Nnull5 = Convert.ToInt16(Nul[5]),
                        Nnull6 = Convert.ToInt16(Nul[6]),
                        Nnull7 = Convert.ToInt16(Nul[7]),
                        Nnull8 = Convert.ToInt16(Nul[8]),
                        Nnull9 = Convert.ToInt16(Nul[9]),
                        Nnull10 = Convert.ToInt16(Nul[10]),
                        Nnull11 = Convert.ToInt16(Nul[11]),
                        classSobNeutronsList = cll


                    };
                    double[] mm = classSob.mSig;
                    ScriptEngine engine = Python.CreateEngine();
                    ScriptScope scope = engine.CreateScope();
                    scope.SetVariable("Rez", yNumber);
                
                    scope.SetVariable("ClassSob", classSob);
                    engine.Execute(script, scope);
                    dynamic zNumber = scope.GetVariable("Rez");
                    // dynamic xNumber = scope.GetVariable("x");
                    //   dynamic zNumber = scope.GetVariable("z");
                    if(zNumber.ToString()=="true")
                    {
                        await SaveFileDelegate?.Invoke(data1, dataTail1, time1, nameFile, true);
                    }
                }
                else
                {
                    await SaveFileDelegate?.Invoke(data1, dataTail1, time1, nameFile, true);
                }
               
            }



        }
        public async Task ObrSigData(int[] nul, string nameFile, string nemeBAAK, int[,] data1, string time1, string tipN, ClassUserSetUp classUserSetUp)
        {


            int[] Amp = new int[12];
            double[] Nul = new double[12];
            int[] coutN1 = new int[12];
            Double[] sig = new Double[12];
            bool bad = false;
            int[] timeS = new int[12];
            double[] sumDetQ = new double[12];
            double[,] data1S = new double[12, 1024];
            int[] maxTime = new int[12];
            int[] PolovmaxTime = new int[12];
            int[] maxAmp = new int[12];
            int[] Amp34 = new int[12];
            int[] Amp14 = new int[12];
            int d = 1;
            int[] sumSig = new int[12];
            int[] sumBB = new int[12];
            int[] firstTimeN = new int[12];
            try
            {

             
                switch (tipN)
                {
                    
                    case "N":
                        ParserBAAK12.ParseBinFileBAAK12.MaxAmpAndNul(data1, ref sig, ref Amp, ref Nul, ref bad, ClassUserSetUp.ObrNoise, ClassUserSetUp.KoefNoise, classUserSetUp.PorogS);
                        for (int i = 0; i < 12; i++)
                        {
                            for (int j = 0; j < 1024; j++)
                            {
                                data1S[i, j] = Convert.ToDouble(data1[i, j]) - Nul[i];
                            }
                        }
                        ParserBAAK12.ParseBinFileBAAK12.SumSig(data1S, out sumDetQ);
                        break;
                    case "V":
                      //  Debug.WriteLine("Es0");
                        ParserBAAK12.ParseBinFileBAAK12.MaxAmpAndNul(data1, ref sig, ref Amp, ref Nul, ref bad, ClassUserSetUp.ObrNoise, ClassUserSetUp.KoefNoise, classUserSetUp.PorogS);
                       // Debug.WriteLine("Es1");
                        ObrabotcaURAN.Obrabotca.AmpAndTime(data1, Nul, out maxTime, out maxAmp);
                       // Debug.WriteLine("Es2");
                        bool dN = ObrabotcaURAN.Obrabotca.Dneutron(maxAmp, classUserSetUp.PorogSN, out d);
                      //  Debug.WriteLine("Es3");
                        firstTimeN = ObrabotcaURAN.Obrabotca.FirstTme(maxTime, maxAmp, classUserSetUp.PorogS, data1, Nul, ref PolovmaxTime, out Amp34, out Amp14);
                        sumSig = ColSigVatias(data1, Nul);
                       sumBB= ColBin(data1, Nul);

                        for (int i = 0; i < 12; i++)
                        {
                            int x = 0;
                            for (int j = 350; j < 1024; j++)
                            {
                                // Debug.WriteLine(data1[i, j].ToString());
                                //Debug.WriteLine(data1[i, j].ToString() +"-"+ Nul[i].ToString());
                                // data1S[i, j] = Convert.ToDouble(data1[i, j]) - Nul[i];
                                int AN = data1[i, j] - Convert.ToInt32(Nul[i]);
                                if (AN > 5)
                                {
                                    sumDetQ[i] += AN;
                                }

                            }
                            
                        }
                      //  ParserBAAK12.ParseBinFileBAAK12.SumSig(data1S, out sumDetQ);
                      // Debug.WriteLine(sumDetQ[0].ToString());
                        break;
                    default:

                        break;
                }

                timeS = ParserBAAK12.ParseBinFileBAAK12.TimeS(data1, classUserSetUp.PorogS, Amp, Nul);
            }

            catch (Exception ex)
            {

            }

            string[] array = nameFile.Split('_');
            string nameFileNew = String.Empty;
            string nameKlNew = String.Empty;
            if (array.Length > 2)
            {
                nameFileNew = nameFile;
                nameKlNew = array[0];
            }
            if (array.Length == 2)
            {
                nameFileNew = array[0].Split("№")[1] + "_" + array[1] + "_" + "T";
                nameKlNew = array[0].Split("№")[1];
            }

            if (!bad)
            {
                switch (tipN)
                {
                    
                    case "N":
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
     () => {
         ViewModel.ClassSobsN.Add(new ClassSobN()
         {
             nameFile = nameFile,
             nameklaster = array[0],
             nameBAAK = nemeBAAK,
             time = time1,
             Amp0 = Convert.ToInt16(Amp[0]),
             Amp1 = Convert.ToInt16(Amp[1]),
             Amp2 = Convert.ToInt16(Amp[2]),
             Amp3 = Convert.ToInt16(Amp[3]),
             Amp4 = Convert.ToInt16(Amp[4]),
             Amp5 = Convert.ToInt16(Amp[5]),
             Amp6 = Convert.ToInt16(Amp[6]),
             Amp7 = Convert.ToInt16(Amp[7]),
             Amp8 = Convert.ToInt16(Amp[8]),
             Amp9 = Convert.ToInt16(Amp[9]),
             Amp10 = Convert.ToInt16(Amp[10]),
             Amp11 = Convert.ToInt16(Amp[11]),

             TimeS0 = timeS[0].ToString(),
             TimeS1 = timeS[1].ToString(),
             TimeS2 = timeS[2].ToString(),
             TimeS3 = timeS[3].ToString(),
             TimeS4 = timeS[4].ToString(),
             TimeS5 = timeS[5].ToString(),
             TimeS6 = timeS[6].ToString(),
             TimeS7 = timeS[7].ToString(),
             TimeS8 = timeS[8].ToString(),
             TimeS9 = timeS[9].ToString(),
             TimeS10 = timeS[10].ToString(),
             TimeS11 = timeS[11].ToString(),
            
             sig0 = sig[0],
             sig1 = sig[1],
             sig2 = sig[2],
             sig3 = sig[3],
             sig4 = sig[4],
             sig5 = sig[5],
             sig6 = sig[6],
             sig7 = sig[7],
             sig8 = sig[8],
             sig9 = sig[9],
             sig10 = sig[10],
             sig11 = sig[11],
             SumAmp = Convert.ToInt32(Amp.Sum()),


             QS0 = sumDetQ[0],
             QS1 = sumDetQ[1],
             QS2 = sumDetQ[2],
             QS3 = sumDetQ[3],
             QS4 = sumDetQ[4],
             QS5 = sumDetQ[5],
             QS6 = sumDetQ[6],
             QS7 = sumDetQ[7],
             QS8 = sumDetQ[8],
             QS9 = sumDetQ[9],
             QS10 = sumDetQ[10],
             QS11 = sumDetQ[11]

         });
         ViewModel.CountObrabSob++;

     });
                        break;
                    case "V":
                        try
                        {
                            List<ChNeutron> chNeutrons = new List<ChNeutron>();
                            for (int i = 0; i < 12; i++)
                            {
                                chNeutrons.Add(new ChNeutron()
                                {
                                    ChN = i + 1,
                                    Amp = Convert.ToInt16(maxAmp[i]),
                                    FirstTimeV = firstTimeN[i],
                                    MaxTime = maxTime[i],
                                    Nnull = Convert.ToInt16(Nul[i]),
                                    PolmaxTimeV = PolovmaxTime[i],
                                    sig = sig[i], 
                                    Amp34= Convert.ToInt16(Amp34[i]),
                                    Amp14= Convert.ToInt16(Amp14[i])
                                  
                                });
                            }


                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
        () =>
        {

            ViewModel.ClassSobsV.Add(new ClassSobV()
            {
                chNeutrons = chNeutrons,
                nameFile = nameFile,
                nameklaster = array[0],
                nameBAAK = nemeBAAK,
                time = time1,
                Amp0 = Convert.ToInt16(maxAmp[0]),
                Amp1 = Convert.ToInt16(maxAmp[1]),
                Amp2 = Convert.ToInt16(maxAmp[2]),
                Amp3 = Convert.ToInt16(maxAmp[3]),
                Amp4 = Convert.ToInt16(maxAmp[4]),
                Amp5 = Convert.ToInt16(maxAmp[5]),
                Amp6 = Convert.ToInt16(maxAmp[6]),
                Amp7 = Convert.ToInt16(maxAmp[7]),
                Amp8 = Convert.ToInt16(maxAmp[8]),
                Amp9 = Convert.ToInt16(maxAmp[9]),
                Amp10 = Convert.ToInt16(maxAmp[10]),
                Amp11 = Convert.ToInt16(maxAmp[11]),
                maxTimeV = maxTime,
                PolmaxTimeV = PolovmaxTime,

                sig0 = sig[0],
                sig1 = sig[1],
                sig2 = sig[2],
                sig3 = sig[3],
                sig4 = sig[4],
                sig5 = sig[5],
                sig6 = sig[6],
                sig7 = sig[7],
                sig8 = sig[8],
                sig9 = sig[9],
                sig10 = sig[10],
                sig11 = sig[11],

                Nnull0 = Convert.ToInt16(Nul[0]),
                Nnull1 = Convert.ToInt16(Nul[1]),
                Nnull2 = Convert.ToInt16(Nul[2]),
                Nnull3 = Convert.ToInt16(Nul[3]),
                Nnull4 = Convert.ToInt16(Nul[4]),
                Nnull5 = Convert.ToInt16(Nul[5]),
                Nnull6 = Convert.ToInt16(Nul[6]),
                Nnull7 = Convert.ToInt16(Nul[7]),
                Nnull8 = Convert.ToInt16(Nul[8]),
                Nnull9 = Convert.ToInt16(Nul[9]),
                Nnull10 = Convert.ToInt16(Nul[10]),
                Nnull11 = Convert.ToInt16(Nul[11]),
                AmpNV = maxAmp[d - 1],
                nV = d,
                FirstTimeV = firstTimeN,
                MaxTime = maxTime[d - 1],
                QS0 = sumDetQ[0],
                QS1 = sumDetQ[1],
                QS2 = sumDetQ[2],
                QS3 = sumDetQ[3],
                QS4 = sumDetQ[4],
                QS5 = sumDetQ[5],
                QS6 = sumDetQ[6],
                QS7 = sumDetQ[7],
                QS8 = sumDetQ[8],
                QS9 = sumDetQ[9],
                QS10 = sumDetQ[10],
                QS11 = sumDetQ[11],
                sumsig = sumSig,
                sumBin = sumBB



            }) ;
            ViewModel.CountObrabSob++;

        });
                        }
                        catch (Exception ex)
                        {
                            MessageDialog messageDialog = new MessageDialog(ex.ToString());
                            await messageDialog.ShowAsync();
                        }
                        break;
                    default:

                        break;
                }




            }
            else
            {

                switch (tipN)
                {
                   
                    case "N":
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
     () => {
         ViewModel._DataColecSobPloxN.Add(new ClassSobN()
         {
             nameFile = nameFile,
             nameklaster = array[0],
             nameBAAK = nemeBAAK,
             time = time1,
             Amp0 = Convert.ToInt16(Amp[0]),
             Amp1 = Convert.ToInt16(Amp[1]),
             Amp2 = Convert.ToInt16(Amp[2]),
             Amp3 = Convert.ToInt16(Amp[3]),
             Amp4 = Convert.ToInt16(Amp[4]),
             Amp5 = Convert.ToInt16(Amp[5]),
             Amp6 = Convert.ToInt16(Amp[6]),
             Amp7 = Convert.ToInt16(Amp[7]),
             Amp8 = Convert.ToInt16(Amp[8]),
             Amp9 = Convert.ToInt16(Amp[9]),
             Amp10 = Convert.ToInt16(Amp[10]),
             Amp11 = Convert.ToInt16(Amp[11]),

             TimeS0 = timeS[0].ToString(),
             TimeS1 = timeS[1].ToString(),
             TimeS2 = timeS[2].ToString(),
             TimeS3 = timeS[3].ToString(),
             TimeS4 = timeS[4].ToString(),
             TimeS5 = timeS[5].ToString(),
             TimeS6 = timeS[6].ToString(),
             TimeS7 = timeS[7].ToString(),
             TimeS8 = timeS[8].ToString(),
             TimeS9 = timeS[9].ToString(),
             TimeS10 = timeS[10].ToString(),
             TimeS11 = timeS[11].ToString(),
             sig0 = sig[0],
             sig1 = sig[1],
             sig2 = sig[2],
             sig3 = sig[3],
             sig4 = sig[4],
             sig5 = sig[5],
             sig6 = sig[6],
             sig7 = sig[7],
             sig8 = sig[8],
             sig9 = sig[9],
             sig10 = sig[10],
             sig11 = sig[11],
             SumAmp = Convert.ToInt32(Amp.Sum()),

             QS0 = sumDetQ[0],
             QS1 = sumDetQ[1],
             QS2 = sumDetQ[2],
             QS3 = sumDetQ[3],
             QS4 = sumDetQ[4],
             QS5 = sumDetQ[5],
             QS6 = sumDetQ[6],
             QS7 = sumDetQ[7],
             QS8 = sumDetQ[8],
             QS9 = sumDetQ[9],
             QS10 = sumDetQ[10],
             QS11 = sumDetQ[11]

         });

     });
                        break;
                    case "V":
                        ViewModel._DataColecSobPloxV.Add(new ClassSobV()
                        {
                            nameFile = nameFile,
                            nameklaster = array[0],
                            nameBAAK = nemeBAAK,
                            time = time1,
                            Amp0 = Convert.ToInt16(maxAmp[0]),
                            Amp1 = Convert.ToInt16(maxAmp[1]),
                            Amp2 = Convert.ToInt16(maxAmp[2]),
                            Amp3 = Convert.ToInt16(maxAmp[3]),
                            Amp4 = Convert.ToInt16(maxAmp[4]),
                            Amp5 = Convert.ToInt16(maxAmp[5]),
                            Amp6 = Convert.ToInt16(maxAmp[6]),
                            Amp7 = Convert.ToInt16(maxAmp[7]),
                            Amp8 = Convert.ToInt16(maxAmp[8]),
                            Amp9 = Convert.ToInt16(maxAmp[9]),
                            Amp10 = Convert.ToInt16(maxAmp[10]),
                            Amp11 = Convert.ToInt16(maxAmp[11]),
                            PolmaxTimeV = PolovmaxTime,

                            sig0 = sig[0],
                            sig1 = sig[1],
                            sig2 = sig[2],
                            sig3 = sig[3],
                            sig4 = sig[4],
                            sig5 = sig[5],
                            sig6 = sig[6],
                            sig7 = sig[7],
                            sig8 = sig[8],
                            sig9 = sig[9],
                            sig10 = sig[10],
                            sig11 = sig[11],

                            Nnull0 = Convert.ToInt16(Nul[0]),
                            Nnull1 = Convert.ToInt16(Nul[1]),
                            Nnull2 = Convert.ToInt16(Nul[2]),
                            Nnull3 = Convert.ToInt16(Nul[3]),
                            Nnull4 = Convert.ToInt16(Nul[4]),
                            Nnull5 = Convert.ToInt16(Nul[5]),
                            Nnull6 = Convert.ToInt16(Nul[6]),
                            Nnull7 = Convert.ToInt16(Nul[7]),
                            Nnull8 = Convert.ToInt16(Nul[8]),
                            Nnull9 = Convert.ToInt16(Nul[9]),
                            Nnull10 = Convert.ToInt16(Nul[10]),
                            Nnull11 = Convert.ToInt16(Nul[11]),
                            AmpNV = maxAmp[d],
                            nV = d,
                            FirstTimeV = firstTimeN,
                            MaxTime = maxTime[d],
                            QS0 = sumDetQ[0],
                            QS1 = sumDetQ[1],
                            QS2 = sumDetQ[2],
                            QS3 = sumDetQ[3],
                            QS4 = sumDetQ[4],
                            QS5 = sumDetQ[5],
                            QS6 = sumDetQ[6],
                            QS7 = sumDetQ[7],
                            QS8 = sumDetQ[8],
                            QS9 = sumDetQ[9],
                            QS10 = sumDetQ[10],
                            QS11 = sumDetQ[11]



                        });
                        break;
                    default:

                        break;
                }
            }



        }



        /*Загрузка файлов и выбор файлов*/
        /// <summary>
        /// Загружает пакеты из файлов в очередб на обработку
        /// </summary>
        /// <param name="nBaaK"></param>
        /// <param name="leng"></param>
        /// <param name="nameRan"></param>
        /// <param name="listt"></param>
        /// <param name="cancellationToken"></param>
        private async void ZapicOcheredNaObrabotkyAsync(string nBaaK, int leng, string nameRan, List<ClassСписокList> listt, CancellationToken cancellationToken)
        {

            int ccc = 0;
            int[] masNul = new int[12];
            for (int i = 0; i < 12; i++)
            {
                masNul[i] = 2058;
            }
            uint numBytesLoaded = 1024;
            uint numBytesLoaded1 = 504648;
        
            foreach (ClassСписокList d in listt)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(50);

                    break;
                }
          


                string tipN = "T";
                // tipN = d.file1.DisplayName.Split('_')[2];
                if(d.file1.DisplayName.Contains("Test"))
                {
                    tipN = "T";

                }
                else
                {
                    string[] tipParser = d.file1.DisplayName.Split('_');
                    if (tipParser.Length > 2)
                    {
                        tipN = tipParser[2];
                    }
                    else
                    {

                    }
                }
               if(tipN=="Т")
                {
                    numBytesLoaded1 = 504648;
                }
               else
                {

                    // numBytesLoaded1 = 147624;
                    numBytesLoaded1 = 24603;


                }


                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    d.Status = true;
});

               
                    try
                    {
                        var stream = await d.file1.OpenAsync(Windows.Storage.FileAccessMode.Read);
                        //  ulong size = stream.Size;

                        bool end = false;
                        uint kol = 0;
                        List<byte> dataOnePac = new List<byte>();

                        int tecpos = 0;
                        int countFlagEnt = 0;
                        int pac = 0;
                        while (!end)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                Thread.Sleep(50);

                                break;
                            }
                            using (var inputStream = stream.GetInputStreamAt(kol * numBytesLoaded1))
                            {
                                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                                {
                                    numBytesLoaded = await dataReader.LoadAsync(numBytesLoaded1);
                                    if( numBytesLoaded != 0)
                                    {
                                          if (cancellationToken.IsCancellationRequested)
                                    {
                                        Thread.Sleep(50);

                                        break;
                                    }
                                          byte[] dd = new byte[numBytesLoaded];
                                          dataReader.ReadBytes(dd);
                                          if (dd[numBytesLoaded - 1] == 0xFF && dd[numBytesLoaded - 2] == 0xFF && dd[numBytesLoaded - 3] == 0xFF && dd[numBytesLoaded - 4] == 0xFF)
                                    {

                                        int cc1 = Count();
                                        if (cc1 > 1800)
                                        {
                                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusP = true;
if (cancellationToken.IsCancellationRequested)
{
    Thread.Sleep(50);
}
});
                                            Thread.Sleep(10000);
                                            long totalMemory = GC.GetTotalMemory(false);
                                            GC.Collect();
                                            GC.WaitForPendingFinalizers();

                                            while (Count() > 500)
                                            {
                                                if (cancellationToken.IsCancellationRequested)
                                                {
                                                    Thread.Sleep(50);

                                                    break;
                                                }
                                                Thread.Sleep(5000);
                                            }
                                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusP = true;
});
                                        }
                                        OcherediNaObrab.Enqueue(new MyclasDataizFile { NameFile = d.file1.DisplayName, Buf00 = dd, LenghtChenel = leng, НулеваяЛиния = masNul, NameBaaR12 = nBaaK.ToString(), Ran = nameRan, tipName = tipN });
                                        pac++;
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { d.StatusSize += (ulong)dd.Length; ViewModel.CountNaObrabZ++; });


                                    }
                                          else
                                       {


                                        if ((dd[numBytesLoaded - 1] == 0xFE && dd[numBytesLoaded - 2] == 0xFE && dd[numBytesLoaded - 3] == 0xFE && dd[numBytesLoaded - 4] == 0xFE) || (numBytesLoaded > 502640 && numBytesLoaded < 504648))
                                        {

                                        }
                                        else
                                        {
                                            for (int i = 0; i < numBytesLoaded; i++)
                                            {
                                                if (cancellationToken.IsCancellationRequested)
                                                {
                                                    Thread.Sleep(50);

                                                    break;
                                                }
                                                int cc1 = Count();
                                                if (cc1 > 1800)
                                                {
                                                    if (cancellationToken.IsCancellationRequested)
                                                    {
                                                        Thread.Sleep(50);

                                                        break;
                                                    }
                                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    d.StatusP = true;
});
                                                    Thread.Sleep(10000);
                                                    long totalMemory = GC.GetTotalMemory(false);
                                                    GC.Collect();
                                                    GC.WaitForPendingFinalizers();
                                                    // for(int f=0; f<10000; f++)
                                                    //  {
                                                    //      int xx = 0;
                                                    //  }
                                                    //Thread.Sleep(20000);
                                                    while (Count() > 500)
                                                    {
                                                        if (cancellationToken.IsCancellationRequested)
                                                        {
                                                            Thread.Sleep(50);

                                                            break;
                                                        }
                                                        Thread.Sleep(5000);
                                                    }
                                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    d.StatusP = true;
});
                                                }



                                                var b = dd[i];

                                                dataOnePac.Add(b);
                                                tecpos++;
                                                if (b == 0xFF)
                                                {
                                                    countFlagEnt++;
                                                    if (countFlagEnt == 4)
                                                    {

                                                        if (tecpos > 502640 && tecpos < 504648)
                                                        {

                                                            dataOnePac = null;
                                                        }
                                                        else
                                                        {


                                                            OcherediNaObrab.Enqueue(new MyclasDataizFile { NameFile = d.file1.DisplayName, Buf00 = dataOnePac.ToArray(), LenghtChenel = leng, НулеваяЛиния = masNul, NameBaaR12 = nBaaK.ToString(), Ran = nameRan, tipName = tipN });
                                                        }
                                                        pac++;

                                                        dataOnePac = new List<byte>();
                                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
    () =>
    {
        d.StatusSize += 504648;
        ViewModel.CountNaObrabZ++;
    });
                                                        //Thread.Sleep(1000);
                                                        countFlagEnt = 0;
                                                        tecpos = 0;

                                                    }
                                                }
                                                else
                                                {
                                                    countFlagEnt = 0;
                                                }

                                            }
                                        }



                                    }
                                    }
                                    else
                                    {
                                       end = true;
                                    }

                                }
                            }
                            kol++;
                        }
                        stream.Dispose();
                    }
                    catch
                    {
                        // var  messageDialog = new MessageDialog("ошибка открытия файла" + d.file1.Path +"   ");
                        // await messageDialog.ShowAsync();
                    }


                
         

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    d.Status = false;
    d.StatusSize = 0;
});
            }

            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();


            }


        }
        private async void ZapicOcheredNaObrabotkyAsync(string nBaaK, int leng, string nameRan, List<ClassСписокList> listt, CancellationToken cancellationToken, bool tt)
        {

           
            int[] masNul = new int[12];
            for (int i = 0; i < 12; i++)
            {
                masNul[i] = 2058;
            }
            uint numBytesLoaded = 1024;
            uint numBytesLoaded1 = 504648;
            int x = (from isEs in listt.AsParallel() where isEs.sizeF > 500 select isEs).Count();
      
            if (x > 0)
            {
                foreach (ClassСписокList d in listt)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Thread.Sleep(50);

                        break;
                    }



                    string tipN = "T";
                    // tipN = d.file1.DisplayName.Split('_')[2];
                    if (d.file1.DisplayName.Contains("Test"))
                    {
                        tipN = "T";

                    }
                    else
                    {
                        string[] tipParser = d.file1.DisplayName.Split('_');
                        if (tipParser.Length > 2)
                        {
                            tipN = tipParser[2];
                        }
                        else
                        {

                        }
                    }



                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
    () =>
    {
        d.Status = true;
    });


                    try
                    {
                        var stream = await d.file1.OpenAsync(Windows.Storage.FileAccessMode.Read);
                        //  ulong size = stream.Size;

                        bool end = false;
                        uint kol = 0;
                        List<byte> dataOnePac = new List<byte>();

                        int tecpos = 0;
                        int countFlagEnt = 0;
                        int pac = 0;
                        while (!end)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                Thread.Sleep(50);

                                break;
                            }
                            using (var inputStream = stream.GetInputStreamAt(kol * numBytesLoaded1))
                            {

                                using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                                {
                                    numBytesLoaded = await dataReader.LoadAsync(numBytesLoaded1);
                                    //  if (numBytesLoaded < numBytesLoaded1 || numBytesLoaded == 0)
                                    if ( numBytesLoaded == 0)
                                    {
                                         end = true;
                                        break;
                                    }

                                    byte[] dd = new byte[numBytesLoaded];
                                    dataReader.ReadBytes(dd);
                                    if (dd[numBytesLoaded - 1] == 0xFF && dd[numBytesLoaded - 2] == 0xFF && dd[numBytesLoaded - 3] == 0xFF && dd[numBytesLoaded - 4] == 0xFF)
                                    {

                                        int cc1 = Count();
                                        if (cc1 > 1800)
                                        {
                                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    d.StatusP = true;
    if (cancellationToken.IsCancellationRequested)
    {
        Thread.Sleep(50);

        //break;
    }
});
                                            Thread.Sleep(10000);
                                            long totalMemory = GC.GetTotalMemory(false);
                                            GC.Collect();
                                            GC.WaitForPendingFinalizers();
                                            // for(int f=0; f<10000; f++)
                                            //  {
                                            //      int xx = 0;
                                            //  }
                                            //Thread.Sleep(20000);
                                            while (Count() > 500)
                                            {
                                                if (cancellationToken.IsCancellationRequested)
                                                {
                                                    Thread.Sleep(50);

                                                    break;
                                                }
                                                Thread.Sleep(5000);
                                            }
                                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
    d.StatusP = true;
});
                                        }

                                        OcherediNaObrab.Enqueue(new MyclasDataizFile { NameFile = d.file1.DisplayName, Buf00 = dd, LenghtChenel = leng, НулеваяЛиния = masNul, NameBaaR12 = nBaaK.ToString(), Ran = nameRan, tipName = tipN });
                                        pac++;
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { d.StatusSize += (ulong)dd.Length; ViewModel.CountNaObrabZ++; });
                                        kol++;
                                        continue;
                                    }
                                    if ((dd[numBytesLoaded - 1] == 0xFE && dd[numBytesLoaded - 2] == 0xFE && dd[numBytesLoaded - 3] == 0xFE && dd[numBytesLoaded - 4] == 0xFE) || (numBytesLoaded > 502640 && numBytesLoaded < 504648))
                                    {
                                        kol++;
                                        continue;

                                    }
                                    for (int i = 0; i < numBytesLoaded; i++)
                                    {

                                        //  if (cancellationToken.IsCancellationRequested)
                                        //  {
                                        //      Thread.Sleep(50);

                                        //     break;
                                        // }
                                        /* int cc1 = Count();
                                         if (cc1 > 1800)
                                         {
                                             if (cancellationToken.IsCancellationRequested)
                                             {
                                                 Thread.Sleep(50);

                                                 break;
                                             }
                                             await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusP = true;
});
                                             Thread.Sleep(10000);
                                             long totalMemory = GC.GetTotalMemory(false);
                                             GC.Collect();
                                             GC.WaitForPendingFinalizers();
                                             // for(int f=0; f<10000; f++)
                                             //  {
                                             //      int xx = 0;
                                             //  }
                                             //Thread.Sleep(20000);
                                             while (Count() > 500)
                                             {
                                                 if (cancellationToken.IsCancellationRequested)
                                                 {
                                                     Thread.Sleep(50);

                                                     break;
                                                 }
                                                 Thread.Sleep(5000);
                                             }
                                             await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusP = true;
});
                                         }
                                         */


                                        var b = dd[i];

                                        dataOnePac.Add(b);
                                        tecpos++;
                                        if (b == 0xFF)
                                        {
                                            countFlagEnt++;
                                            if (countFlagEnt == 4)
                                            {

                                                if (tecpos > 502640 && tecpos < 504648)
                                                {

                                                    dataOnePac = null;
                                                }
                                                else
                                                {


                                                    OcherediNaObrab.Enqueue(new MyclasDataizFile { NameFile = d.file1.DisplayName, Buf00 = dataOnePac.ToArray(), LenghtChenel = leng, НулеваяЛиния = masNul, NameBaaR12 = nBaaK.ToString(), Ran = nameRan, tipName = tipN });
                                                }
                                                pac++;

                                                dataOnePac = new List<byte>();
                                                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusSize += 504648;
ViewModel.CountNaObrabZ++;
});
                                                //Thread.Sleep(1000);
                                                countFlagEnt = 0;
                                                tecpos = 0;

                                            }
                                        }
                                        else
                                        {
                                            countFlagEnt = 0;
                                        }

                                    }
                                }
                            }
                            kol++;
                        }
                        stream.Dispose();
                    }
                    catch
                    {
                        // var  messageDialog = new MessageDialog("ошибка открытия файла" + d.file1.Path +"   ");
                        // await messageDialog.ShowAsync();
                    }





                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
    () =>
    {
        d.Status = false;
    });
                }
            }
            else
            {
                foreach (ClassСписокList d in listt)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Thread.Sleep(50);

                        break;
                    }



                    string tipN = "T";
                 
                    // tipN = d.file1.DisplayName.Split('_')[2];
                    if (d.file1.DisplayName.Contains("Test"))
                    {
                        tipN = "T";

                    }
                    else
                    {
                        string[] tipParser = d.file1.DisplayName.Split('_');
                        if (tipParser.Length > 2)
                        {
                            tipN = tipParser[2];
                        }
                        else
                        {

                        }
                    }


              
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,() =>
    {
        d.Status = true;
    });


                    try
                    {
                       
                        var buffer = await Windows.Storage.FileIO.ReadBufferAsync(d.file1);
                        //  ulong size = stream.Size;
                     
                        bool end = false;
                        uint kol = 0;
                        List<byte> dataOnePac = new List<byte>();
                        int tecpos = 0;
                        int countFlagEnt = 0;
                        int pac = 0;
                        using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
                        {
                            byte[] dd1 = new byte[buffer.Length];
                        
                            dataReader.ReadBytes(dd1);
                            while (!end)
                            {
                              
                            
                                //if (buffer.Length < numBytesLoaded || buffer.Length == 0)
                                if (buffer.Length == 0)
                                {
                                    // end = true;
                                    break;
                                }
                                byte[] dd = new byte[numBytesLoaded1];
                                Array.Copy(dd1, kol * numBytesLoaded1, dd, 0, numBytesLoaded1);
                                uint pozEnd = numBytesLoaded1 * kol;
                                if (dd[numBytesLoaded1 - 1] == 0xFF && dd[numBytesLoaded1 - 2] == 0xFF && dd[numBytesLoaded1 - 3] == 0xFF && dd[numBytesLoaded1 - 4] == 0xFF)
                                {
                                    
                             
                                    int cc1 = Count();
                                    if (cc1 > 1800)
                                    {
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusP = true;
if (cancellationToken.IsCancellationRequested)
{
    Thread.Sleep(50);

        //break;
    }
});
                                        Thread.Sleep(10000);
                                        long totalMemory = GC.GetTotalMemory(false);
                                        GC.Collect();
                                        GC.WaitForPendingFinalizers();
                                        // for(int f=0; f<10000; f++)
                                        //  {
                                        //      int xx = 0;
                                        //  }
                                        //Thread.Sleep(20000);
                                        while (Count() > 500)
                                        {
                                            if (cancellationToken.IsCancellationRequested)
                                            {
                                                Thread.Sleep(50);

                                                break;
                                            }
                                            Thread.Sleep(5000);
                                        }
                                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusP = true;
});
                                    }

                                    OcherediNaObrab.Enqueue(new MyclasDataizFile { NameFile = d.file1.DisplayName, Buf00 = dd, LenghtChenel = leng, НулеваяЛиния = masNul, NameBaaR12 = nBaaK.ToString(), Ran = nameRan, tipName = tipN });
                                    pac++;
                                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { d.StatusSize += (ulong)dd.Length; ViewModel.CountNaObrabZ++; });
                                    kol++;
                                    continue;
                                }
                                if ((dd[numBytesLoaded - 1] == 0xFE && dd[numBytesLoaded - 2] == 0xFE && dd[numBytesLoaded - 3] == 0xFE && dd[numBytesLoaded - 4] == 0xFE) || (numBytesLoaded > 502640 && numBytesLoaded < 504648))
                                {
                                    
                                    kol++;
                                    continue;

                                }
                                for (int i = (int)pozEnd; i < dd1.Length; i++)
                                {
                              
                                      if (cancellationToken.IsCancellationRequested)
                                      {
                                          Thread.Sleep(50);

                                         break;
                                     }
                                     int cc1 = Count();
                                     if (cc1 > 1800)
                                     {
                                         if (cancellationToken.IsCancellationRequested)
                                         {
                                             Thread.Sleep(50);

                                             break;
                                         }
                                         await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusP = true;
});
                                         Thread.Sleep(10000);
                                         long totalMemory = GC.GetTotalMemory(false);
                                         GC.Collect();
                                         GC.WaitForPendingFinalizers();
                                         // for(int f=0; f<10000; f++)
                                         //  {
                                         //      int xx = 0;
                                         //  }
                                         //Thread.Sleep(20000);
                                         while (Count() > 500)
                                         {
                                             if (cancellationToken.IsCancellationRequested)
                                             {
                                                 Thread.Sleep(50);

                                                 break;
                                             }
                                             Thread.Sleep(5000);
                                         }
                                         await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusP = true;
});
                                     }
                                     
                                    var b = dd[i];

                                    dataOnePac.Add(b);
                                    tecpos++;
                                    if (b == 0xFF)
                                    {
                                        countFlagEnt++;
                                        if (countFlagEnt == 4)
                                        {

                                            if (tecpos > 502640 && tecpos < 504648)
                                            {

                                                dataOnePac = null;
                                            }
                                            else
                                            {
                                                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { d.StatusSize += (ulong)dd.Length; ViewModel.CountNaObrabZ++; });
                                                OcherediNaObrab.Enqueue(new MyclasDataizFile { NameFile = d.file1.DisplayName, Buf00 = dataOnePac.ToArray(), LenghtChenel = leng, НулеваяЛиния = masNul, NameBaaR12 = nBaaK.ToString(), Ran = nameRan, tipName = tipN });
                                            }
                                            pac++;

                                            dataOnePac = new List<byte>();
                                            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
() =>
{
d.StatusSize += 504648;
ViewModel.CountNaObrabZ++;
});
                                            //Thread.Sleep(1000);
                                            countFlagEnt = 0;
                                            tecpos = 0;

                                        }
                                    }
                                    else
                                    {
                                        countFlagEnt = 0;
                                    }

                                }


                            }


                        }
                       
                       
                    }
                    catch
                    {
                        // var  messageDialog = new MessageDialog("ошибка открытия файла" + d.file1.Path +"   ");
                        // await messageDialog.ShowAsync();
                    }





                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
    () =>
    {
        d.Status = false;
        d.StatusSize = 0;
    });
                }
            }
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();


            }


        }
      
    }
}
