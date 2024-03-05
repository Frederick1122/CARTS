using UnityEngine;

public class FpsLocker : MonoBehaviour
{
    [SerializeField, Range(10, 300)] private int _frameRate = 75;

    private void Awake() =>
        SetUpFrameRate();

    public void SetUpFrameRate() => Application.targetFrameRate = _frameRate;
}
