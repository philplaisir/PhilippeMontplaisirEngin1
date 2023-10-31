using UnityEngine;

public class LeavingGroundState : CharacterState
{
    private Animator m_animator;
    private float m_maxFallDistance;

    public override void OnEnter()
    {
        Debug.Log("Character entering state: LeavingGroundState\n");

        m_stateMachine.LeavingGroundStartingPosition = m_stateMachine.transform.position;
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        
        m_animator.SetBool("TouchGround", false);
    }

    public override void OnExit()
    {
        Debug.Log("Character exiting state: LeavingGroundState\n");
    }

    public override void OnUpdate()
    {        
    }

    public override void OnFixedUpdate()
    {
        CharacterControllerInAirFU();        
    }    

    public override bool CanEnter(IState currentState)
    {
        if (currentState is FreeState)
        {
            return !m_stateMachine.IsInContactWithFloor();
        }

        return false;
    }

    public override bool CanExit()
    {        
        if (Mathf.Abs(m_stateMachine.transform.position.y - m_stateMachine.LeavingGroundStartingPosition.y) >= m_maxFallDistance)
        {
            return true;
        }
        if (m_stateMachine.IsInContactWithFloor())
        {            
            return true;
        }
        if (Input.GetKey(KeyCode.F) && !m_stateMachine.IsInContactWithFloor())
        {
            // Get stunned in air testing
            return true;
        }
        
        return false;        
    }

    //88888888888888888888888888888888888888888888888888888

    private void CharacterControllerInAirFU()
    {        
        Vector3 movementVector = Vector3.zero;
        Vector3 projectedVectorForward = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        Vector3 projectedVectorRight = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up);

        if (Input.GetKey(KeyCode.W))
        {
            movementVector += projectedVectorForward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementVector += -projectedVectorForward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementVector += -projectedVectorRight;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementVector += projectedVectorRight;
        }

        movementVector.Normalize();

        m_stateMachine.RB.AddForce(movementVector * m_stateMachine.FallingAccelerationXZ, ForceMode.Acceleration);
    }
}
