using IMDBImport.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBImport
{
    class TitleBasic
    {
        public string tconst { get; set; }
        public string titleType { get; set; }
        public string primaryTitle { get; set; }
        public string originalTitle { get; set; }
        public string isAdult { get; set; }
        public string startYear { get; set; }
        public string endYear { get; set; }
        public string runTimeMinutes { get; set; }

        public bool? isAdultNull
        {
            get
            {
                if (isAdult.ToLower() == "\\n")
                {
                    return null;
                }
                return isAdult == "1";
            }
        }
        public int? startYearNull
        {
            get
            {
                if (startYear.ToLower() == "\\n")
                {
                    return null;
                }
                return int.Parse(startYear);
            }
        }
        public int? endYearNull
        {
            get
            {
                if (endYear.ToLower() == "\\n")
                {
                    return null;
                }
                try { 
                return int.Parse(endYear);
                } catch (Exception ex)
                {
                    Console.WriteLine("endyear is: " + endYear);
                    throw ex;
                }
            }
        }
        public int? runTimeMinutesNull
        {
            get
            {
                if (runTimeMinutes.ToLower() == "\\n")
                {
                    return null;
                }
                return int.Parse(runTimeMinutes);
            }
        }


        public TitleBasic()
        {
        }

        public TitleBasic(string[] splitLine)
        {
            tconst = splitLine[0];
            titleType = splitLine[1];
            primaryTitle = splitLine[2];
            originalTitle = splitLine[3];
            isAdult = splitLine[4];
            startYear = splitLine[5];
            endYear = splitLine[6];
            runTimeMinutes = splitLine[7];
        }

        public TitlesBasic ToEFModel()
        {
            return new TitlesBasic() { Tconst = tconst, TitleType = titleType, PrimaryTitle = primaryTitle,
                                       OriginalTitle = originalTitle, IsAdult = isAdultNull, StartYear = startYearNull,
                                       EndYear = endYearNull, RuntimeMinutes = runTimeMinutesNull };
        }
    }
}
