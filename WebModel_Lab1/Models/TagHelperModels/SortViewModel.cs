namespace WebModel_Lab1.Models.TagHelperModels
{
    public class SortViewModel
    {
        public SortState UsreouSort { get; }
        public SortState BalanceSort { get; }
        public SortState CreditSort { get; }
        public SortState Current { get; }

        public SortViewModel(SortState sortOrder)
        {
            UsreouSort = sortOrder == SortState.UsreouAsc ? SortState.UsreouDesc : SortState.UsreouAsc;
            BalanceSort = sortOrder == SortState.BalanceAsc ? SortState.BalanceDesc : SortState.BalanceAsc;
            CreditSort = sortOrder == SortState.CreditAsc ? SortState.CreditDesc : SortState.CreditAsc;
            Current = sortOrder;
        }
    }
}

