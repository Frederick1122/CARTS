using Base;
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
            {
                var newGameObject = Instantiate(gameObjectToInstall);
                DontDestroyOnLoad(newGameObject);

                foreach (var component in newGameObject.GetComponents(typeof(Component)))
                {
                    if (component is ICreate createComponent)
                    {
                        createComponent.Create();
                    }
                }
            }
        }
    }
}
