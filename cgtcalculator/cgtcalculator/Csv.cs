using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;

namespace cgtcalculator
{
    public class Csv
    {
        public static List<Transaction> GetTransactions(string[] csvFilePaths)
        {
            List<Transaction> transactions =
                csvFilePaths.SelectMany(f => GetTransactions(f)).ToList();

            List<Transaction> transactionsDeduped = transactions.Distinct().ToList();

            return transactionsDeduped;
        }

        public static List<Transaction> GetTransactions(string csvFilePath)
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                FileCultureName = "en-AU",
                IgnoreTrailingSeparatorChar = true
            };

            CsvContext cc = new CsvContext();

            IEnumerable<ComsecTransactionCsv> comsecTransactions =
                cc.Read<ComsecTransactionCsv>(csvFilePath, inputFileDescription).ToList();

            List<Transaction> transactions = comsecTransactions.Where(t => t.TransactionType == "Contract").Select(t => ToTransaction(t)).ToList();

            return transactions;
        }

        public static void WriteResults(string resultsCsvFilePath, List<ResultCsv> results)
        {
            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',', 
                FirstLineHasColumnNames = true,
                FileCultureName = "en-AU" 
            };

            CsvContext cc = new CsvContext();

            cc.Write(results, resultsCsvFilePath, outputFileDescription);
        }

        private static Transaction ToTransaction(ComsecTransactionCsv comsecTransactionCsv)
        {
            var transaction = new Transaction();

            // Detail field has format
            // <B or S> <quanity> <share code> @ <share price>
            // For example
            // B 269 VTS @ 148.620000

            string[] detailComponents = comsecTransactionCsv.Details.Split(new Char[] { ' ' });

            transaction.TransactionDate = comsecTransactionCsv.TransactionDate;
            transaction.ShareCode = detailComponents[2];
            transaction.Quantity = int.Parse(detailComponents[1]);

            if (detailComponents[0] == "S")
            {
                transaction.Amount = comsecTransactionCsv.Credit;
            }
            else
            {
                transaction.Amount = -1 * comsecTransactionCsv.Debit;
            }

            transaction.Reference = comsecTransactionCsv.Reference;

            return transaction;
        }


    }
}
