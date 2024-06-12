using Cars.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Tools
{
    public class CarScreener : MonoBehaviour
    {
        const string SCREEN_PATH = "Assets/ART/CarScreens";

        [SerializeField]
        private List<GameObject> _carPrefabs;

        [Header("Screen characteristic")]
        [SerializeField]
        private Vector2Int _resolution = new (1024, 1024);
        [SerializeField]
        private Camera _camera;

        [Header("Environment")]
        [SerializeField]
        private Transform _carPlace;
        
        private GameObject _spawnedCar = null;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                MakeCarScreens();
        }

        private void MakeCarScreens()
        {
            foreach (var carPrefab in _carPrefabs)
            {
                SpawnCar(carPrefab);
                string screenPath = $"{SCREEN_PATH}/{carPrefab.name}.png";
                TakeTransparentScreenshot(_camera, _resolution.x, _resolution.y, screenPath);
            }
        }

        private void SpawnCar(GameObject carPref)
        {
            Debug.Log("Spawn");

            if (_spawnedCar != null)
                DestroyImmediate(_spawnedCar);
             
            var car = Instantiate(carPref, _carPlace);
            var rb = car.GetComponentsInChildren<Rigidbody>();
            foreach (var r in rb)
                r.isKinematic = true;
            _spawnedCar = car;
        }

        private void TakeTransparentScreenshot(Camera cam, int width, int height, string savePath)
        {
            // Depending on your render pipeline, this may not work.
            var bak_cam_targetTexture = cam.targetTexture;
            var bak_cam_clearFlags = cam.clearFlags;
            var bak_RenderTexture_active = RenderTexture.active;

            var tex_transparent = new Texture2D(width, height, TextureFormat.ARGB32, false);
            // Must use 24-bit depth buffer to be able to fill background.
            var render_texture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
            var grab_area = new Rect(0, 0, width, height);

            RenderTexture.active = render_texture;
            cam.targetTexture = render_texture;
            cam.clearFlags = CameraClearFlags.SolidColor;

            // Simple: use a clear background
            cam.backgroundColor = Color.clear;
            cam.Render();
            tex_transparent.ReadPixels(grab_area, 0, 0);
            tex_transparent.Apply();

            // Encode the resulting output texture to a byte array then write to the file
            byte[] pngShot = ImageConversion.EncodeToPNG(tex_transparent);
            File.WriteAllBytes(savePath, pngShot);

            cam.clearFlags = bak_cam_clearFlags;
            cam.targetTexture = bak_cam_targetTexture;
            RenderTexture.active = bak_RenderTexture_active;
            RenderTexture.ReleaseTemporary(render_texture);
            Texture2D.Destroy(tex_transparent);
        }
    }
}
