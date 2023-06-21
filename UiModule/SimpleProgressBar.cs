using UnityEngine;
using UnityEngine.UI;

namespace UiModule
{
    public sealed class SimpleProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;

        public void SetProgress(float normalizedProgress)
        {
            _fillImage.fillAmount = normalizedProgress;
        }
    }
}