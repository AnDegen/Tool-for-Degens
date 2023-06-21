namespace MvvmModule
{
    public abstract class EmptyViewModel : ViewModel
    {
        protected EmptyViewModel(IViewModelFactory viewModelFactory) : base(viewModelFactory)
        {
        }
    }
}