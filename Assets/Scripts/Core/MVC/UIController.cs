using UnityEngine;

public abstract class UIController : MonoBehaviour, IUiController
{
    [SerializeField] protected UIView _view;

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
        _view.UpdateView(GetViewData());
    }

    public virtual void UpdateView(UIModel uiModel)
    {
        UpdateView();
    }

    protected abstract UIModel GetViewData();

    protected T GetView<T>() where T : UIView
    {
        return (T) _view;
    }
}

public interface IUiController
{
    public void Show() { }

    public virtual void Hide() { }

    public virtual void Init() { }

    public virtual void UpdateView() { }
}