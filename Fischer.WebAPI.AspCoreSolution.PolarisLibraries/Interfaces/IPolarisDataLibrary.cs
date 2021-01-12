using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Interfaces
{
    public interface IPolarisDataLibrary
    {
        public IPolarisAccountHolder GetSingleAccountInfoByGuid(string sqlCommandText);

        public Task<List<IPolarisAccountHolder>> GetAllAccounts(string sqlCommandText);

        public Task<List<IPolarisTransaction>> GetTransactionsByAccountGuid(string sqlCommandText);

        public void CreateAccountHolder();

        public void CreateTransactions(List<IPolarisTransaction> transactions);

        public bool DeleteAccountHolderByAccountGuid(string sqlCommandText);
    }
}
