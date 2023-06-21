using Savidiy.Utils;
using Zenject;

namespace MvvmModule
{
    public abstract class ViewModel<TModel> : ViewModel
    {
        public TModel Model { get; }

        protected ViewModel(TModel model, IViewModelFactory viewModelFactory) : base(viewModelFactory)
        {
            Model = model;
        }
    }

    public abstract class ViewModel : DisposableCollector, IViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly DiContainer _diContainer;

        protected ViewModel(IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        protected TViewModel CreateViewModel<TViewModel, TModel>(TModel model) where TViewModel : ViewModel<TModel>
        {
            TViewModel viewModel = _viewModelFactory.CreateViewModel<TViewModel, TModel>(model);
            return viewModel;
        }

        protected TViewModel CreateEmptyViewModel<TViewModel>() where TViewModel : EmptyViewModel
        {
            var viewModel = _viewModelFactory.CreateEmptyViewModel<TViewModel>();
            return viewModel;
        }
    }
}