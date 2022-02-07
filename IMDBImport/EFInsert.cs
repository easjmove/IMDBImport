using IMDBImport.EFModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBImport
{
    class EFInsert : IInsert
    {
        public void InsertData(SqlConnection sqlconn, List<TitleBasic> allTitles)
        {
            IMDBContext context = new IMDBContext();
            
            foreach (TitleBasic title in allTitles)
            {
                context.TitlesBasics.Add(title.ToEFModel());
            }
            context.SaveChanges();
        }
    }
}
