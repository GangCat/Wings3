using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static PlayerController;
public enum E_State
{
    NONE = -1,
    IDLE,
    MOVE,
    DAMAGED,
    INTERACT,
    LENGTH
}

public class PlayerStateMachine
{
    private PlayerState currentState;
    private PlayerState[] arrPlayerStates;

    public PlayerState CurrnentState => currentState;

    public PlayerStateMachine(PlayerData _playerData)
    {
        arrPlayerStates = new PlayerState[(int)E_State.LENGTH];
        arrPlayerStates[(int)E_State.IDLE] = new PlayerIdleState(_playerData);
        arrPlayerStates[(int)E_State.MOVE] = new PlayerMoveState(_playerData);
        currentState = arrPlayerStates[(int)E_State.IDLE];
    }


    public void ChangeState(E_State _newState)
    {

        currentState.Exit();
        currentState = arrPlayerStates[(int)_newState];
        currentState.Enter();
    }

    public void Update()
    {
            currentState.LogicUpdate();
    }

    public void FixedUpdate()
    {
            CurrnentState.PhysicsUpdate();
    }

}
