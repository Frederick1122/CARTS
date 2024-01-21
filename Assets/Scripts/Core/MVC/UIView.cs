using UnityEngine;

public class UIView<T> : MonoBehaviour where T : UIModel
{
    virtual public void Show()
    {
        gameObject.SetActive(true);

        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    virtual public void Hide()
    {
        gameObject.SetActive(false);

        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    virtual public void Init(T uiModel)
    {

    }

    virtual public void UpdateView(T uiModel)
    {

    }

    virtual public void Terminate()
    {

    }
}
