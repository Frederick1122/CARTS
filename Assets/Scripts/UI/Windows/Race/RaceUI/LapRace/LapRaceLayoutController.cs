using Race.RaceManagers;
using UnityEngine;

namespace UI.Windows.LapRace
{
    public class LapRaceLayoutController : RaceLayoutController
    {
        public override RaceType raceType 
        {
            get { return RaceType.LAP_RACE; }
        }

        private readonly LapRaceLayoutModel _model = new();
        [SerializeField] private LapCounterController _lapCounterController;
        [SerializeField] private PositionCounterController _positionCounterController;

        public override void Init()
        {
            GetView<LapRaceLayoutView>().OnNeedToPause += Pause;

            _lapCounterController.Init();
            _positionCounterController.Init();
            base.Init();
        }

        private void OnDestroy() =>
            GetView<LapRaceLayoutView>().OnNeedToPause -= Pause;

        public override void Show()
        {
            _lapCounterController.Show();
            _positionCounterController.Show();
            base.Show();
        }

        public override void Hide()
        {
            _lapCounterController.Hide();
            _positionCounterController.Hide();
            base.Hide();
        }

        protected override UIModel GetViewData()
        {
            return _model;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Pause();
        }

        private void Pause() => RaceManager.Instance.PauseRace();
    }
}