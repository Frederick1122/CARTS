using UnityEngine;

public abstract class UIController<T, T2> : MonoBehaviour, IUiController where T : UIView<T2>, new() where T2 : UIModel
{
    [SerializeField] protected T _view;

    public virtual void Show()
    {
        _view.Show();
    }

    public virtual void Hide()
    {
        _view.Hide();
    }

    public virtual void Init()
    {
        _view.Init(GetViewData());
    }

    public virtual void UpdateView()
    {

    }

    public virtual void UpdateView(T2 uiModel)
    {
        _view.UpdateView(uiModel);
    }

    protected abstract T2 GetViewData();
}

public interface IUiController
{
    public void Show() { }

    public virtual void Hide() { }

    public virtual void Init() { }

    public virtual void UpdateView() { }
}