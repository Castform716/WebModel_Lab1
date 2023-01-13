namespace WebModel_Lab1.Models.TagHelperModels
{
    public class AccsViewModel
    {
        public IEnumerable<Account> Accounts { get; }
        public PageViewModel PageViewModel { get; }
        public FilterViewModel FilterViewModel { get; }
        public SortViewModel SortViewModel { get; }
        public AccsViewModel(IEnumerable<Account> nodes, PageViewModel pageViewModel,
            FilterViewModel filterViewModel, SortViewModel sortViewModel)
        {
            Accounts = nodes;
            PageViewModel = pageViewModel;
            FilterViewModel = filterViewModel;
            SortViewModel = sortViewModel;
        }
    }
}
