using CameraManger.Lobby;
using Core.FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageState : FsmState
{
    public GarageState(Fsm fsm) : base(fsm)
    {
    }

    public override void Enter()
    {
        LobbyCameraManager.Instance.SwitchCamera(CameraPositions.Garage);
        base.Enter();
    }
}
