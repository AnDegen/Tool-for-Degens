using System;
using UiModule;
using UnityEngine;
using UnityEngine.UI;

namespace MvvmModule
{
    public sealed class B123Hierarchy : MonoBehaviour
    {
    }

    public sealed class B123View : View<B123Hierarchy, IB123ViewModel>
    {
        // _view = CreateView<B123View, B123Hierarchy>(PREFAB_NAME, root);
        public B123View(B123Hierarchy hierarchy, IViewFactory viewFactory) : base(hierarchy, viewFactory)
        {
        }

        protected override void UpdateViewModel(IB123ViewModel viewModel)
        {
        }
    }

    public interface IB123ViewModel : IViewModel
    {
    }

    public sealed class B123ViewModel : ViewModel<B123Args>, IB123ViewModel
    {
        // var args = new B123Args();
        // var viewModel = CreateViewModel<B123ViewModel, B123Args>(args);

        public B123ViewModel(B123Args model, IViewModelFactory viewModelFactory) : base(model, viewModelFactory)
        {
        }
    }

    public class B123Args
    {
    }
    
    public sealed class B123Presenter : IDisposable, IB123Presenter
    {
        // Container.BindInterfacesTo<B123Presenter>().AsSingle();
        private const string PREFAB_NAME = "_window";
        private readonly WindowsRootProvider _windowsRootProvider;
        private readonly IViewFactory _viewFactory;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly B123View _view;
        
        public B123Presenter(WindowsRootProvider windowsRootProvider, IViewFactory viewFactory,
            IViewModelFactory viewModelFactory)
        {
            _viewFactory = viewFactory;
            _viewModelFactory = viewModelFactory;
            _windowsRootProvider = windowsRootProvider;
            _view = CreateView();
            _view.SetActive(false);
        }
        
        public void ShowWindow()
        {
            var args = new B123Args();
            var viewModel = _viewModelFactory.CreateViewModel<B123ViewModel, B123Args>(args);
            _view.Initialize(viewModel);
            _view.SetActive(true);
        }
        
        public void HideWindow()
        {
            _view.ClearViewModel();
            _view.SetActive(false);
        }
        
        public void Dispose()
        {
            _view.Dispose();
        }
        
        private B123View CreateView()
        {
            Transform root = _windowsRootProvider.GetWindowRoot();
            var view = _viewFactory.CreateView<B123View, B123Hierarchy>(PREFAB_NAME, root);
            return view;
        }
    }

    public interface IB123Presenter
    {
        void ShowWindow();
        void HideWindow();
    }
}