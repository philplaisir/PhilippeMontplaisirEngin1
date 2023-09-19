using UnityEngine;

// TODO
// Checker pour quand 3 touches en même temps
// Checker pour faire fonctionner la limite solidement, actuellement la velocité est limité mais dépasse toujours un peu la limite que je lui donne
// Facultatif Essayer d'implémenter d'autres types de déplacements (relatif au personnag, tank control)
// Facultatif Essayer d'ajouter contrôle avec manette

public class FreeState : CharacterState
{
    private float m_turnSmoothVelocity;
    //private float m_accelerationValue;


    public override void OnEnter()
    {
        Debug.Log("Enter state: FreeState\n");
        //m_accelerationValue = m_stateMachine.GroundAccelerationValue;
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

    public override bool CanEnter(/*CharacterState currentState*/)
    {
        // Fak dans le fond le can enter c'est pour entrer dans la current State
        // On met tous les states qui peuvent entrer dans le currentSate ici pour essayer de rentrer 
        // Si le currentState est jumpState on entre
        //var jumpState = (JumpState)currentState;
        //if (jumpState != null) 
        //{
        //    //Si je suis ici c'est que je suis présentement dans le jump state et teste si je peux entrer dans FreeState
        //
        //    //Je ne peux entrer dans le FreeState que si je touche le sol
        //    return m_stateMachine.IsInContactWithFloor();
        //
        //}

        return m_stateMachine.IsInContactWithFloor();


        //return false;
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
        MovementDeceleration();
                
        if (Input.anyKey)
        {
            ReorientCharacterTowardsChameraDirection();
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

        DiagonalVelocityLimitsCalculator(movementVector, projectedVectorForward, projectedVectorRight);
        AddForceToMovementVector(movementVector);
        CalculationsForAnimation(projectedVectorForward, projectedVectorRight);        

        if (movementVector.magnitude > 0)
        {
            VelocityRegulatorBasedOnLimitsAndInputs();
            //VelocityRegulatorBasedOnLimitsTwo(projectedVectorForward, projectedVectorRight);
        }

        // For display in editor
        m_stateMachine.MovementDirectionVector = movementVector;

    }

    private void AddForceToMovementVector(Vector3 movementVector)
    {
        m_stateMachine.RB.AddForce(movementVector * m_stateMachine.GroundAccelerationValue, ForceMode.Acceleration);
    }

    private void CalculationsForAnimation(Vector3 projectedVec3Forward, Vector3 projectedVec3Right)
    {
        float forwardComponent = Vector3.Dot(m_stateMachine.RB.velocity, projectedVec3Forward);
        float rightComponent = Vector3.Dot(m_stateMachine.RB.velocity, projectedVec3Right);

        Vector2 animationComponents = new Vector2(rightComponent, forwardComponent);
        m_stateMachine.UpdateFreeStateAnimatorValues(animationComponents);
    }

    private void MovementDeceleration()
    {
        if ((!Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.S) &&
            !Input.GetKey(KeyCode.D)) ||
            IsTwoOrMoreReverseInputsInputedSimultaneouslyOneRelativeToCamera())
        {
            if (m_stateMachine.RB.velocity.magnitude < 0.3f)
            {
                m_stateMachine.RB.velocity = Vector3.zero;
                return;
            }
            Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);

        }            
    }

    private void ReorientCharacterTowardsChameraDirection()
    {
        m_stateMachine.GameObject.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(m_stateMachine.Transform.transform.eulerAngles.y, m_stateMachine.Camera.transform.eulerAngles.y, ref m_turnSmoothVelocity, m_stateMachine.TurnSmoothTime);
    }

    private void VelocityRegulatorBasedOnLimitsAndInputs()
    {
        if (m_stateMachine.RB.velocity.magnitude < m_stateMachine.MaxForwardVelocity &&
            m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxStrafeVelocity &&
            ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))))
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxForwardDiagonalsVelocity;
            return;
        }
        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxBackwardVelocity &&
            Input.GetKey(KeyCode.S) ||
            (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) ||
            (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)))
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxBackwardVelocity;
            return;
        }
        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxStrafeVelocity &&
            (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxStrafeVelocity;
            return;
        }

        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxForwardVelocity &&
            Input.GetKey(KeyCode.W))
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxForwardVelocity;
            return;
        }
    }

    private void DiagonalVelocityLimitsCalculator(Vector3 movementVector, Vector3 projectedVectorForward, Vector3 projectedVectorRight)
    {
        float forwardComponent = Vector3.Dot(movementVector, projectedVectorForward);
        float sideComponent = Mathf.Abs(Vector3.Dot(movementVector, projectedVectorRight));
        float componentsTotal = forwardComponent + sideComponent;

        float forwardRatio = forwardComponent / componentsTotal;
        float sideRatio = sideComponent / componentsTotal;

        m_stateMachine.MaxForwardDiagonalsVelocity = forwardRatio * m_stateMachine.MaxForwardVelocity + sideRatio * m_stateMachine.MaxStrafeVelocity;
    }

    private void GroundVelocityLimitsCalculator()
    {

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


