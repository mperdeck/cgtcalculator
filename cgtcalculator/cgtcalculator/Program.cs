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

In Commsec, open Portfolio | Accounts tab, Transactions tab, specify date range that spans ALL your share transactions, search, download the CSV file.

You specify the .csv files as positional input parameters. Order of the parameters does not matter. 
The transactions will be deduped:

cgtcalculator <path to csv file> <path to csv file> ....

It writes a results.csv file to the current directory with the net losses and gains per share.

It uses first in / first out to match share purchases against shares sold.

For sales where at least one matching buy is more than a year before the sale, it separately shows 
a 50% discount if you made a gain. Note that this needs to be adjusted if you apply (part of) your
losses against the original gain!

>>>>>>>>>>>>>>>>>>>>>
This program has these bugs:

1) IMPORTANT - If one buy matches 2 different sales, than the cost of the sale for one of the matches is wrong.
   In the wrong match, it assumes that the cost of the matched shares is the full cost of the entire buy, thereby
   overstating losses.

2) If there are multiple sales of the same stock and one or more of those sales resulted in a gain, then you want
   to optimise the order in which buys are matched against sales, so the 50% discount (when the asset was held over a year)
   is maximised. It doesn't do that optimisation.

To solve these bugs you'll want to restructure the program.

1) To the transactions, add a field for QuantityLeftToMatch. Use that to keep track of how much of a buy is still available to be matched,
and how much of a sell is still to be matched.

2) When matching sells, there are 2 strategies to find buys to match: in addition to coming before the sale and being of same stock,
you can search latest buy date first and then work back to the past, or search oldest buy date first and work towards the present.
You'll want to test both strategies for each sell.
That is, if there are N sells, you'll wind up with 2^N scenarios with the strategies applied for each sell.
Then calculate the losses/profits for each scenario and find the best one.

Note that when visiting sells, you always want to go from oldest to youngest. Otherwise a later sell may match against buys needed
by an earlier sell. Also, that way the outcome for sells in previous financial years remains the same, even when doing calculations 
for a later financial year.
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
