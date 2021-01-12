using System;
using System.ComponentModel.DataAnnotations;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Interfaces;

namespace Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Objects
{
    public class PolarisTransaction : IPolarisTransaction
    {
        [Key]
        public int TransactionId { get; set; }
        public Guid TransactionGuid { get; set; }
        public Guid AccountGuid { get; set; }
        public string TransactionType { get; set; }
        public decimal BeginningBalance { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Memo { get; set; }
        public decimal EndingBalance { get; set; }
    }
}
