using UnityEngine;

// TODO
// Checker pour quand 3 touches en même temps
// Checker pour faire fonctionner la limite solidement, actuellement la velocité est limité mais dépasse toujours un peu la limite que je lui donne
// Facultatif Essayer d'implémenter d'autres types de déplacements (relatif au personnag, tank control)
// Facultatif Essayer d'ajouter contrôle avec manette
// Facultatif mettre un run avec le shift

public class FreeState : CharacterState
{
    private float m_turnSmoothVelocity;
    
        

    public override void OnEnter()
    {
        Debug.Log("Enter state: FreeState\n");        
       
    }

    public override void OnUpdate()
    {
        CalculateAngleUnderCharacter();


    }

    public override void OnFixedUpdate()
    {
        // CHARACTER MOVEMENT RELATIVE TO CAMERA
        CharacterControllerFU();
        KeepCharacterOnGroundFU();

    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FreeState\n");
    }

    public override bool CanEnter(CharacterState currentState)
    {
        
        if (currentState is JumpState || currentState is LeavingGroundState || currentState is OnGroundState) 
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

    private void CalculateAngleUnderCharacter()
    {
        RaycastHit hit;
        if (Physics.Raycast(m_stateMachine.transform.position, -m_stateMachine.transform.up, out hit, 1.0f))
        {
            float slopeAngleUnderCharacter = Vector3.Angle(hit.normal, Vector3.up);
            m_stateMachine.FloorAngleUnderCharacter = slopeAngleUnderCharacter;
            //Debug.Log("Slope Angle: " + m_slopeAngleUnderCharacter);
        }
    }

    private void KeepCharacterOnGroundFU()
    {
        // To keep character always on terrain when terrain is angled
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
        
        Vector3 movementVector = new Vector3(0, m_stateMachine.RB.velocity.y, 0); // pourrait être un zero
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
            //VelocityRegulatorBasedOnLimitsTwo(projectedVectorForward, projectedVectorRight);
        }

        CalculationsForAnimation(projectedVectorForward, projectedVectorRight);

        // For display in editor
        m_stateMachine.MovementDirectionVector = movementVector;

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
            IsTwoOrMoreReverseInputsInputedSimultaneouslyOneRelativeToCamera())
        {
            if (m_stateMachine.RB.velocity.magnitude < 0.4f)
            {
                m_stateMachine.RB.velocity = new Vector3(0, m_stateMachine.RB.velocity.y, 0);
                return;
            }
            Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);

            //Vector3 decelerationVector = new Vector3(m_stateMachine.RB.velocity.x, 0, m_stateMachine.RB.velocity.z);
            //decelerationVector.Normalize();
            //m_stateMachine.RB.AddForce(-decelerationVector * m_stateMachine.DecelerationValue, ForceMode.Acceleration);

        }            
    }

    private void ReorientCharacterWhenMoving()
    {
        m_stateMachine.GameObject.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(m_stateMachine.Transform.transform.eulerAngles.y, m_stateMachine.Camera.transform.eulerAngles.y, ref m_turnSmoothVelocity, m_stateMachine.TurnSmoothTime);
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

    private bool IsTwoOrMoreReverseInputsInputedSimultaneouslyOneRelativeToCamera()
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

    private void VelocityRegulatorBasedOnLimitsTwo(Vector3 projectedVectorForward, Vector3 projectedVectorRight)
    {
        float forwardDot = Vector3.Dot(m_stateMachine.RB.velocity.normalized, projectedVectorForward);
        float rightDot = Vector3.Dot(m_stateMachine.RB.velocity.normalized, projectedVectorRight);
        // Peut-être avec un abs serait mieux

        if (forwardDot > 0.9f)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxForwardVelocity;
            return;
        }
        else if (rightDot > 0.85f || rightDot < -0.85f)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxStrafeVelocity;
        }
    }



}


