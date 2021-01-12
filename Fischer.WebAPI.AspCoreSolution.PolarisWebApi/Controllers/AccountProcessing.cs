using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Interfaces;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Objects;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fischer.WebAPI.AspCoreSolution.PolarisWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountProcessing : ControllerBase
    {
        private PolarisDataLibrary polarisLibrary;
        private IConfiguration config;
        private PolarisEFDataLibrary polarisEfDataLibrary;

        public AccountProcessing(IConfiguration theConfiguration)
        {
            config = theConfiguration;
        }

        [HttpGet("GetAllAccounts")]
        public async Task<ActionResult<List<IPolarisAccountHolder>>> GetAllAccounts()
        {
            try
            {
                string connectionString = config.GetSection("ConnectionStrings:PolarisDatabase").Value;

                #region SQL Testing
                //string commandText = "SELECT AccountGuid, AccountHolder, AccountType FROM PolarisAccounts";
                //polarisLibrary = new PolarisDataLibrary(connectionString, commandText);
                #endregion

                #region EF Testing
                polarisEfDataLibrary = new PolarisEFDataLibrary(connectionString);
                List<IPolarisAccountHolder> accountInfoList = await polarisEfDataLibrary.GetAllAccounts();
                #endregion

                return accountInfoList;

            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET api/<AccountProcessing>/5
        [HttpGet("GetSingleAccountHolderByGuid/{accountGuid}")]
        public IPolarisAccountHolder GetSingleAccountHolderByGuid(string accountGuid)
        {
            try
            {
                string connectionString = config.GetSection("ConnectionStrings:PolarisDatabase").Value;
                #region SQL Testing
                //string commandText =
                //    $"SELECT AccountGuid, AccountHolder, AccountType FROM PolarisAccounts WHERE AccountGuid='{accountGuid}'";
                //polarisLibrary = new PolarisDataLibrary(connectionString, commandText);
                #endregion

                #region EF Testing
                polarisEfDataLibrary = new PolarisEFDataLibrary(connectionString);
                IPolarisAccountHolder accountHolder = polarisEfDataLibrary.GetSingleAccountByGuid(accountGuid);
                #endregion

                return accountHolder;
            }
            catch (Exception)
            {
                throw;
            }

        }

        // POST api/<PolarisApi>
        [HttpPost("CreateNewAccountHolder")]
        public void PostAccountHolder([FromBody] PolarisAccountHolder polarisAccountHolder)
        {
            try
            {
                string connectionString = config.GetSection("ConnectionStrings:PolarisDatabase").Value;
                #region SQL Testing
                //Guid accountHolderGuid = Guid.Parse(polarisAccountHolder.AccountGuid.ToString());
                //string accountHolder = polarisAccountHolder.AccountHolder;
                //string accountType = polarisAccountHolder.AccountType;
                //string commandText =
                //    $"INSERT INTO PolarisAccounts (AccountGuid,AccountHolder,AccountType) " +
                //    $"VALUES('{accountHolderGuid}','{accountHolder}','{accountType}')";

                //polarisLibrary = new PolarisDataLibrary(connectionString, commandText);

                //polarisLibrary.CreateAccountHolder();
                #endregion

                #region EF Testing

                polarisEfDataLibrary = new PolarisEFDataLibrary(connectionString);
                IPolarisAccountHolder accountHolder = new PolarisAccountHolder
                {
                    AccountGuid = Guid.NewGuid(),
                    AccountHolder = polarisAccountHolder.AccountHolder,
                    AccountType = polarisAccountHolder.AccountType
                };

                bool isCreated = polarisEfDataLibrary.CreateNewAccountHolder(accountHolder);

                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST api/<PolarisApi>
        [HttpPost("CreateNewAccountHolderNoAccountGuid")]
        public void PostAccountHolderNoAccountGuidField([FromBody] PolarisAccountHolder polarisAccountHolder)
        {
            try
            {
                string connectionString = config.GetSection("ConnectionStrings:PolarisDatabase").Value;
                #region EF Testing
                polarisEfDataLibrary = new PolarisEFDataLibrary(connectionString);
                IPolarisAccountHolder accountHolder = new PolarisAccountHolder
                {
                    AccountGuid = Guid.NewGuid(),
                    AccountHolder = polarisAccountHolder.AccountHolder,
                    AccountType = polarisAccountHolder.AccountType
                };

                bool isCreated = polarisEfDataLibrary.CreateNewAccountHolderAddAccountGuid(accountHolder);

                #endregion
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE api/<AccountProcessing>/5
        [HttpDelete("DeleteAccountHolderByAccountGuid/{accountGuid}")]
        public void DeleteAccountHolderByAccountGuid(string accountGuid)
        {
            string connectionString = config.GetSection("ConnectionStrings:PolarisDatabase").Value;

            #region SQL Testing
            //string commandText = $"DELETE FROM PolarisAccounts WHERE AccountGuid='{accountGuid}'";
            //polarisLibrary = new PolarisDataLibrary(connectionString, commandText);
            //bool successfulDelete = polarisLibrary.DeleteAccountHolderByAccountGuid(commandText);
            #endregion

            #region EF Testing
            polarisEfDataLibrary = new PolarisEFDataLibrary(connectionString);
            bool isDeleted = polarisEfDataLibrary.DeleteAccountHolderByAccountGuid(accountGuid);
            #endregion
        }
    }
}

