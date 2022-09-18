namespace Core.Utils.Entities
{
    public class BaseViewModel
    {
        public int PageSize { get; set; } = 100000;
        public int PageNo { get; set; } = 1;
        public string FromDate { get; set; }
        public string ToDate { get; set; }

        //public string SESSION_ID { get; set; }
        //public string SESSION_TOKEN { get; set; }

        public string ProgramCode { get; set; }
        public string BranchCode { get; set; }
        public string CenterCode { get; set; }
        public string CenterNo { get; set; }
        public string MemberNo { get; set; }
        public string LeadNo { get; set; }

        public string SetBy { get; set; }
        public string EIN { get; set; }
        public string CenterDay { get; set; }
    }
}
