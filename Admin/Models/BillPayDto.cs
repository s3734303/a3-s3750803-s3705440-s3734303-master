using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models
{
    public enum PeriodType
    {
        M = 1,  //Monthly
        Q = 2,  //Quaterly
        Y = 3,  //Yearly
        S = 4   //Once Off
    }
    public class BillPayDto
    {
        [Key, Required, Range(1000, 9999)]
        public int BillPayId { get; set; }

        [Required]
        [ForeignKey("Account")]
        public int AccountNumber { get; set; }
        public virtual AccountDto Account { get; set; }

        [Required, Range(1000, 9999), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PayeeId { get; set; }
        public virtual PayeeDto Payee { get; set; }

        [Required, Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ScheduleDate { get; set; }

        [Required]
        public PeriodType Period { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ModifyDate { get; set; }

        public Boolean Active { get; set; }
    }
}
