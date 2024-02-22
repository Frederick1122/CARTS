using Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    [RequireComponent(typeof(LineRenderer))]
    public class FloatingUICarCharacteristic : MonoBehaviour
    {
        public event Action<ModificationType> OnCharacteristicUpgrade = delegate { };

        [field: SerializeField] public ModificationType ModificationType { get; private set; } 

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _characteristicName;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _cost;

        private LineRenderer _lineRenderer;
        private Camera _uiCamera;
        private Canvas _uiCanvas;

        public void Init(Camera uiCamera, Canvas uiCanvas)
        {
            _uiCamera = uiCamera;
            _uiCanvas = uiCanvas;
            _upgradeButton.onClick.AddListener(RequestForUpgrade);
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void OnDestroy()
        {
            _upgradeButton.onClick.RemoveListener(RequestForUpgrade);
        }

        private void RequestForUpgrade() =>
            OnCharacteristicUpgrade?.Invoke(ModificationType);

        public void UpdateInfo(int level, int cost)
        {
            _level.text = level.ToString();
            _cost.text = cost.ToString();
        }

        public void DrawLine(Transform endPointObject)
        {
            _lineRenderer.enabled = true;

            var screen = Camera.main.WorldToScreenPoint(endPointObject.position);
            screen.z = (_uiCanvas.transform.position - _uiCamera.transform.position).magnitude;
            var pos = _uiCamera.ScreenToWorldPoint(screen);
            _lineRenderer.SetPosition(0, pos);
            _lineRenderer.SetPosition(1, transform.position);
        }

        public void DisableLine()
        {
            _lineRenderer.enabled = false;
        }
    }
}
