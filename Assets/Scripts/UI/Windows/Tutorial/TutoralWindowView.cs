using Swiper;
using System;
using System.Collections.Generic;
using Knot.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Tutorial
{
    public class TutoralWindowView : UIView
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private SwiperController _swiper;

        public override void Init(UIModel uiModel)
        {
            _backButton.onClick.AddListener(RequestForCloseTutorial);

            _swiper.Init();
        }

        public override void UpdateView(UIModel uiModel)
        {
            var castModel = (TutorialWindowModel)uiModel;
            var swiperDatas = new List<SwiperData>();
            
            foreach (var tutorialStage in castModel.TutorialStages)
            {
                swiperDatas.Add(new("", null, tutorialStage.Value));
            }
            
            _swiper.UpdateAll(swiperDatas);
            _swiper.SelectTab(0);
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(RequestForCloseTutorial);
        }

        private void RequestForCloseTutorial()
        {
            Hide();
        }
    }

    public class TutorialWindowModel : UIModel
    {
        public readonly List<KnotTextKeyReference> TutorialStages;

        public TutorialWindowModel(List<KnotTextKeyReference> tutorialStages)
        {
            TutorialStages = tutorialStages;
        }
    }
}
