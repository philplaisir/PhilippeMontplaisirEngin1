using UnityEngine;

public class OnGroundState : CharacterState
{
    private Animator m_animator;

    private float m_onGroundDelay;

    public override void OnEnter()
    {
        Debug.Log("Enter state: OnGroundState\n");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        //m_animator.SetBool("OnGround", true);
        //m_animator.SetTrigger("OnGroundAfterFalling");
        m_onGroundDelay = 1.0f;
        m_stateMachine.IsStunned = false;
        m_animator.SetTrigger("Stunned");
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: OnGroundState\n");
        //m_animator.SetBool("OnGround", false);
        
    }

    public override void OnFixedUpdate()
    {

    }

    public override void OnUpdate()
    {
        //m_animator.SetBool("OnGround", true);
        m_onGroundDelay -= Time.deltaTime;
    }

    public override bool CanEnter(CharacterState currentState)
    {
        if(currentState is StunInAirState)
        {
            return m_stateMachine.IsInContactWithFloor();

        }
        if (currentState is FallingState )
        {
            return m_stateMachine.IsInContactWithFloor();
        }
        if (currentState is FreeState || currentState is AttackingState)
        {
            return m_stateMachine.IsStunned;
        }
        

        return false;

    }

    public override bool CanExit()
    {
        return m_onGroundDelay <= 0;        
    }

}
