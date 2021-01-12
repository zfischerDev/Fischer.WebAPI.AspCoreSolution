using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Fischer.WebAPI.AspCoreSolution.PolarisWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionProcessing : ControllerBase
    {
        private PolarisDataLibrary polarisLibrary;
        private PolarisEFDataLibrary polarisEfDataLibrary;
        private IConfiguration config;

        public TransactionProcessing(IConfiguration theConfiguration)
        {
            config = theConfiguration;
        }

        // GET api/<PolarisMain>/5
        [HttpGet("GetTransactionsByAccountGuid/{accountGuid}")]
        public async Task<ActionResult<List<IPolarisTransaction>>> GetTransactionsByAccountGuid(string accountGuid)
        {
            try
            {
                string connectionString = config.GetSection("ConnectionStrings:PolarisDatabase").Value;
                #region SQL Testing
                //string sqlCommandText = $"SELECT TransactionGuid,AccountGuid,TransactionType," +
                //                        $"BeginningBalance,TransactionDateTime,TransactionAmount," +
                //                        $"Memo,EndingBalance FROM PolarisTransactions WHERE AccountGuid='{accountGuid}'";

                //string commandTextString = "SELECT AccountGuid, AccountHolder, AccountType FROM PolarisAccounts";

                //polarisLibrary = new PolarisDataLibrary(connectionString, commandTextString);
                //List<IPolarisTransaction> transactions = await polarisLibrary.GetTransactionsByAccountGuid(sqlCommandText);
                #endregion

                #region EF Testing
                polarisEfDataLibrary = new PolarisEFDataLibrary(connectionString);
                List<IPolarisTransaction> transactions =
                    await polarisEfDataLibrary.GetTransactionsByAccountGuidEF(accountGuid);
                #endregion

                return transactions;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // DELETE api/<AccountProcessing>/5
        [HttpDelete("DeleteTransactionsByTransactionGuid/{transactionId}")]
        public void DeleteTransactionsByTransactionGuid(string transactionId)
        {
            try
            {
                string connectionString = config.GetSection("ConnectionStrings:PolarisDatabase").Value;
                #region EF Testing
                polarisEfDataLibrary = new PolarisEFDataLibrary(connectionString);
                bool isDeleted = polarisEfDataLibrary.DeleteTransactionByTransactionGuid(transactionId);
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE api/<AccountProcessing>/5
        [HttpDelete("DeleteAllTransactionsByAccountGuid/{accountId}")]
        public void DeleteAllTransactionsByAccountGuid(string accountId)
        {
            try
            {
                string connectionString = config.GetSection("ConnectionStrings:PolarisDatabase").Value;
                #region EF Testing
                polarisEfDataLibrary = new PolarisEFDataLibrary(connectionString);
                bool isDeleted = polarisEfDataLibrary.DeleteAllTransactionByAccountGuid(accountId);
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
