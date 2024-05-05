using System.Collections;
using System.Collections.Generic;
using UI.Windows.Tutorial;
using UnityEngine;

public class TutorialWindowController : UIController
{
    [SerializeField] private List<Sprite> _tutorialSprites = new();

    public override void Show()
    {
        UpdateView();
        base.Show();
    }

    protected override UIModel GetViewData()
    {
        return new TutorialWindowModel(_tutorialSprites);
    }
}
