using Cars;
using Cars.Controllers;
using Managers;
using Managers.Libraries;
using UI;
using UnityEngine;

namespace Lobby.Garage
{
    public class Garage : MonoBehaviour
    {
        [SerializeField] private Transform _carPlace;

        public CarData SpawnedCarData {  get; private set; }
        public CarPrefabData SpawnedCarPrefabData { get; private set; }

        public void Init()
        {
            PlayerManager.Instance.OnPlayerCarChange += UpdateGarage;
            UIManager.Instance.GetLobbyUi().OnCarInGarageUpdate += UpdateGarage;
        }

        private void OnDestroy()
        {
            PlayerManager.Instance.OnPlayerCarChange -= UpdateGarage;
            UIManager.Instance.GetLobbyUi().OnCarInGarageUpdate -= UpdateGarage;
        }

        public void UpdateGarage(CarData data)
        {
            for (int i = 0; i < _carPlace.childCount; i++)
                Destroy(_carPlace.GetChild(i).gameObject);
            
            var carPref = CarLibrary.Instance.GetConfig(data.configKey).prefab;
            var spawnedCar = Instantiate(carPref, _carPlace);
            spawnedCar.RbSphere.isKinematic = true;
            DestroyImmediate(spawnedCar.GetComponent<Rigidbody>());
            spawnedCar.transform.position -= carPref.GetLowestPoint();
            
            SpawnedCarData = data;
            SpawnedCarPrefabData = spawnedCar;
        }
    }
}
