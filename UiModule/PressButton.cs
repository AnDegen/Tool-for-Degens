using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UiModule
{
    public sealed class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler,
        IPointerExitHandler
    {
        private readonly ReactiveProperty<bool> _isPressed = new();

        public IReadOnlyReactiveProperty<bool> IsPressed => _isPressed;

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed.Value = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed.Value = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPressed.Value = eventData.clickCount > 0;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPressed.Value = false;
        }
    }
}