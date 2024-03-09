using DG.Tweening;
using UnityEngine;

namespace Obstacles
{
    public class Drawbridge : AbstractObstacle
    {
        [SerializeField] private Transform _pivot;
        [Space]
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay = 1f;
        [Range(1,89)]
        [SerializeField] private float _rotationDegrees = 45f;

        [SerializeField] private bool _isTurnUp = false;
        
        
        private void Start()
        {
            Sequence sequence = DOTween.Sequence();

            var modificator = _isTurnUp ? -1 : 1;
            var startRotation = transform.rotation.eulerAngles;
            sequence.AppendInterval(_delay);
            sequence.Append(_pivot.DORotate(startRotation + new Vector3(0, 0,  _rotationDegrees * modificator),  _duration / 2));
            sequence.AppendInterval(_delay);
            sequence.Append(_pivot.DORotate(startRotation,  _duration / 2));
            sequence.OnComplete(() => sequence.Restart());
            sequence.Play();
        }
    }
}