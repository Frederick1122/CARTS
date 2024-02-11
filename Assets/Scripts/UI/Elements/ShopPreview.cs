using CameraManger.Rotate;
using Managers.Libraries;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Elements
{
    public class ShopPreview : MonoBehaviour
    {
        [SerializeField] private ObjectRotate _rotator;

        private Transform _carPreviewPlace => _rotator.transform;

        public void StartPreview()
        {
            _rotator.ChangeRotateCondition(true);
            gameObject.SetActive(true);
        }

        public void StartPreview(CarData data)
        {
            SpawnCar(data);
            StartPreview();
        }

        public void StopPreview()
        {
            _rotator.ChangeRotateCondition(false);
            gameObject.SetActive(false);
        }

        public void SpawnCar(CarData data)
        {
            for (int i = 0; i < _carPreviewPlace.childCount; i++)
                Destroy(_carPreviewPlace.GetChild(i).gameObject);

            var carPref = CarLibrary.Instance.GetConfig(data.configKey).prefab;

            var car = Instantiate(carPref, _carPreviewPlace).gameObject;
            Destroy(car.GetComponent<Rigidbody>());

            SetGameLayerRecursive(car, 10);
        }

        private void SetGameLayerRecursive(GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
            {
                SetGameLayerRecursive(child.gameObject, layer);
            }
        }
    }
}
