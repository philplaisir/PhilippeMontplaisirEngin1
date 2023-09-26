using UnityEngine;

public class FallingState : CharacterState
{
    private Animator m_animator;    
    
    

    public override void OnEnter()
    {
        Debug.Log("Enter state: FallingState\n");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        //m_animator.SetBool("TouchGround", false);
        //m_animator.SetBool("EndFall", false);
        m_animator.SetTrigger("Falling");




    }

    public override void OnExit()
    {

        Debug.Log("Exit state: FallingState\n");
        //m_animator.SetBool("TouchGround", false);
        //m_animator.SetBool("EndFall", true);
        
    }

    public override void OnFixedUpdate()
    {        
        m_stateMachine.RB.AddForce(Vector3.down * m_stateMachine.FallGravity, ForceMode.Acceleration);
    }

    public override void OnUpdate()
    {        
        m_animator.SetBool("TouchGround", false);
    }

    public override bool CanEnter(CharacterState currentState)
    {
        if (currentState is JumpState)
        {
            return !m_stateMachine.IsInContactWithFloor() && m_stateMachine.IsJumpingForTooLong; 
        }
        if (currentState is LeavingGroundState)
        {
            return !m_stateMachine.IsInContactWithFloor();
        }        

        return false;        
    }
    
    public override bool CanExit()
    {
        return m_stateMachine.IsInContactWithFloor();        
    }

    
}
