using System;

namespace Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Interfaces
{
    public interface IPolarisAccountHolder
    {
        public int AccountId { get; set; }
        public Guid AccountGuid { get; set; }
        public string AccountHolder { get; set; }
        public string AccountType { get; set; }
    }
}
