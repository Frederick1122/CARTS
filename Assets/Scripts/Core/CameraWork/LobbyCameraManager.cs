using Base;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManger.Lobby
{
    public class LobbyCameraManager : Singleton<LobbyCameraManager>
    {
        [Header("Cameras")]
        [SerializeField] private CinemachineVirtualCamera _mainCamera;
        [SerializeField] private CinemachineVirtualCamera _shopCamera;
        [SerializeField] private CinemachineVirtualCamera _startRaceCamera;
        [SerializeField] private CinemachineVirtualCamera _garageCamera;

        private readonly Dictionary<CameraPositions, CinemachineVirtualCamera> _cameraByPosition = new();
        public void Init()
        {
            _cameraByPosition.Add(CameraPositions.Default, _mainCamera);
            _cameraByPosition.Add(CameraPositions.Shop, _shopCamera);
            _cameraByPosition.Add(CameraPositions.StartRace, _startRaceCamera);
            _cameraByPosition.Add(CameraPositions.Garage, _garageCamera);

            foreach (var camera in _cameraByPosition.Values)
                camera.gameObject.SetActive(false);

            _mainCamera.gameObject.SetActive(true);
        }

        public void SwitchCamera(CameraPositions position)
        {
            foreach (var cam in _cameraByPosition)
            {
                if(cam.Key == position)
                    cam.Value.gameObject.SetActive(true);
                else
                    cam.Value.gameObject.SetActive(false);
            }
        }
    }

    public enum CameraPositions
    {
        Default = 0,
        Shop = 1,
        StartRace = 2,
        Garage = 3
    }
}
