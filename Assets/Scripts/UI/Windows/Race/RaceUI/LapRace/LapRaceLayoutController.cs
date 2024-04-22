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

        private LapRaceState _lapRaceState;
        private bool _isRaceActive = false;

        public override void Init()
        {
            _lapRaceState = (LapRaceState)RaceManager.Instance.GetState(RaceType.LAP_RACE);
            _lapRaceState.OnStartAction += Reset;
            _lapRaceState.OnFinishAction += Stop;
            
            GetView<LapRaceLayoutView>().OnNeedToPause += Pause;

            _lapCounterController.Init();
            _positionCounterController.Init();
            base.Init();
        }

        private void OnDestroy()
        {
            if (_lapRaceState != null)
            {
                _lapRaceState.OnStartAction -= Reset;
                _lapRaceState.OnFinishAction -= Stop;
            }
            
            GetView<LapRaceLayoutView>().OnNeedToPause -= Pause;
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
            
            if (!_isRaceActive)
                return;
            
            _model.distance = _lapRaceState.GetPlayerPassedDistance();
            UpdateView();
        }

        private void Pause()
        {
            RaceManager.Instance.PauseRace();
        }

        private void Reset()
        {
            _isRaceActive = true;
            _model.maxDistance = _lapRaceState.GetLapDistance() * _lapRaceState.GetMaxLapCount();
            _lapCounterController.Show();
            _positionCounterController.Show();
        }

        private void Stop()
        {
            _isRaceActive = false;
        }
    }
}