using UnityEngine;

public class OnGroundState : CharacterState
{
    private Animator m_animator;
    private float m_onGroundDelay;

    public override void OnEnter()
    {
        Debug.Log("Character entering state: OnGroundState\n");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();       
        m_onGroundDelay = 1.0f;        
        m_animator.SetTrigger("Stunned");
    }

    public override void OnExit()
    {        
        Debug.Log("Character exiting state: OnGroundState\n");       
    }    

    public override void OnUpdate()
    {        
        m_onGroundDelay -= Time.deltaTime;
    }

    public override void OnFixedUpdate()
    {
    }

    public override bool CanEnter(IState currentState)
    {        
        if (currentState is FallingState )
        {
            return m_stateMachine.IsInContactWithFloor();
        }
        if (currentState is FreeState)
        {
            // For testing purpose
            return Input.GetKey(KeyCode.F);
        }
        
        return false;
    }

    public override bool CanExit()
    {
        return m_onGroundDelay <= 0;        
    }

}
