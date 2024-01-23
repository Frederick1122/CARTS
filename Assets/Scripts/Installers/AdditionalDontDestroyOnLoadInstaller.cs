using UnityEngine;
using Zenject;

namespace Installers
{
    public class AdditionalDontDestroyOnLoadInstaller : MonoInstaller
    {
        [SerializeField] private GameObject[] _gameObjects;

        public override void InstallBindings()
        {
            foreach (var gameObjectToInstall in _gameObjects)
                DontDestroyOnLoad(Instantiate(gameObjectToInstall));
        }
    }
}
