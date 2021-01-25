using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class Payee
    {
        [Key, Required, Range(1000, 9999), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PayeeId { get; set; }

        [Required, StringLength(50)]
        public String PayeeName { get; set; }

        [MaxLength(50)]
        public String PayeeAddress { get; set; }

        [MaxLength(40)]
        public String PayeeCity { get; set; }

        [StringLength(3)]
        public String PayeeState { get; set; }

        [MaxLength(4)]
        public String PostCode { get; set; }

        [Required, MaxLength(15)]
        public String PhoneNum { get; set; }

        public virtual List<BillPay> BillPays { get; set; }
    }
}
