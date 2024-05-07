using Cars;
using Cars.Controllers;
using ConfigScripts;
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
        public CarConfig SpawnedCarConfig { get; private set; }

        public void Init()
        {
            PlayerManager.Instance.OnPlayerCarChange += UpdateGarage;
            PlayerManager.Instance.OnPlayerCarUpdate += UpdateGarage;
           // UIManager.Instance.GetLobbyUi().OnCarInGarageUpdate += UpdateGarage;
        }

        private void OnDestroy()
        {
            PlayerManager.Instance.OnPlayerCarChange -= UpdateGarage;
            PlayerManager.Instance.OnPlayerCarUpdate -= UpdateGarage;
            //UIManager.Instance.GetLobbyUi().OnCarInGarageUpdate -= UpdateGarage;
        }

        public void UpdateGarage(CarData data)
        {
            for (int i = 0; i < _carPlace.childCount; i++)
                Destroy(_carPlace.GetChild(i).gameObject);

            SpawnedCarConfig = CarLibrary.Instance.GetConfig(data.configKey);
            SpawnedCarData = data;

            var carPref = SpawnedCarConfig.prefab;
            var spawnedCar = Instantiate(carPref, _carPlace);
            spawnedCar.RbSphere.isKinematic = true;
            DestroyImmediate(spawnedCar.GetComponent<Rigidbody>());
            spawnedCar.transform.position -= carPref.GetLowestPoint();
        }
    }
}
