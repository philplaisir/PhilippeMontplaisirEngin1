using UnityEngine;

public class LeavingGroundState : CharacterState
{
    private Animator m_animator;
    private float m_timerBeforeFalling;

    public override void OnEnter()
    {
        Debug.Log("Enter state: LeavingGroundState\n");

        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        m_animator.SetBool("TouchGround", false);

        m_timerBeforeFalling = 0.5f;
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: LeavingGroundState\n");
    }

    public override void OnFixedUpdate()
    {
        CharacterControllerInAirFU();
        m_stateMachine.RB.AddForce(Vector3.down * m_stateMachine.FallGravity, ForceMode.Acceleration);
    }

    public override void OnUpdate()
    {
        m_animator.SetBool("TouchGround", false);

        m_timerBeforeFalling -= Time.deltaTime;
    }

    public override bool CanEnter(CharacterState currentState)
    {
        if (currentState is FreeState)
        {
            return !m_stateMachine.IsInContactWithFloor();
        }

        return false;
    }

    public override bool CanExit()
    {
        if (m_stateMachine.IsInContactWithFloor())
        {
            return true;
        }
        if (m_timerBeforeFalling < 0)
        {
            return true;
        }

        return false;        
    }

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
