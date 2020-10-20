using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataYRAN
{
    /// <summary>
    /// Определяет и хранит тип платы БААК12
    /// </summary>
    public class tipBAAK
    {
        tip tipp = tip.T;
        public tip Tip
        {
            get
            {
                return tipp;
            }
            set
            {
                tipp = value;
            }
           
        }
       


    }
   /// <summary>
   /// Перечисление типа плат(T-плата с хвостом 200 МГц, N - плата без хвоста 200МГц, V плата для вариации 100 МГц)
   /// </summary>
   public enum tip
    {
        V,
        T,
        N
  
    }

}
