using UnityEngine;

public class HitState : CharacterState
{
    private Animator m_animator;
    private float m_hitStunTimer;



    public override void OnEnter()
    {
        Debug.Log("Enter state: HitState\n");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        m_animator.SetTrigger("Hit");
        m_hitStunTimer = 0.5f;
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: HitState\n");
        m_stateMachine.IsHit = false;
    }

    public override void OnFixedUpdate()
    {
        if (m_stateMachine.RB.velocity.magnitude > 0)
        {
            Vector3 decelerationVector = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.AddForce(-decelerationVector * m_stateMachine.DecelerationValue, ForceMode.Acceleration);
        }        
    }

    public override void OnUpdate()
    {
        m_hitStunTimer -= Time.deltaTime;
    }

    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState || currentState is AttackingState)
        {
            return m_stateMachine.IsHit;            
        }
        return false;        
    }

    public override bool CanExit()
    {
        //if (m_stateMachine.IsStunned)
        //{
        //    return true;
        //}
        return m_hitStunTimer < 0;      
        


    }
}


