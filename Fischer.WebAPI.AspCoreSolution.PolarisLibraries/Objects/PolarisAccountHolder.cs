using System;
using System.ComponentModel.DataAnnotations;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Interfaces;

namespace Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Objects
{
    public class PolarisAccountHolder : IPolarisAccountHolder
    {
        [Key]
        public int AccountId { get; set; }
        public Guid AccountGuid { get; set; }
        public string AccountHolder { get; set; }
        public string AccountType { get; set; }
    }
}
