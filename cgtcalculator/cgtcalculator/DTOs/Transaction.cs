using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;
using System;

namespace cgtcalculator
{
    public class Transaction
    {
        // Amount is positive when selling, negative when buying.
        public DateTime TransactionDate { get; set; }
        public string ShareCode { get; set; } // 3 letter code
        public string Reference { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }

        public Transaction()
        {
        }

        public Transaction(DateTime TransactionDate, string ShareCode, string Reference, int Quantity, decimal Amount, int MatchedQuantity, decimal MatchedAmount)
        {
            this.TransactionDate = TransactionDate;
            this.ShareCode = ShareCode;
            this.Reference = Reference;
            this.Quantity = Quantity;
            this.Amount = Amount;
        }
    }
}




