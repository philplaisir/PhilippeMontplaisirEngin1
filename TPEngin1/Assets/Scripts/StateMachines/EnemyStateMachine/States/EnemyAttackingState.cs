using UnityEngine;

public class EnemyAttackingState : EnemyState
{
    private Animator m_animator;
    private float m_delay;

    public override void OnEnter()
    {
        Debug.Log("Enemy entering state: EnemyAttackingState");

        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        
        m_animator.SetTrigger("Attacking");
        m_stateMachine.Attacking = true;
        m_delay = 0.4f;
    }

    public override void OnExit()
    {        
        Debug.Log("Enemy exiting state: EnemyAttackingState");
    }

    public override void OnUpdate()
    {
        m_delay -= Time.deltaTime;
        
        if (m_delay <= 0)
        {
            m_stateMachine.Attacking = false;
        }        
    }

    public override void OnFixedUpdate()
    {        
    }

    public override bool CanEnter(IState currentState)
    {
        if (currentState is EnemyFreeState)
        {
            return Input.GetKeyDown(KeyCode.Q);
        }
        return false;
    }

    public override bool CanExit()
    {
        if (!m_stateMachine.Attacking)
        {
            return true;
        }
        if (m_stateMachine.IsHit)
        {
            return true;
        }

        return false;
    }
}
