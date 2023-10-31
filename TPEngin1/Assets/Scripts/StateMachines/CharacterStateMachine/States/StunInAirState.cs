using UnityEngine;

public class StunInAirState : CharacterState
{
    private Animator m_animator;
    private float m_stunOnGroundTimer;

    public override void OnEnter()
    {
        Debug.Log("Character entering state: StunInAirState\n");

        m_animator = m_stateMachine.GetComponentInParent<Animator>();        
        m_animator.SetTrigger("Stunned");
        m_stunOnGroundTimer = 1.0f;
    }

    public override void OnExit()
    {
        Debug.Log("Character exiting state: StunInAirState\n");
    }    

    public override void OnUpdate()
    {
       if (m_stateMachine.IsInContactWithFloor())
       {
           m_stunOnGroundTimer -= Time.deltaTime;
       }
    }

    public override void OnFixedUpdate()
    {        
    }

    public override bool CanEnter(IState currentState)
    {
        if (currentState is JumpState || currentState is LeavingGroundState || currentState is FallingState)
        {
            return Input.GetKey(KeyCode.F) && !m_stateMachine.IsInContactWithFloor();
        }
        return false;
    }

    public override bool CanExit()
    {
        return m_stunOnGroundTimer < 0;        
    }
}


