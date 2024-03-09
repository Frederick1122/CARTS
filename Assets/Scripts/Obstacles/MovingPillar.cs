using DG.Tweening;
using UnityEngine;

namespace Obstacles
{
    public class MovingPillar : AbstractObstacle
    {
        [SerializeField] private float _startDelay = 0f;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _height = 1f;
        
        private void Start()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence.AppendInterval(_startDelay);
            sequence.Append(transform.DOMoveY(_height + transform.position.y,  _duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine));
            sequence.Play();
        }
    }
}