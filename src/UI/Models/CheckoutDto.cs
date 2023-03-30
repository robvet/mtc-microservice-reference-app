using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MusicStore.Models
{
    //[Bind(Include = "FirstName,LastName,Address,City,State,PostalCode,Country,Phone,Email")]
    public class CheckoutDto
    {
        [BindNever]
        [ScaffoldColumn(false)]
        public string BasketId { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public int OrderId { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public string PromoCode { get; set; }

        [BindNever]
        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(25)]
        public string LastName { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 3)]
        public string Address { get; set; }

        [Required]
        [StringLength(25)]
        public string City { get; set; }

        [Required]
        [StringLength(25)]
        public string State { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        [StringLength(10, MinimumLength = 5)]
        public string PostalCode { get; set; }

        [Required]
        [Phone]
        //[StringLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //[BindNever]
        //[ScaffoldColumn(false)]
        //[Column(TypeName = "decimal(18,2)")]
        //public decimal Total { get; set; }

        [CreditCard]
        [Required]
        [Display(Name = "Credit Card Number")]
        //[StringLength(20)]
        public string CreditCardNumber { get; set; }


        [Required]
        [RegularExpression(@"^(\d{3})$", ErrorMessage = "Security code must be 3 numeric digits")]
        [Display(Name = "Security Code")]
        [StringLength(20)]
        public string SecurityCode { get; set; }

        [Required]
        [Display(Name = "Cardholder Name")]
        [StringLength(20)]
        public string CardholderName { get; set; }

        [Required]
        [Display(Name = "Expiration Date (mm/dd/yyyy)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        //[StringLength(20)]
        public DateTime ExpirationDate { get; set; }
    }
}