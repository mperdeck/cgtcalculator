using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace cgtcalculator
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine(
                    @"
This program calculates the capital loss or gain on each share transaction.
It gets the transactions from one or more .csv files. The format of these files is that of the
file you get when you follow these instructions:

In Commsec, open Accounts tab, Transactions tab, specify date range that spans ALL your share transactions, download the CSV file.

You specify the .csv files as positional input parameters. Order of the parameters does not matter. 
The transactions will be deduped:

cgtcalculator <path to csv file> <path to csv file> ....

It writes a results.csv file to the current directory with the net losses and gains per share.

It uses first in / first out to match share purchases against shares sold.

For sales where at least one matching buy is more than a year before the sale, it separately shows 
a 50% discount if you made a gain. Note that this needs to be adjusted if you apply (part of) your
losses against the original gain!
");
                return 1;
            }

            string[] commsecTransactionsCsvFilePaths = args;

            List<Transaction> transactions = Csv.GetTransactions(commsecTransactionsCsvFilePaths);

            List<ResultCsv> results = Calculator.Calculate(transactions);

            string resultsCsvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "results.csv");

            Csv.WriteResults(resultsCsvFilePath, results);

            return 0;
        }
    }
}
