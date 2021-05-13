using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DataYRAN.Pasport
{
   public partial class BlankPagePasport
    {
      
        public List<RasAmp> AmpRas(List<ClassSob> classRazvertka1)
        {
            List<RasAmp> classRasAmp = new List<RasAmp>();
            foreach (ClassSob classSob in classRazvertka1)
            {
                
                for (int i = 0; i < 12; i++)
                {
                    var orderedNumbers1 = from ClassRasAmp in classRasAmp.AsParallel()
                                          where ClassRasAmp.znacRas == classSob.mAmp[i]
                                          select ClassRasAmp;
                    if (orderedNumbers1.Count() != 0)
                    {


                        foreach (RasAmp classRas in orderedNumbers1)
                        {
                            int[] f1 = classRas.MAmp;
                            f1[i] = f1[i] + 1;
                        }
                    }
                    else
                    {
                        int[] f1 = new int[12];
                        f1[i] = 1;
                        classRasAmp.Add(new RasAmp() { znacRas = classSob.mAmp[i], MAmp = f1 });
                    }




                    /*
                    if (classRasNuls.Count != 0)
                    {

                        int x = 0;
                       
                        foreach (ClassRasNul classRas in classRasNuls)
                        {
                            if(classRas.znacRas == classSob.mNull[i])
                            {
                                int[] f2 = classRas.MNullLine;
                                f2[i] = f2[i] + 1;


                                x++;

                            }
                         
                        }
                        if(x>0)
                        {
                            x = 0;
                        }
                        else
                        {
                            int[] f2 = new int[12];
                            f2[i] = 1;
                            classRasNuls.Add(new ClassRasNul() { znacRas = classSob.mNull[i], MNullLine = f2 });

                        }
                    }
                    else
                    {
                        int[] f3 = new int[12];
                        f3[0] = 1;
                        classRasNuls.Add(new ClassRasNul() { znacRas = classSob.mNull[i], MNullLine=f3  });
                    }
                    */
                }


            }
            //    var orderedNumbers = from ClassRasNul in classRasNuls
            //  orderby ClassRasNul.znacRas
            // select ClassRasNul;
            classRasAmp = (from ClassRasNul in classRasAmp
                                    orderby ClassRasNul.znacRas
                                    select ClassRasNul).ToList<RasAmp>();
            return classRasAmp;
            //DataColecN = orderedNumbersnew.ToList();
            //eutronGrid.ItemsSource = DataColecN;




        }
       
       
    }
}
