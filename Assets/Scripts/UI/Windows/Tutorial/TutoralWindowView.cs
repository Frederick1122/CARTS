using Swiper;
using System;
using System.Collections.Generic;
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
            if (_swiper.ElementsCount == 0)
            {
                _swiper.Clear();
                var castModel = (TutorialWindowModel)uiModel;

                foreach (var slide in castModel.TutorialSlides)
                {
                    SwiperData data = new("", slide);
                    _swiper.AddItems(data);
                }
            }

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
        public readonly List<Sprite> TutorialSlides = new();

        public TutorialWindowModel(List<Sprite> tutorialSlides)
        {
            TutorialSlides = tutorialSlides;
        }
    }
}
