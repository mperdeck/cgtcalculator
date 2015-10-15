using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;
using System;

namespace cgtcalculator
{
    public class ResultCsv
    {
        // Amount is positive when selling, negative when buying.
        // TotalGain is negative when making a loss

        // Format: yy-yy
        [CsvColumn(Name = "Fin. Year", FieldIndex = 1)]
        public string FinYear { get; set; }

        [CsvColumn(Name = "TransactionDate", OutputFormat = "d", FieldIndex = 2)]
        public DateTime? TransactionDate { get; set; }

        [CsvColumn(Name = "ShareCode", FieldIndex = 3)]
        public string ShareCode { get; set; } // 3 letter code

        [CsvColumn(FieldIndex = 4, Name = "Reference")]
        public string Reference { get; set; }

        [CsvColumn(Name = "Quantity", FieldIndex = 5)]
        public int? Quantity { get; set; }

        [CsvColumn(Name = "Amount", OutputFormat = "0.##", FieldIndex = 60)]
        public decimal? Amount { get; set; }

        // Following fields only relevant for buys

        [CsvColumn(Name = "MatchedQuantity", FieldIndex = 70)]
        public int? MatchedQuantity { get; set; }

        [CsvColumn(Name = "MatchedAmount", OutputFormat = "0.##", FieldIndex = 80)]
        public decimal? MatchedAmount { get; set; }

        [CsvColumn(Name = "Gain", OutputFormat = "0.##", FieldIndex = 90)]
        public decimal? Gain { get; set; }

        // Loss is always given as negative number
        [CsvColumn(Name = "Loss", OutputFormat = "0.##", FieldIndex = 100)]
        public decimal? Loss { get; set; }

        [CsvColumn(Name = "Attracts Discount", FieldIndex = 110)]
        public bool? AttractsDiscount { get; set; }

        [CsvColumn(Name = "Unadjusted Discount", FieldIndex = 120)]
        public decimal? UnadjustedDiscount { get; set; }

        public ResultCsv(DateTime? TransactionDate, string ShareCode, string Reference, int? Quantity, decimal? Amount,
            int? MatchedQuantity, decimal? MatchedAmount)
        {
            this.TransactionDate = TransactionDate;
            this.ShareCode = ShareCode;
            this.Reference = Reference;
            this.Quantity = Quantity;
            this.Amount = Amount;
            this.MatchedQuantity = MatchedQuantity;
            this.MatchedAmount = MatchedAmount;

            if (TransactionDate.HasValue)
            {
                FinYear = DateToFinYear(TransactionDate.Value);
            }
        }

        public void SetTotals(decimal totalGain, bool attractsDiscount)
        {
            if (totalGain > 0)
            {
                this.Gain = totalGain;

                if (attractsDiscount)
                {
                    this.UnadjustedDiscount = 0.5m * totalGain;
                }
            }
            else
            {
                this.Loss = totalGain;
            }

            this.AttractsDiscount = attractsDiscount;
        }

        private string DateToFinYear(DateTime d)
        {
            int beginYear = d.Year - 2000;
            if (d.Month <= 6) { beginYear--; }
            int endYear = beginYear + 1;

            return string.Format("{0:00}-{1:00}", beginYear, endYear);
        }
    }
}






