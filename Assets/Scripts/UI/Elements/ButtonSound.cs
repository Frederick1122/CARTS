using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public class ButtonSound : MonoBehaviour
    {
        private const string BUTTON_SOUND = "SFX/UI/PressButton";
        //[SerializeField] private EventReference _soundEffect;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(PlaySound);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(PlaySound);
        }

        private void PlaySound()
        {
            SoundManager.Instance.PlayOneShot(BUTTON_SOUND);
        }
    }
}