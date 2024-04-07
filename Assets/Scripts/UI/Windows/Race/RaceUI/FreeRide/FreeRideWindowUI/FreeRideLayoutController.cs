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
        
        private FreeRideState _freeRideState;
        private readonly FreeRideLayoutModel _model = new();

        public override void Init()
        {
            _freeRideState = (FreeRideState)RaceManager.Instance.GetState(RaceType.FREE_RIDE);
            _freeRideState.OnFinishAction += _scoreController.ResetScore;
            GetView<FreeRideLayoutView>().OnNeedToPause += Pause;

            _scoreController.Init();
            base.Init();
        }

        private void OnDestroy()
        {
            GetView<FreeRideLayoutView>().OnNeedToPause -= Pause;
            _freeRideState.OnFinishAction -= _scoreController.ResetScore;
        }

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
