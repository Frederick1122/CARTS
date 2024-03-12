using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowManager : MonoBehaviour
{
    protected Dictionary<Type, IUiController> _controllers = new();

    protected abstract void AddControllers();

    public virtual void Init()
    {
        AddControllers();
        InitAll();
        HideAll();
    }

    private void InitAll()
    {
        foreach (var controller in _controllers.Values)
            controller.Init();
    }

    private void HideAll()
    {
        foreach (var controllerPair in _controllers)
            controllerPair.Value.Hide();
    }

    public IUiController ShowWindow(Type T, bool hideOtherWindows = true)
    {        
        if (hideOtherWindows)
            HideAll();

        var controller = _controllers[T];
        controller.Show();
        return controller;
    }

    public void HideWindow(Type T)
    {
        _controllers[T].Hide();
    }
}