using UnityEngine;

public class EnemyHitState : EnemyState
{
    public override void OnEnter()
    {
        Debug.Log("Enemy entering state : EnemyHitState");
        m_stateMachine.Animator.SetTrigger("Hit");        
    }

    public override void OnExit()
    {
        m_stateMachine.IsHit = false;
        Debug.Log("Enemy exiting state : EnemyHitState");
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
    }    

    public override bool CanEnter(IState currentState)
    {
        if (currentState is EnemyFreeState)
        {            
            return m_stateMachine.IsHit;
        }
        return false;
    }

    public override bool CanExit()
    {
        return true;
    }
}
