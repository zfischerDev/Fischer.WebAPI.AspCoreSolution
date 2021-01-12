using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Interfaces;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Fischer.WebAPI.AspCoreSolution.PolarisLibraries
{
    public class PolarisEFDataLibrary
    {
        private readonly string connectionString;
        //private readonly string commandText;
        //bool theBool = AccountGuidExists(Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")); 


        public PolarisEFDataLibrary(string polarisConnectionString)
        {
            connectionString = polarisConnectionString;
        }

        #region Public Methods

        #region AccountHolder

        #region Create 
        public bool CreateNewAccountHolder(IPolarisAccountHolder accountHolder)
        {
            bool createSuccessful = false;
            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);
                var polarisAccountHolder = new PolarisAccountHolder
                {
                    AccountGuid = accountHolder.AccountGuid,
                    AccountHolder = accountHolder.AccountHolder,
                    AccountType = accountHolder.AccountType
                };

                efContext.Entry(polarisAccountHolder).State = EntityState.Added;
                efContext.PolarisAccounts.Add(polarisAccountHolder);
                efContext.SaveChanges();
                createSuccessful = true;

                return createSuccessful;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool CreateNewAccountHolderAddAccountGuid(IPolarisAccountHolder accountHolder)
        {
            bool createSuccessful = false;
            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);
                var polarisAccountHolder = new PolarisAccountHolder
                {
                    //Create a new guid
                    AccountGuid = Guid.NewGuid(),
                    AccountHolder = accountHolder.AccountHolder,
                    AccountType = accountHolder.AccountType
                };

                efContext.Entry(polarisAccountHolder).State = EntityState.Added;
                efContext.PolarisAccounts.Add(polarisAccountHolder);
                efContext.SaveChanges();
                createSuccessful = true;

                return createSuccessful;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

        #region Retrieve
        public Task<List<IPolarisAccountHolder>> GetAllAccounts()
        {
            //open the connection
            List<IPolarisAccountHolder> AccountsList = new List<IPolarisAccountHolder>();
            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);

                //Get all the rows
                var theRows = (from rws in efContext.PolarisAccounts
                               select new
                               {
                                   //Need to cast guid to string or will get 
                                   //invalid cast error.
                                   accountGuid = rws.AccountGuid.ToString(),
                                   accountHolder = rws.AccountHolder,
                                   accountType = rws.AccountType
                               }).ToList();

                //Put the rows into the PolarisAccountHolder list
                foreach (var zfRow in theRows)
                {
                    IPolarisAccountHolder accountHolder = new PolarisAccountHolder();
                    //Pass AccountGuid back as Guid instead of string
                    accountHolder.AccountGuid = Guid.Parse(zfRow.accountGuid);
                    accountHolder.AccountHolder = zfRow.accountHolder;
                    accountHolder.AccountType = zfRow.accountType;
                    AccountsList.Add(accountHolder);
                }

                //return the account
                return Task.FromResult(AccountsList);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public IPolarisAccountHolder GetSingleAccountByGuid(string accountGuid)
        {
            //List<IPolarisAccountHolder> AccountsList = new List<IPolarisAccountHolder>();
            IPolarisAccountHolder accountHolder = new PolarisAccountHolder();

            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);

                //Get all the rows
                var theRow = (from rws in efContext.PolarisAccounts
                        .Where(rws => rws.AccountGuid.Equals(Guid.Parse(accountGuid)))
                              select new
                              {
                                  //Need to cast guid to string or will get 
                                  //invalid cast error.
                                  accountGuid = rws.AccountGuid.ToString(),
                                  accountHolder = rws.AccountHolder,
                                  accountType = rws.AccountType
                              }).FirstOrDefault();


                accountHolder.AccountGuid = Guid.Parse(theRow.accountGuid);
                accountHolder.AccountHolder = theRow.accountHolder;
                accountHolder.AccountType = theRow.accountType;

                //return the account
                return accountHolder;

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
        #endregion

        #region Delete
        public bool DeleteAccountHolderByAccountGuid(string accountGuid)
        {
            bool deleteSuccessful = false;
            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);
                var polarisAccount = new PolarisAccountHolder
                {
                    AccountGuid = Guid.Parse(accountGuid)
                };

                efContext.Entry(polarisAccount).State = EntityState.Deleted;
                efContext.PolarisAccounts.Remove(polarisAccount);
                //efContext.SaveChanges(); //Removed as it was throwing error

                return deleteSuccessful;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion

        #endregion

        #region Transactions

        #region Create

        #endregion

        #region Retrieve

        public Task<List<IPolarisTransaction>> GetTransactionsByAccountGuidEF(string accountGuid)
        {
            List<IPolarisTransaction> transactionList = new List<IPolarisTransaction>();
            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);
                List<IPolarisTransaction> TransactionList = new List<IPolarisTransaction>();

                var transRows = (from plTrxs in efContext.PolarisTransactions
                        .Where(plTrxs => plTrxs.AccountGuid.Equals(Guid.Parse(accountGuid)))
                                 select new

                                 {
                                     acctGuid = plTrxs.AccountGuid.ToString(),
                                     trxGuid = plTrxs.TransactionGuid.ToString(),
                                     trxType = plTrxs.TransactionType,
                                     trxBegBal = plTrxs.BeginningBalance,
                                     trxDate = plTrxs.TransactionDateTime,
                                     trxAmt = plTrxs.TransactionAmount,
                                     trxMemo = plTrxs.Memo,
                                     trxEndBal = plTrxs.EndingBalance
                                 }).ToList();

                //Put the rows into the PolarisTransaction list
                foreach (var trxRow in transRows)
                {
                    IPolarisTransaction polarisTransaction = new PolarisTransaction();
                    polarisTransaction.AccountGuid = Guid.Parse(trxRow.acctGuid.ToString());
                    polarisTransaction.TransactionGuid = Guid.Parse(trxRow.trxGuid.ToString());
                    polarisTransaction.TransactionType = trxRow.trxType;
                    polarisTransaction.BeginningBalance = trxRow.trxBegBal;
                    polarisTransaction.TransactionDateTime = trxRow.trxDate;
                    polarisTransaction.TransactionAmount = trxRow.trxAmt;
                    polarisTransaction.Memo = trxRow.trxMemo;
                    polarisTransaction.EndingBalance = decimal.Parse(trxRow.trxEndBal.ToString());
                    polarisTransaction.EndingBalance = trxRow.trxEndBal;
                    TransactionList.Add(polarisTransaction);
                }

                //return the account
                return Task.FromResult(TransactionList);

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        #endregion

        #region Delete 

        public bool DeleteTransactionByTransactionGuid(string transactionGuid)
        {
            bool deleteSuccessful = false;
            //int tableId;
            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);
                PolarisTransaction polarisTransaction = new PolarisTransaction();
                IPolarisTransaction plTrx = new PolarisTransaction();
                //Get the table ID by searching with the Transaction ID
                var transRows = (from plTrxs in efContext.PolarisTransactions
                        .Where(plTrxs => plTrxs.TransactionGuid.Equals(Guid.Parse(transactionGuid)))
                                 select new
                                 {
                                     tableId = plTrxs.TransactionId
                                 }).FirstOrDefault();

                //Put the table ID into the Transaction model
                polarisTransaction.TransactionId = transRows.tableId;

                efContext.Entry(polarisTransaction).State = EntityState.Deleted;
                efContext.Remove(polarisTransaction);
                efContext.SaveChanges();

                return deleteSuccessful;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool DeleteAllTransactionByAccountGuid(string accountGuid)
        {
            bool deleteSuccessful = false;
            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);

                //Get the Sql Server Table ID for each row that matches the Account Guid
                var transRows = (from plTrxs in efContext.PolarisTransactions
                        .Where(plTrxs => plTrxs.AccountGuid.Equals(Guid.Parse(accountGuid)))
                                 select new
                                 {
                                     tableId = plTrxs.TransactionId
                                 }).ToList();

                //Iterate through the list and delete the rows
                foreach (var transactionRow in transRows)
                {
                    PolarisTransaction transaction = new PolarisTransaction();
                    transaction.TransactionId = transactionRow.tableId;
                    efContext.Entry(transaction).State = EntityState.Deleted;

                    efContext.PolarisTransactions.RemoveRange(transaction);
                    efContext.SaveChanges();
                }

                return deleteSuccessful;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Private Methods

        private bool accountGuidExists(Guid accountGuid)
        {
            bool accountExists = false;

            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);
                var accountGuidMatchResult = (from foundRow in efContext.PolarisAccounts
                        .Where(foundRow => foundRow.AccountGuid.Equals(accountGuid))
                                              select new
                                              {
                                                  acctId = foundRow.AccountId
                                              }).FirstOrDefault();

                //If the value returns null, there is no match
                if (accountGuidMatchResult != null)
                {
                    accountExists = true;
                }
                return accountExists;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private bool transactionGuidExists(Guid transactionGuid)
        {
            bool accountExists = false;

            try
            {
                PolarisEFContext efContext = new PolarisEFContext(connectionString);
                var transactionGuidMatchResult = (from foundRow in efContext.PolarisTransactions
                        .Where(foundRow => foundRow.TransactionGuid.Equals(transactionGuid))
                                                  select new
                                                  {
                                                      trxId = foundRow.TransactionGuid
                                                  }).FirstOrDefault();

                //If the value returns null, there is no match
                if (transactionGuidMatchResult != null)
                {
                    accountExists = true;
                }
                return accountExists;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion
    }
}
