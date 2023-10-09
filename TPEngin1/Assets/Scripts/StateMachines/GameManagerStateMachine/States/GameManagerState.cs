using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManagerState : IState
{
    protected GameManagerSM m_stateMachine;

    public void OnStart(GameManagerSM stateMachine)
    {
        m_stateMachine = stateMachine;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual bool CanEnter(CharacterState currentState, EnemyState currentEnemyState)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CanExit()
    {
        throw new System.NotImplementedException();
    }

}
