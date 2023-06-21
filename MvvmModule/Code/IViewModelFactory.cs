namespace MvvmModule
{
    public interface IViewModelFactory
    {
        TViewModel CreateViewModel<TViewModel, TModel>(TModel model) 
            where TViewModel : ViewModel<TModel>;

        TViewModel CreateEmptyViewModel<TViewModel>() 
            where TViewModel : EmptyViewModel;
    }
}