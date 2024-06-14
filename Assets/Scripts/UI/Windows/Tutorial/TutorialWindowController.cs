using System.Collections;
using System.Collections.Generic;
using Knot.Localization;
using UI.Windows.Tutorial;
using UnityEngine;

public class TutorialWindowController : UIController
{
    [SerializeField] private List<KnotTextKeyReference> _tutorialStages;
    
    public override void Show()
    {
        UpdateView();
        base.Show();
    }

    protected override UIModel GetViewData()
    {
        return new TutorialWindowModel(_tutorialStages);
    }
}
