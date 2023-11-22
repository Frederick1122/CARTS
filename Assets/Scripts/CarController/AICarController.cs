using System.Collections;
using UnityEngine;

public class AICarController : CarController
{
    [SerializeField] private int _maxStackTime = 5; 
    private bool _isStack;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        CheckIfStack();
    }

    private void CheckIfStack()
    {
        if (_rb.velocity.magnitude <= 0.1f)
        {
            _isStack = true;
            StartCoroutine(ResetCarCuro());
        }
        else
        {
            _isStack = false;
            StopCoroutine(ResetCarCuro());
        }
    }

    private IEnumerator ResetCarCuro()
    {
        int stackTime = _maxStackTime;

        while (stackTime > 0)
        {
            if (_isStack == false)
                yield break;

            yield return new WaitForSeconds(1);
            stackTime--;
        }

        if (_isStack == false)
            yield break;

        ResetCar();
    }
}
