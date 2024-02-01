using CameraManger.Lobby;
using Core.FSM;
using System.Collections;
using System.Collections.Generic;
using UI.Windows.MapSelection;
using UI;
using UnityEngine;
using UI.Windows.Garage;

public class GarageState : FsmState
{
    public GarageState(Fsm fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        base.Enter();
        LobbyCameraManager.Instance.SwitchCamera(CameraPositions.Garage);
        UIManager.Instance.GetLobbyUi().ShowWindow(typeof(GarageWindowController));
    }
}
