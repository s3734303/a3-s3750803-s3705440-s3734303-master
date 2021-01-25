using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3,
        ServiceCharge = 4,
        BillPay = 5
    }

    public class TransactionDto
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public int AccountNumber { get; set; }
        public virtual AccountDto Account { get; set; }

        [ForeignKey("DestinationAccount")]
        public int? DestinationAccountNumber { get; set; }
        public virtual AccountDto DestinationAccount { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [MaxLength(250)]
        public String Comment { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ModifyDate { get; set; }

    }
}
