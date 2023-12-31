using UnityEngine;

public class UIController<T, T2> : MonoBehaviour where T : UIView<T2> where T2 : UIModel
{
    [SerializeField] protected T _view;

    virtual public void Show()
    {
        _view.Show();
    }

    virtual public void Hide()
    {
        _view.Hide();
    }

    virtual public void Init()
    {

    }

    virtual public void UpdateView()
    {

    }

    virtual public void Terminate()
    {

    }
}
