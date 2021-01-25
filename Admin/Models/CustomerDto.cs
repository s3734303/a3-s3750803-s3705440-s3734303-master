using System;
using System.Net.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Admin.Models
{
    public class CustomerDto
    {
        [Key, Required, Range(1000, 9999), Display(Name = "Customer ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [Required, MaxLength(50), Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [MaxLength(11)]
        public string TFN { get; set; }

        [MaxLength(50), Display(Name = "Address")]
        public String CustAddress { get; set; }

        [MaxLength(40), Display(Name = "City")]
        public String CustCity { get; set; }

        [StringLength(3), Display(Name = "State")]
        public String CustState { get; set; }

        [MaxLength(4)]
        public String PostCode { get; set; }

        [Required, MaxLength(15, ErrorMessage = "Format :- +61 XXX XXX XXX"), Display(Name = "Phone Number")]
        public String PhoneNum { get; set; }
        public virtual List<AccountDto> Accounts { get; set; }

        public static implicit operator CustomerDto(HttpResponseMessage v)
        {
            throw new NotImplementedException();
        }
    }
}
