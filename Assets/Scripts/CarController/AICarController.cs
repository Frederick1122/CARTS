using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AICarController : CarController
{
    [SerializeField] private int _maxStackTime = 5; 
    private bool _isStack;
    private CancellationTokenSource _resetCarCts = new();

    protected override async void FixedUpdate()
    {
        base.FixedUpdate();
        CheckIfStack();
    }

    private void CheckIfStack()
    {
        if (_rb.velocity.magnitude <= 0.1f && !_isStack)
        {
            _isStack = true;
            _resetCarCts = new CancellationTokenSource();
            ResetCarTask().Forget();
        }
        else if (_rb.velocity.magnitude > 0.1f && _isStack)
        {
            _isStack = false;
            _resetCarCts.Cancel();
        }
    }

    private async UniTaskVoid ResetCarTask()
    {
        int stackTime = _maxStackTime;

        while (stackTime > 0 && _isStack)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: _resetCarCts.Token);
            stackTime--;
        }

        if (!_isStack)
            return;

        ResetCar();
    }
}
