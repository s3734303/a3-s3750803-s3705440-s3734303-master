using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    public class Login
    {
        [Required, Range(1000, 9999), Display(Name = "Customer ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }

        [Key, Required, StringLength(8)]
        public string UserID { get; set; }

        [Required, StringLength(64)]
        public string Password { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime ModifyDate { get; set; }

        [Required, Range(0,3)]
        public int Flag { get; set; }

        [Required]
        public bool LoginStatus { get; set; }

        public DateTime Timer { get; set; }
    }
}
