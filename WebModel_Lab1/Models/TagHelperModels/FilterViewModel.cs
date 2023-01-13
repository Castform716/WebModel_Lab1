using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebModel_Lab1.Models.TagHelperModels
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Account> accs, string USREOU, string currency)
        {
            
            accs.Insert(0, new Account { Usreou = "0", Currency = "0" });
            Accounts = new SelectList(accs, "USREOU", "AccountNumber", "Currency", USREOU);
            SelectedUSREOU = USREOU;
            SelectedCurrency = currency;
        }
        public SelectList Accounts { get; } 
        public string SelectedUSREOU { get; } 
        public string SelectedCurrency { get; } 
    }
}
