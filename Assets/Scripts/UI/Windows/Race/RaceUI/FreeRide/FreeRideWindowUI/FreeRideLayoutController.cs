using Race.RaceManagers;
using UnityEngine;

namespace UI.Windows.FreeRide
{
    public class FreeRideLayoutController : RaceLayoutController
    {
        public override RaceType raceType 
        {
            get { return RaceType.FREE_RIDE; }
        }

        [Header("Own controllers")]
        [SerializeField] private ScoreController _scoreController;

        private readonly FreeRideLayoutModel _model = new();

        public override void Init()
        {
            GetView<FreeRideLayoutView>().OnNeedToPause += Pause;

            _scoreController.Init();
            base.Init();
        }

        private void OnDestroy() =>
            GetView<FreeRideLayoutView>().OnNeedToPause -= Pause;

        public override void Show()
        {
            _scoreController.Show();
            base.Show();
        }

        public override void Hide()
        {
            _scoreController.Hide();
            base.Hide();
        }

        public void ResetScore()
        {
            _scoreController.ResetScore();
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
