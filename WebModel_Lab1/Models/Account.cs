using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebModel_Lab1.Models;

public partial class Account
{
    [Required(ErrorMessage = "Account number is not stated")]
    //[RegularExpression(@"^[0-9]''.'+$",
    //     ErrorMessage = "Letters are not allowed.")]
    [StringLength(17, MinimumLength = 8, ErrorMessage = "Account number cannot be shorter than 8 or longer than 17 symbols")]
    public string AccountNumber { get; set; } = null!;

    
    public string Usreou { get; set; } = null!;

    
    public string Itn { get; set; } = null!;

    public string? Currency { get; set; }

    [Required(ErrorMessage = "Balance can't be empty")]
    public double Balance { get; set; }

    public double? CreditSum { get; set; }

    public virtual Customer? ItnNavigation { get; set; }

    public virtual Bank? UsreouNavigation { get; set; }
}
