using UnityEngine;

namespace MvvmModule
{
    public interface IPrefabFactory
    {
        T Instantiate<T>(string prefabName, Transform parent)
            where T : MonoBehaviour;
    }
}