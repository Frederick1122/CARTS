using Base;
using System;
using System.Collections.Generic;

public abstract class UIManager<TM> : Singleton<TM>
        where TM : UIManager<TM>
{
    protected Dictionary<Type, IUiController> _controllers = new();

    protected abstract void AddControllers();

    public virtual void Init()
    {
        AddControllers();
        InitAll();
        HideAll();
    }

    protected virtual void InitAll()
    {
        foreach (var controller in _controllers.Values)
            controller.Init();
    }

    protected virtual void HideAll()
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
}