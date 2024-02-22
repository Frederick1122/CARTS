using Cars;
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
            SpawnedCarPrefabData = Instantiate(carPref, _carPlace);
            SpawnedCarData = data;

            Destroy(SpawnedCarPrefabData.gameObject.GetComponent<Rigidbody>());
        }
    }
}
