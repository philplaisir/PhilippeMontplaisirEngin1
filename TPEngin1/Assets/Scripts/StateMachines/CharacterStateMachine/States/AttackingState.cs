using UnityEngine;

public class AttackingState : CharacterState
{
    private Animator m_animator;
    private float m_delay; 

    public override void OnEnter()
    {
        Debug.Log("Enter state: AttackingState");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();

        m_stateMachine.AttackHitBox.SetActive(true);
        m_animator.SetTrigger("Attacking");
        m_stateMachine.Attacking = true;
        m_delay = 0.4f;        
    }

    public override void OnExit()
    {
        m_stateMachine.AttackHitBox.SetActive(false);
        Debug.Log("Exit state: AttackingState");
    }

    public override void OnUpdate()
    {
        m_delay -= Time.deltaTime;

        if (m_delay <= 0)
        {
            m_stateMachine.Attacking = false;
        }

        //Debug.Log(m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        //{
        //    m_stateMachine.Attacking = false;        
        //}
    }

    public override void OnFixedUpdate()
    {
        Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
        m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);
    }    

    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState)
        {
            return Input.GetKeyDown(KeyCode.E);
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
        //if (m_stateMachine.IsStunned)
        //{
        //    return true;
        //}
        return false;

    }

}
