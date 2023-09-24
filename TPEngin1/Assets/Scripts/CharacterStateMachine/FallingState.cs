using UnityEngine;

public class FallingState : CharacterState
{
    private Animator m_animator;
    private float m_fallingTimer;
    
    

    public override void OnEnter()
    {
        Debug.Log("Enter state: FallingState\n");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        m_animator.SetBool("TouchGround", false);
        m_fallingTimer = 0.0f;

    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FallingState\n");
    }

    public override void OnFixedUpdate()
    {
        if (m_fallingTimer <= 0.5 && !m_stateMachine.IsJumpingForTooLong)
        {
            CharacterControllerFallingFU();
        }
        m_stateMachine.RB.AddForce(Vector3.down * m_stateMachine.FallGravity, ForceMode.Acceleration);
    }

    public override void OnUpdate()
    {
        m_fallingTimer += Time.deltaTime;
        m_animator.SetBool("TouchGround", false);
    }

    public override bool CanEnter(CharacterState currentState)
    {
        if (currentState is JumpState)
        {
            return !m_stateMachine.IsInContactWithFloor() && m_stateMachine.IsLosingAltitude && m_stateMachine.IsJumpingForTooLong; 
        }
        if (currentState is FreeState)
        {
            return !m_stateMachine.IsInContactWithFloor();
        }

        return false;        
    }
    
    public override bool CanExit()
    {
        return m_stateMachine.IsInContactWithFloor();        
    }

    private void CharacterControllerFallingFU()
    {
        if (Input.anyKey)
        {
            //ReorientCharacterTowardsChameraDirection();
        }
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


        if (movementVector.magnitude > 0)
        {
            //VelocityRegulatorBasedOnLimits();

        }
    }
}
