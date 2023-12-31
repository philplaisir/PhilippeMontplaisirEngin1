using UnityEngine;

public class FreeState : CharacterState
{
    private float m_turnSmoothVelocity;       

    public override void OnEnter()
    {
        Debug.Log("Character entering state: FreeState\n");
    }

    public override void OnExit()
    {
        Debug.Log("Character exiting state: FreeState\n");
    }

    public override void OnUpdate()
    {
        CalculateAngleUnderCharacter();        
    }

    public override void OnFixedUpdate()
    {
        CharacterControllerFU();
        KeepCharacterOnGroundWhenAngledFU();
        UpdateHorizontalMovementsFU();
        UpdateVerticalCameraMovementsBasedOnSecondaryObjectRotationFU();
    }    

    public override bool CanEnter(IState currentState)
    {        
        if (currentState is JumpState || currentState is LeavingGroundState || currentState is GettingUpState || currentState is StandbyState) 
        {            
            return m_stateMachine.IsInContactWithFloor();        
        }
        if (currentState is HitState)
        {
            return m_stateMachine.IsInContactWithFloor();
        }        
        if (currentState is AttackingState)
        {
            if (m_stateMachine.Attacking == false)
            {
                return true;
            }

            return false;
        }        

        return false;                
    }

    public override bool CanExit()
    {
        return true;
    }

    //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888

    private void UpdateHorizontalMovementsFU()
    {        
        float horizontalInput = Input.GetAxis("Mouse X");
        float rotationAmountX = horizontalInput * m_stateMachine.RotationSpeedHorizontal;        
        m_stateMachine.MainCharacter.transform.Rotate(0, rotationAmountX, 0);
    }

    private void UpdateVerticalCameraMovementsBasedOnSecondaryObjectRotationFU()
    {
        float verticalInput = Input.GetAxis("Mouse Y");
        float rotationAmountY = verticalInput * m_stateMachine.RotationSpeedVertical;
        m_stateMachine.ObjectToRotateAround.transform.rotation *= Quaternion.AngleAxis(rotationAmountY, Vector3.right);

        Vector3 currentEulerAngles = m_stateMachine.ObjectToRotateAround.transform.eulerAngles;
        float comparisonAngle = ClampAngle(currentEulerAngles.x);
        float clampedX = Mathf.Clamp(comparisonAngle, m_stateMachine.VerticalCameraLimits.x, m_stateMachine.VerticalCameraLimits.y);
        Vector3 clampedEulerAngles = new Vector3(clampedX, currentEulerAngles.y, currentEulerAngles.z);
        m_stateMachine.ObjectToRotateAround.transform.eulerAngles = clampedEulerAngles;
    }

    private float ClampAngle(float angle)
    {
        if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }

    private void CalculateAngleUnderCharacter()
    {
        RaycastHit hit;

        if (Physics.Raycast(m_stateMachine.transform.position, -m_stateMachine.transform.up, out hit, 1.0f))
        {
            float slopeAngleUnderCharacter = Vector3.Angle(hit.normal, Vector3.up);
            m_stateMachine.FloorAngleUnderCharacter = slopeAngleUnderCharacter;            
        }
    }

    private void KeepCharacterOnGroundWhenAngledFU()
    {        
        RaycastHit hit;

        if (Physics.Raycast(m_stateMachine.transform.position, -Vector3.up, out hit))
        {
            Vector3 slopeNormal = hit.normal;
            Vector3 newVelocity = Vector3.ProjectOnPlane(m_stateMachine.RB.velocity, slopeNormal);
            m_stateMachine.RB.velocity = newVelocity;
        }
    }

    private void CharacterControllerFU()
    {
        DecelerateMovement();
                
        if (Input.anyKey)
        {
            ReorientCharacterWhenMoving();
        }
        
        Vector3 movementVector = new Vector3(0, m_stateMachine.RB.velocity.y, 0); // pourrait �tre un zero
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
        m_stateMachine.RB.AddForce(movementVector * m_stateMachine.GroundAcceleration, ForceMode.Acceleration);        

        if (movementVector.magnitude > 0)
        {
            CalculateDiagonalMaxVelocity(movementVector, projectedVectorForward, projectedVectorRight);
            RegulateVelocity();            
        }

        CalculationsForAnimation(projectedVectorForward, projectedVectorRight);        
    }    

    private void CalculationsForAnimation(Vector3 projectedVec3Forward, Vector3 projectedVec3Right)
    {
        float forwardComponent = Vector3.Dot(m_stateMachine.RB.velocity, projectedVec3Forward);
        float rightComponent = Vector3.Dot(m_stateMachine.RB.velocity, projectedVec3Right);

        Vector2 animationComponents = new Vector2(rightComponent, forwardComponent);
        m_stateMachine.UpdateFreeStateAnimatorValues(animationComponents);
    }

    private void DecelerateMovement()
    {
        if ((!Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.S) &&
            !Input.GetKey(KeyCode.D)) ||
            CheckIfTooManyInputs())
        {
            if (m_stateMachine.RB.velocity.magnitude <= 0)
                return;
            if (m_stateMachine.RB.velocity.magnitude < 0.4f)
            {               
                m_stateMachine.RB.velocity = new Vector3(0, m_stateMachine.RB.velocity.y, 0);                
                return;
            }
            Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);            
        }            
    }

    private void ReorientCharacterWhenMoving()
    {
        m_stateMachine.MainCharacter.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(m_stateMachine.Transform.transform.eulerAngles.y, m_stateMachine.Camera.transform.eulerAngles.y, ref m_turnSmoothVelocity, m_stateMachine.TurnSmoothTime);
    }

    private void RegulateVelocity()
    {   
        float maxVelocity = 0.0f;

        if (((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))))
        {
            maxVelocity = m_stateMachine.MaxForwardDiagonalsVelocity;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            maxVelocity = m_stateMachine.MaxForwardVelocity;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            maxVelocity = m_stateMachine.MaxBackwardVelocity;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            maxVelocity = m_stateMachine.MaxStrafeVelocity;
        }

        if (m_stateMachine.RB.velocity.magnitude > maxVelocity)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= maxVelocity;
        }
    }

    private void CalculateDiagonalMaxVelocity(Vector3 movementVector, Vector3 projectedVectorForward, Vector3 projectedVectorRight)
    {
        float forwardComponent = Vector3.Dot(movementVector, projectedVectorForward);
        float sideComponent = Mathf.Abs(Vector3.Dot(movementVector, projectedVectorRight));
        float componentsTotal = forwardComponent + sideComponent;

        float forwardRatio = forwardComponent / componentsTotal;
        float sideRatio = sideComponent / componentsTotal;

        m_stateMachine.MaxForwardDiagonalsVelocity = forwardRatio * m_stateMachine.MaxForwardVelocity + sideRatio * m_stateMachine.MaxStrafeVelocity;
    }    

    private bool CheckIfTooManyInputs()
    {
        bool returnValue = false;

        //if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) ||
        //    (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))            
        //{
        //    returnValue = true;
        //}

        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) ||
            (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) ||
            (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) ||
            (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S)) ||
            (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W)))
        {
            returnValue = true;
        }

        return returnValue;
    }
}








// TODO
// Checker pour quand 3 touches en m�me temps
// Checker pour faire fonctionner la limite solidement, actuellement la velocit� est limit� mais d�passe toujours un peu la limite que je lui donne
// Facultatif Essayer d'impl�menter d'autres types de d�placements (relatif au personnag, tank control)
// Facultatif Essayer d'ajouter contr�le avec manette
// Facultatif mettre un run avec le shift