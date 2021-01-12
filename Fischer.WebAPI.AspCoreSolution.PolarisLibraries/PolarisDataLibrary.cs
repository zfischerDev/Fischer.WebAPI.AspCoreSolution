using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Interfaces;
using Fischer.WebAPI.AspCoreSolution.PolarisLibraries.Objects;
using Microsoft.Data.SqlClient;

namespace Fischer.WebAPI.AspCoreSolution.PolarisLibraries
{
    public class PolarisDataLibrary : IPolarisDataLibrary
    {
        private readonly string connectionString;
        private readonly string commandText;

        public PolarisDataLibrary(string polarisConnectionString, string polarisCommandText)
        {
            connectionString = polarisConnectionString;
            commandText = polarisCommandText;
        }

        #region EntityFramework 
        public Task<List<IPolarisAccountHolder>> GetAllAccountsEF()
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
        public IPolarisAccountHolder GetSingleAccountByGuidEF(string accountGuid)
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

        #region Create Methods
        public void CreateAccountHolder()
        {
            SqlConnection sqlConnection = ConnectToSqlServer(connectionString);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public void CreateTransactions(List<IPolarisTransaction> transactions)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Retrieve Methods
        public Task<List<IPolarisAccountHolder>> GetAllAccounts(string polarisCommandText)
        {
            //open the connection
            SqlConnection sqlConnection = ConnectToSqlServer(connectionString);
            List<IPolarisAccountHolder> AccountsList = new List<IPolarisAccountHolder>();
            try
            {
                #region Entity Framework 
                PolarisEFContext efContext = new PolarisEFContext(connectionString);
                var theRows = (from rws in efContext.PolarisAccounts
                               select new
                               {
                                   accountGuid = rws.AccountGuid.ToString(),
                                   accountHolder = rws.AccountHolder,
                                   accountType = rws.AccountType
                               }).ToList();

                foreach (var zfRow in theRows)
                {
                    string acctGuid = zfRow.accountGuid;
                    string acctName = zfRow.accountHolder;
                    string accType = zfRow.accountType;
                }

                #endregion

                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);

                //Use a DataReader
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    IPolarisAccountHolder accountHolder = new PolarisAccountHolder();
                    //accountHolder accountInfo = new AccountInfo();
                    accountHolder.AccountGuid = Guid.Parse(dataReader[0].ToString());
                    accountHolder.AccountHolder = dataReader[1].ToString();
                    accountHolder.AccountType = dataReader[2].ToString();
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
        public IPolarisAccountHolder GetSingleAccountInfoByGuid(string sqlCommandText)
        {
            SqlConnection sqlConnection = ConnectToSqlServer(connectionString);
            PolarisAccountHolder accountholder = new PolarisAccountHolder();
            try
            {
                //open the connection
                //sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sqlCommandText, sqlConnection);

                //Use a DataReader
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    accountholder.AccountGuid = Guid.Parse(dataReader[0].ToString());
                    accountholder.AccountHolder = dataReader[1].ToString();
                    accountholder.AccountType = dataReader[2].ToString();
                }

                //return the account
                return accountholder;

            }
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="accountGuid"></param>
        /// <returns></returns>
        public Task<List<IPolarisTransaction>> GetTransactionsByAccountGuid(string sqlCommandText)
        {
            List<IPolarisTransaction> transactionList = new List<IPolarisTransaction>();
            //open the connection
            SqlConnection sqlConnection = ConnectToSqlServer(connectionString);

            //string sqlCommandText = $"SELECT TransactionGuid,AccountGuid,TransactionType," +
            //                    $"BeginningBalance,TransactionDateTime,TransactionAmount," +
            //                    $"Memo,EndingBalance FROM PolarisTransactions WHERE AccountGuid='{accountGuid}'";

            try
            {
                SqlCommand sqlCommand = new SqlCommand(sqlCommandText, sqlConnection);

                //Use a DataReader
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    IPolarisTransaction transaction = new PolarisTransaction();

                    //Transaction transaction = new Transaction();
                    transaction.TransactionGuid = Guid.Parse(dataReader[0].ToString());
                    transaction.AccountGuid = Guid.Parse(dataReader[1].ToString());
                    transaction.TransactionType = dataReader[2].ToString();
                    transaction.BeginningBalance = decimal.Parse(dataReader[3].ToString());
                    transaction.TransactionDateTime = DateTime.Parse(dataReader[4].ToString());
                    transaction.TransactionAmount = decimal.Parse(dataReader[5].ToString());
                    transaction.Memo = dataReader[6].ToString();
                    transaction.EndingBalance = decimal.Parse(dataReader[7].ToString());

                    transactionList.Add(transaction);
                }

                //return the transactions
                return Task.FromResult(transactionList);

            }
            catch (Exception exception)
            {

                throw exception;
            }
            finally
            {
                sqlConnection.Close();
            }

        }
        #endregion

        #region Update Methods

        #endregion

        #region Delete Methods
        public bool DeleteAccountHolderByAccountGuid(string sqlCommandText)
        {
            bool deleteSuccessful = false;

            SqlConnection sqlConnection = ConnectToSqlServer(connectionString);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(commandText, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                deleteSuccessful = true;
                return deleteSuccessful;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        #endregion

        #region Private Methods
        private SqlConnection ConnectToSqlServer(string connectionString)
        {

            try
            {
                SqlConnection getSqlConnection = new SqlConnection(connectionString);
                getSqlConnection.Open();

                return getSqlConnection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

    }
}
