using UI.Windows.RaceLoadOut;
using UnityEngine;

namespace UI
{
    public class RaceUIManager : UIManager<RaceUIManager>
    {
        [SerializeField] private RaceLoadoutController _raceLoadoutController;

        [ContextMenu("Init")]
        public override void Init()
        {
            base.Init();
            _raceLoadoutController.Show();
        }

        protected override void AddControllers() =>
            _controllers.Add(_raceLoadoutController.GetType(), _raceLoadoutController);
    }
}
