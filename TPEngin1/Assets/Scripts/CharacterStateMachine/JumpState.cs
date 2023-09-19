using UnityEngine;

//TODO Checker pour plus de précision jump

public class JumpState : CharacterState
{
    private const float STATE_EXIT_TIMER = 0.2f;
    private float m_currentStateTimer = 0.0f;
    private float m_turnSmoothVelocity;
    private float m_jumpBaseHeight;
    private float m_jumpedHeight;

    public override void OnEnter()
    {
        Debug.Log("Enter state: JumpState\n");

        //Effectuer le saut
        m_stateMachine.RB.AddForce(Vector3.up * m_stateMachine.JumpIntensity, ForceMode.Acceleration);
        m_currentStateTimer = STATE_EXIT_TIMER;
        //m_stateMachine.m_isJumping = true;
        //m_jumpBaseHeight = m_stateMachine.DistanceBetweenCharacterAndFloor;
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: JumpState\n");
    }

    public override void OnFixedUpdate()
    {
        CharacterControllerJumpFU();

    }

    public override void OnUpdate()
    {
        m_currentStateTimer -= Time.deltaTime;
    }

    public override bool CanEnter(/*CharacterState currentState*/)
    {
        // Si on veut essayer var jumpState = TryCast<JumpState>(currentEnter)
        //if (currentSate is FreeState) à privilégier
        //{
        //    return Input.GetKeyDown(KeyCode.Space);
        //}
        //return false;
        //Debug.Log("Entered can enter jump state");
        //This must be run in Update absolutely
        return Input.GetKeyDown(KeyCode.Space);
    }

    public override bool CanExit()
    {
        //float maxJumpInteractionAllowance = m_jumpBaseHeight + 3;
        //float currentMaxHeightAfterJump = maxJumpInteractionAllowance + m_stateMachine.DistanceBetweenCharacterAndFloor;
        //
        //
        //if (m_currentStateTimer <= 0 && m_stateMachine.DistanceBetweenCharacterAndFloor < maxJumpInteractionAllowance + m_stateMachine.DistanceBetweenCharacterAndFloor)
        //{
        //
        //}
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

        m_stateMachine.RB.AddForce(movementVector * m_stateMachine.JumpAccelerationValue, ForceMode.Acceleration);

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
