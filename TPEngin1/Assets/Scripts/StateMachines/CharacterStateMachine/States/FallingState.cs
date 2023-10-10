using UnityEngine;

public class FallingState : CharacterState
{
    private Animator m_animator;    

    public override void OnEnter()
    {
        Debug.Log("Enter state: FallingState\n");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        
        m_animator.SetTrigger("Falling");
    }

    public override void OnExit()
    {
        m_animator.ResetTrigger("Falling");
        Debug.Log("Exit state: FallingState\n");        
    }    

    public override void OnUpdate()
    {        
        m_animator.SetBool("TouchGround", false);
    }

    public override void OnFixedUpdate()
    {        
    }

    public override bool CanEnter(IState currentState)
    {
        if (currentState is JumpState)
        {
            if (m_stateMachine.CharacterVelocity.y < 0
                && Mathf.Abs(m_stateMachine.transform.position.y - m_stateMachine.JumpStartingPosition.y) >= m_stateMachine.MaxJumpFallingDistance)
            {
                return true;
            }

            return false;

            //return m_stateMachine.InJumpStateForTooLong; 
        }
        if (currentState is LeavingGroundState)
        {
            if (Mathf.Abs(m_stateMachine.transform.position.y - m_stateMachine.LeavingGroundStartingPosition.y) >= m_stateMachine.MaxLeavingGroundFallingDistance)
            {
                return true;
            }

            return false;

            //return m_stateMachine.LeftGroundForTooLong; 
        }        

        return false;        
    }
    
    public override bool CanExit()
    {
        //if (m_stateMachine.IsStunned)
        //{
        //    return true;
        //}

        return true;
        
        //return m_stateMachine.IsInContactWithFloor();        
    }

    
}
