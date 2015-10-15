using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQtoCSV;
using System;

namespace cgtcalculator
{
    public class ComsecTransactionCsv
    {
        [CsvColumn(Name = "Date", FieldIndex = 1)]
        public DateTime TransactionDate { get; set; }
        [CsvColumn(FieldIndex = 2, Name = "Reference")]
        public string Reference { get; set; }
        [CsvColumn(FieldIndex = 3, Name="Type")]
        public string TransactionType { get; set; }
        [CsvColumn(FieldIndex = 4, Name = "Detail")]
        public string Details { get; set; }
        [CsvColumn(FieldIndex = 5, Name="Debit ($)")]
        public decimal Debit { get; set; }
        [CsvColumn(FieldIndex = 6, Name="Credit ($)")]
        public decimal Credit { get; set; }
        [CsvColumn(FieldIndex = 7, Name="Balance ($)")]
        public decimal Balance { get; set; }
    }
}




