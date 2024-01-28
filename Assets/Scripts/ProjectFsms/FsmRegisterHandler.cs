using Core.FSM;
using System;
using System.Collections.Generic;
using Zenject;

namespace ProjectFsms
{
    public class FsmRegisterHandler : IInitializable, IDisposable
    {
        private readonly List<IFsm> _fsms;
        private readonly FsmManager _fsmManager;

        public FsmRegisterHandler(
            // We need to use InjectSources.Local here, otherwise we will
            // add any project context modals again in each scene
            [Inject(Source = InjectSources.Local)]
            List<IFsm> fsms, FsmManager fsmManager)
        {
            _fsms = fsms;
            _fsmManager = fsmManager;
        }

        public void Initialize()
        {
            foreach (var fsm in _fsms)
                _fsmManager.AddNewFsm(fsm);
        }

        public void Dispose()
        {
            // We don't want ModalManager to retain references to Modals defined in unloaded scenes
            // (dispose is executed on scene unload)
            foreach (var fsm in _fsms)
                _fsmManager.RemoveFsm(fsm);
        }
    }
}