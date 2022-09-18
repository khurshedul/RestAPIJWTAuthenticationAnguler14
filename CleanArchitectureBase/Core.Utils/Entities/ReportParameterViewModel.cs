using Core.Utils.Entities;
using System;
using System.Collections.Generic;
namespace Core.Utils.Entities
{
    public class ReportParameterViewModel : BaseViewModel
    {
        public string WorkstationNo { get; set; }
        public DateTime? WorkstationSystemDate { get; set; }
        public string AccountNo { get; set; }
        public string BranchName { get; set; }
        public string AreaCode { get; set; }
        public string RegionCode { get; set; }
        public string ZoneCode { get; set; }
        public string CenterDay { get; set; }
        public string MemberName { get; set; }
        public string OperationType { get; set; }
        public string TransactionNo { get; set; }
        public string LoanProductCode { get; set; }
        public string DepositProductCode { get; set; }
        public string WorkStationType { get; set; }
        public decimal CashReceive { get; set; }
        public decimal CashPayment { get; set; }
        public decimal BankReceive { get; set; }
        public string SystemDate { get; set; }
        public string CollectionStatus { get; set; }
        public string OperationTypeforBoth { get; set; }
        public string Status { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string ODClass { get; set; }
        public string ParentAccountNo { get; set; }
        public string LastClosedDate { get; set; }
        public DateTime? LastCloseDay { get; set; }
        public DateTime? LastOpenDay { get; set; }
        public string WorkStation { get; set; }
        public string ViewBy { get; set; }

    }


}
