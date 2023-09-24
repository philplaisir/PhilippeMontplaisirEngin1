using UnityEngine;

//TODO Checker pour plus de précision jump

public class JumpState : CharacterState
{
    private Animator m_animator;
    private const float STATE_EXIT_TIMER = 0.2f;
    private float m_currentStateTimer = 0.0f;
    private float m_turnSmoothVelocity;
    private float m_jumpingTimer = 0.0f;

    [SerializeField]
    private float m_gravityValue = 30.0f;
    
        

    public override void OnEnter()
    {
        Debug.Log("Enter state: JumpState\n");
                
        m_stateMachine.RB.AddForce(Vector3.up * m_stateMachine.JumpIntensity, ForceMode.Acceleration);
        m_currentStateTimer = STATE_EXIT_TIMER;
        m_stateMachine.UpdateFreeStateAnimatorValues(new Vector2(0, 0));
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        m_animator.SetTrigger("Jump");
        m_jumpingTimer = 0.0f;
    }

    public override void OnExit()
    {
        m_stateMachine.IsJumpingForTooLong = false;
        Debug.Log("Exit state: JumpState\n");
    }

    public override void OnFixedUpdate()
    {
        m_stateMachine.RB.AddForce(Vector3.down * m_stateMachine.FallGravity, ForceMode.Acceleration);
        CharacterControllerJumpFU();
    }

    public override void OnUpdate()
    {
        m_animator.SetBool("TouchGround", false);

        m_currentStateTimer -= Time.deltaTime;
        m_jumpingTimer += Time.deltaTime;

        if (m_jumpingTimer > 1.5f)
        {
            m_stateMachine.IsJumpingForTooLong = true;
        }

    }

    public override bool CanEnter(CharacterState currentState)
    {
        

        if (currentState is FreeState)
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;

    }

    public override bool CanExit()
    {
        return m_currentStateTimer <= 0;
    }


    private void CharacterControllerJumpFU()
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
            VelocityRegulatorBasedOnLimits();
            
        }
    }

    private void ReorientCharacterTowardsChameraDirection()
    {
        m_stateMachine.GameObject.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(m_stateMachine.Transform.transform.eulerAngles.y, m_stateMachine.Camera.transform.eulerAngles.y, ref m_turnSmoothVelocity, m_stateMachine.TurnSmoothTime);
    }

    private void VelocityRegulatorBasedOnLimits()
    {

    }


}
