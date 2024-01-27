using TMPro;
using UnityEngine;

namespace UI.Windows.Race.RaceUI
{
    public class RaceWindowView : UIView<RaceWindowModel>
    {
        [SerializeField] private TMP_Text _lapCounter;
    }
    
    public class RaceWindowModel : UIModel
    {
        public int lapCount;
    }
}