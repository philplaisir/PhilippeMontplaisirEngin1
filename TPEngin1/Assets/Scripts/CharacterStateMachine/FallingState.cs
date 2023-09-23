using UnityEngine;

public class FallingState : CharacterState
{

    private Animator m_animator;
    
    

    public override void OnEnter()
    {
        Debug.Log("Enter state: FallingState\n");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        m_animator.SetBool("TouchGround", false);

    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FallingState\n");
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

        if (currentState is FreeState)
        {
            return !m_stateMachine.IsInContactWithFloor();
        }


        return false;

        //return !m_stateMachine.IsInContactWithFloor();
    }

    public override bool CanExit()
    {
        return m_stateMachine.IsInContactWithFloor();
        
    }
}
