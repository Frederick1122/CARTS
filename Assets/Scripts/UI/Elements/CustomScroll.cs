using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    public abstract class CustomScroll<T, T2, T3> : MonoBehaviour where T : UIController<T2, T3> where T2 : UIView<T3>, new() where T3 : UIModel 
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private T _uiControllerPrefab;

        protected List<T> _activeControllers = new();
        protected List<T> _hidingControllers = new();

        public void HideAll()
        {
            foreach (var controller in _activeControllers) 
                controller.Hide();

            _hidingControllers.AddRange(_activeControllers);
            _activeControllers.Clear();
        }

        public virtual void AddElement(T3 uiModel)
        {
            T controller;
            if (_hidingControllers.Count > 0)
            {
                controller = _hidingControllers[0];
                _hidingControllers.RemoveAt(0);
            }
            else
            {
                controller = CreateElement();
                controller.Init();
            }
        
            controller.Show();
            controller.UpdateView(uiModel);
            _activeControllers.Add(controller);
        }

        public void AddRange(List<T3> uiModels)
        {
            foreach (var uiModel in uiModels) 
                AddElement(uiModel);
        }

        private T CreateElement()
        {
            return Instantiate(_uiControllerPrefab, _scrollRect.content);
        }
    }
}
