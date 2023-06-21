using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MvvmModule
{
    public abstract class View<THierarchy, TViewModel> : View<THierarchy>
        where THierarchy : MonoBehaviour
        where TViewModel : class, IViewModel
    {
        private readonly List<IDisposable> _bindDisposables = new();
        private readonly List<(Button button, UnityAction onClick)> _bindButtonDisposables = new();
        private readonly List<(Slider slider, UnityAction<float> onChange)> _bindSliderDisposables = new();
        private readonly List<(Toggle toggle, UnityAction<bool> onChange)> _bindToggleDisposables = new();

        protected TViewModel ViewModel { get; private set; }

        protected View(THierarchy hierarchy, IViewFactory viewFactory) : base(hierarchy, viewFactory)
        {
        }

        public void Initialize(TViewModel viewModel)
        {
            ClearViewModel();
            ViewModel = viewModel;
            UpdateViewModel(viewModel);
        }

        protected abstract void UpdateViewModel(TViewModel viewModel);

        protected override void ReleaseViewModel()
        {
            base.ReleaseViewModel();
            for (int i = 0; i < _bindDisposables.Count; i++)
                _bindDisposables[i].Dispose();

            _bindDisposables.Clear();

            for (var index = 0; index < _bindButtonDisposables.Count; index++)
            {
                (Button button, UnityAction onClick) = _bindButtonDisposables[index];
                button.onClick.RemoveListener(onClick);
            }

            _bindButtonDisposables.Clear();
            
            for (var index = 0; index < _bindSliderDisposables.Count; index++)
            {
                (Slider slider, UnityAction<float> onChange) = _bindSliderDisposables[index];
                slider.onValueChanged.RemoveListener(onChange);
            }

            _bindSliderDisposables.Clear();
            
            for (var index = 0; index < _bindToggleDisposables.Count; index++)
            {
                (Toggle toggle, UnityAction<bool> onChange) = _bindToggleDisposables[index];
                toggle.onValueChanged.RemoveListener(onChange);
            }

            _bindToggleDisposables.Clear();
        }

        public override void ClearViewModel()
        {
            base.ClearViewModel();

            if (ViewModel == null)
                return;

            ReleaseViewModel();
            if (ViewModel is IDisposable disposable)
                disposable.Dispose();

            ViewModel = null;
        }

        public override void Dispose()
        {
            ClearViewModel();
            base.Dispose();
        }

        protected void Bind<T>(IReadOnlyReactiveProperty<T> field, Action<T> onChange)
        {
            _bindDisposables.Add(field.Subscribe(onChange));
        }

        protected void Bind<T>(IReadOnlyReactiveProperty<T> field, Action<Pair<T>> onChange)
        {
            BindSilently(field, onChange);
            onChange(new Pair<T>(field.Value, field.Value));
        }

        protected void BindSilently<T>(IReadOnlyReactiveProperty<T> field, Action<T> onChange)
        {
            _bindDisposables.Add(field.SkipLatestValueOnSubscribe().Subscribe(onChange));
        }

        protected void BindSilently<T>(IReadOnlyReactiveProperty<T> field, Action<Pair<T>> onChange)
        {
            _bindDisposables.Add(field.Pairwise().Subscribe(onChange));
        }

        protected void Bind<T>(IReadOnlyReactiveCollection<T> field, Action<int> onChange)
        {
            _bindDisposables.Add(field.ObserveCountChanged().Subscribe(onChange));
            onChange(field.Count);
        }

        protected void BindClick(Button button, UnityAction onClick)
        {
            button.onClick.AddListener(onClick);
            _bindButtonDisposables.Add((button, onClick));
        }

        protected void BindToggle(Toggle toggle, UnityAction<bool> onChange)
        {
            toggle.onValueChanged.AddListener(onChange);
            _bindToggleDisposables.Add((toggle, onChange));
            onChange?.Invoke(toggle.isOn);
        }

        protected void BindSlider(Slider slider, UnityAction<float> onChange)
        {
            slider.onValueChanged.AddListener(onChange);
            _bindSliderDisposables.Add((slider, onChange));
            onChange?.Invoke(slider.value);
        }
    }

    public abstract class View<THierarchy> : View
        where THierarchy : MonoBehaviour
    {
        public bool IsActive => Hierarchy.gameObject.activeSelf;
        public THierarchy Hierarchy { get; }

        protected View(THierarchy hierarchy, IViewFactory viewFactory) : base(viewFactory)
        {
            Hierarchy = hierarchy;
        }

        public void SetActive(bool active)
        {
            Hierarchy.gameObject.SetActive(active);
        }
    }

    public abstract class View : DisposableCollector
    {
        private readonly IViewFactory _viewFactory;
        private readonly List<View> _childViews = new();

        protected View(IViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        protected virtual void ReleaseViewModel()
        {
        }

        public virtual void ClearViewModel()
        {
            for (var i = 0; i < _childViews.Count; i++)
            {
                _childViews[i].ClearViewModel();
            }
        }

        protected TView CreateView<TView, THierarchy>(THierarchy hierarchy)
            where TView : View<THierarchy> where THierarchy : MonoBehaviour
        {
            TView view = _viewFactory.CreateView<TView, THierarchy>(hierarchy);

            _childViews.Add(view);
            AddDisposable(view);

            return view;
        }

        protected TView CreateView<TView, THierarchy>(string prefabName, Transform parent)
            where TView : View<THierarchy> where THierarchy : MonoBehaviour
        {
            TView view = _viewFactory.CreateView<TView, THierarchy>(prefabName, parent);

            _childViews.Add(view);
            AddDisposable(view);

            return view;
        }
    }
}