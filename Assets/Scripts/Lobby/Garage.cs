using Managers;
using Managers.Libraries;
using UI;
using UnityEngine;

namespace Lobby.Garage
{
    public class Garage : MonoBehaviour
    {
        [SerializeField] private Transform _carPlace;

        public CarData CurrentChosenCar {  get; private set; }

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
            
            CurrentChosenCar = data;
            var carPref = CarLibrary.Instance.GetConfig(CurrentChosenCar.configKey).prefab;

            var car = Instantiate(carPref, _carPlace).gameObject;
            Destroy(car.GetComponent<Rigidbody>());
        }
    }
}
