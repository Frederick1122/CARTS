using DG.Tweening;
using UnityEngine;

namespace Obstacles
{
    public class Hammer : AbstractObstacle
    {
        [SerializeField] private Transform _hammerPivot;
        
        [SerializeField] private float _duration = 1f;
        [Range(1,89)]
        [SerializeField] private float _rotationDegrees = 45f;
        [SerializeField] private bool _isStartLeftRotate;

        private void Start()
        {
            Sequence sequence = DOTween.Sequence();

            var modificator = _isStartLeftRotate ? -1 : 1;
            
            sequence.Append(_hammerPivot.DORotate(new Vector3(_rotationDegrees * modificator, 0,  0),  _duration / 2));
            sequence.Append(_hammerPivot.DORotate(new Vector3(_rotationDegrees * modificator * -1, 0,  0),  _duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));
            sequence.Play();
        }
    }
}