using System;
using System.Collections.Generic;

#nullable disable

namespace IMDBImport.EFModels
{
    public partial class TitlesBasic
    {
        public string Tconst { get; set; }
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool? IsAdult { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
        public int? RuntimeMinutes { get; set; }
    }
}
