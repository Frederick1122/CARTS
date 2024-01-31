using Managers;
using Managers.Libraries;
using System.Collections;
using System.Collections.Generic;
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
        }

        private void OnDestroy()
        {
            PlayerManager.Instance.OnPlayerCarChange -= UpdateGarage;
        }

        public void UpdateGarage(CarData data)
        {
            for (int i = 0; i < _carPlace.childCount; i++)
                Destroy(_carPlace.GetChild(i).gameObject);
            
            CurrentChosenCar = data;
            var carPref = CarLibrary.Instance.GetConfig(CurrentChosenCar.configKey).prefab;

            var car = Instantiate(carPref, _carPlace);
            Destroy(car.GetComponent<Rigidbody>());
        }
    }
}
