using UnityEngine;

public class EnemyHitState : EnemyState
{
    public override void OnEnter()
    {        
        m_stateMachine.Animator.SetTrigger("Hit");
        Debug.Log("Enter enemy state : HitState");
    }

    public override void OnExit()
    {
        m_stateMachine.IsHit = false;
        Debug.Log("Exit enemy state : HitState");
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
