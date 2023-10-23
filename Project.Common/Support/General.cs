using System.ComponentModel.DataAnnotations;

namespace Project.Common.Support
{
    public class ExpressionFilter
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        //public string Value { get; set; }
        public Comparison Comparison { get; set; }
    }

    public enum Comparison
    {
        [Display(Name = "==")]
        Equal,

        [Display(Name = "<")]
        LessThan,

        [Display(Name = "<=")]
        LessThanOrEqual,

        [Display(Name = ">")]
        GreaterThan,

        [Display(Name = ">=")]
        GreaterThanOrEqual,

        [Display(Name = "!=")]
        NotEqual,

        [Display(Name = "Contains")]
        Contains, //for strings  

        [Display(Name = "StartsWith")]
        StartsWith, //for strings  

        [Display(Name = "EndsWith")]
        EndsWith, //for strings  

        [Display(Name = "Range")]
        InRange
    }
    public class GridData<T>
    {
        public int Total { get; set; }
        public int FirstItemOnPage { get; set; }
        public List<T> Rows { get; set; }

        //public int PageCount { get; protected set; }
        //public int PageNumber { get; protected set; }

        //public int PageSize { get; protected set; }

        //public bool HasPreviousPage { get; protected set; }

        //public bool HasNextPage { get; protected set; }

        //public bool IsFirstPage { get; protected set; }

        //public bool IsLastPage { get; protected set; }
        //public int LastItemOnPage { get; protected set; }
    }

    public class DateFormat
    {
        public string value { get; set; }
        public string CSDateFormat { get; set; }
        public string CSDateTimeFormat { get; set; }
        public string MomentDateFormat { get; set; }
        public string MomentDateTimeFormat { get; set; }
    }

    public enum State
    {
        None,
        LowerA,
        CapitalA,
        LowerD1,
        LowerD2,
        LowerD3,
        LowerD4,
        CapitalD1,
        CapitalD2,
        LowerH1,
        LowerH2,
        CapitalH1,
        CapitalH2,
        LowerM1,
        LowerM2,
        CapitalM1,
        CapitalM2,
        CapitalM3,
        CapitalM4,
        LowerS1,
        LowerS2,
        CapitalS1,
        CapitalS2,
        CapitalS3,
        CapitalS4,
        CapitalS5,
        CapitalS6,
        CapitalS7,
        CapitalY1,
        CapitalY2,
        CapitalY3,
        CapitalY4,
        CapitalZ,
        LowerF1,
        LowerF2,
        LowerF3,
        LowerF4,
        LowerF5,
        LowerF6,
        LowerF7,
        CapitalF1,
        CapitalF2,
        CapitalF3,
        CapitalF4,
        CapitalF5,
        CapitalF6,
        CapitalF7,
        LowerG,
        CapitalK,
        LowerT1,
        LowerT2,
        LowerY1,
        LowerY2,
        LowerY3,
        LowerY4,
        LowerY5,
        LowerZ1,
        LowerZ2,
        LowerZ3,
        InSingleQuoteLiteral,
        InDoubleQuoteLiteral,
        EscapeSequence
    }
}
