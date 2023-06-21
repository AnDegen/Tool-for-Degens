using UnityEngine;
using Object = UnityEngine.Object;

namespace UiModule
{
    public class WindowsRootProvider
    {
        private readonly Transform _transform;

        public WindowsRootProvider()
        {
            var windowRootBehaviour = Object.FindObjectOfType<WindowRootBehaviour>();
            _transform = windowRootBehaviour.transform;
        }
        
        public Transform GetWindowRoot()
        {
            return _transform;
        }
    }
}