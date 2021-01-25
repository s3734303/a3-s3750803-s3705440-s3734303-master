using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Models
{
    public enum AccountType
    {
        C = 1,  //Checking Account
        S = 2   //Saving Account
    }

    public class AccountDto
    {
        [Key, Required, Range(1000, 9999), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountNumber { get; set; }
        
        [Required]
        public AccountType AccountType { get; set; }

        [Required, Range(1000, 9999), Display(Name = "Customer ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }
        public virtual CustomerDto Customer { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal Balance { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ModifyDate { get; set; }

        public virtual List<TransactionDto> Transactions { get; set; }

    }
}
