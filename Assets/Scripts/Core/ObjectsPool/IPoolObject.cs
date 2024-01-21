using System;

public interface IPoolObject
{
    event Action<IPoolObject> OnObjectNeededToDeactivate;
    void ResetBeforeBackToPool();
}
