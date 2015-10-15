using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgtcalculator
{
    public class Calculator
    {
        public static List<ResultCsv> Calculate(List<Transaction> transactions)
        {
            return CalculateGainsPerShare(transactions);
        }




        public static List<ResultCsv> CalculateGainsPerShare(List<Transaction> transactions)
        {
            List<Transaction> sales = transactions.Where(t => t.Amount > 0).OrderBy(t => t.TransactionDate).ToList();
            List<Transaction> buys = transactions.Where(t => t.Amount <= 0).OrderBy(t => t.TransactionDate).ToList();
            List<ResultCsv> results = new List<ResultCsv>();

            foreach (Transaction sale in sales)
            {
                var saleResultCsv = new ResultCsv(sale.TransactionDate, sale.ShareCode, sale.Reference, 
                        sale.Quantity, sale.Amount, sale.Quantity, sale.Amount);

                results.Add(saleResultCsv);

                // ------                
                
                List<Transaction> potentialMatchingBuys = 
                    buys.Where(t => (t.TransactionDate <= sale.TransactionDate) && (t.ShareCode == sale.ShareCode)).OrderBy(t => t.TransactionDate).ToList();

                int nbrPotentialMatches = potentialMatchingBuys.Count();
                int quantityToBeMatched = sale.Quantity;
                decimal gain = sale.Amount;
                DateTime earliestMatchingBuy = DateTime.MaxValue;

                for (int i = 0; (i < nbrPotentialMatches) && (quantityToBeMatched > 0); i++)
                {
                    Transaction potentialMatch = potentialMatchingBuys[i];
                    if (potentialMatch.Quantity != 0)
                    {
                        decimal amount = 0;
                        int quantity = 0;

                        if (potentialMatch.Quantity > quantityToBeMatched)
                        {
                            // Quantity of this buy exceeds quantity that was still to be matched.

                            quantity = quantityToBeMatched;
                            amount = potentialMatch.Amount * ((decimal)quantity / (decimal)potentialMatch.Quantity);
                        }
                        else
                        {
                            amount = potentialMatch.Amount;
                            quantity = potentialMatch.Quantity;
                        }

                        gain += amount;
                        quantityToBeMatched -= quantity;

                        if (earliestMatchingBuy > potentialMatch.TransactionDate) { earliestMatchingBuy = potentialMatch.TransactionDate; }

                        // ----------

                        var buyResultCsv = new ResultCsv(potentialMatch.TransactionDate, potentialMatch.ShareCode, potentialMatch.Reference,
                            potentialMatch.Quantity, potentialMatch.Amount, quantity, amount);

                        results.Add(buyResultCsv);

                        // Adjust quantity so this match will taken into account for subsequent matches
                        potentialMatch.Quantity -= quantity;
                    }
                }

                if (quantityToBeMatched > 0)
                {
                    // Not enough buys to match the sale
                    var errorResultCsv = new ResultCsv(null, null, string.Format("*** Not Matched: {0} ***", quantityToBeMatched), null, null, null, null);
                    results.Add(errorResultCsv);
                }
                else
                {
                    TimeSpan periodShareHeld = saleResultCsv.TransactionDate.Value - earliestMatchingBuy;
                    bool attractsDiscount = (periodShareHeld.TotalDays > 366) && (gain > 0);

                    saleResultCsv.SetTotals(gain, attractsDiscount);
                }

                // ------------------

                ResultCsv emptyResultCsv = new ResultCsv(null, null, null, null, null, null, null);
                results.Add(emptyResultCsv);
            }

            return results;
        }



    }
}
