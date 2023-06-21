using Zenject;

namespace MvvmModule
{
    internal sealed class ViewModelFactory : IViewModelFactory
    {
        private readonly DiContainer _diContainer;

        public ViewModelFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public TViewModel CreateViewModel<TViewModel, TModel>(TModel model) 
            where TViewModel : ViewModel<TModel>
        {
            _diContainer.Bind<TModel>().FromInstance(model).AsTransient();
            _diContainer.Bind<TViewModel>().AsTransient();
            var viewModel = _diContainer.Resolve<TViewModel>();
            _diContainer.Unbind<TViewModel>();
            _diContainer.Unbind<TModel>();

            return viewModel;
        }

        public TViewModel CreateEmptyViewModel<TViewModel>() 
            where TViewModel : EmptyViewModel
        {
            _diContainer.Bind<TViewModel>().AsTransient();
            var viewModel = _diContainer.Resolve<TViewModel>();
            _diContainer.Unbind<TViewModel>();

            return viewModel;
        }
    }
}