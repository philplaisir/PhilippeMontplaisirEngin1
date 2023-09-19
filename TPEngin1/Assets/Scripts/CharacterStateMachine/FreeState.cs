using UnityEngine;

// TODO
// Checker pour quand 3 touches en m�me temps
// Checker pour faire fonctionner la limite solidement, actuellement la velocit� est limit� mais d�passe toujours un peu la limite que je lui donne
// Facultatif Essayer d'impl�menter d'autres types de d�placements (relatif au personnag, tank control)
// Facultatif Essayer d'ajouter contr�le avec manette

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
        //Debug.Log("Velocity before" + m_stateMachine.RB.velocity.magnitude);
        
        // CHARACTER MOVEMENT RELATIVE TO CAMERA
        CharacterControllerFU();
        KeepCharacterOnGroundFU();

        // Mettre l'update de l'animation

        //Debug.Log("Velocity " + m_stateMachine.RB.velocity.magnitude);
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
        //    //Si je suis ici c'est que je suis pr�sentement dans le jump state et teste si je peux entrer dans FreeState
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
        if (!Input.GetKey(KeyCode.W) && 
            !Input.GetKey(KeyCode.A) && 
            !Input.GetKey(KeyCode.S) && 
            !Input.GetKey(KeyCode.D))
        {
            MovementDeceleration();            
            return;
        }
        if (Input.anyKey)
        {
            ReorientCharacterTowardsChameraDirection();
        }
        if(IsTwoOrMoreReverseInputsInputedSimultaneouslyOneRelativeToCamera())
        {
            MovementDeceleration();
            return;
        }

        // hit.normal

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

        m_stateMachine.RB.AddForce(movementVector * m_stateMachine.GroundAccelerationValue, ForceMode.Acceleration);

        //m_stateMachine.UpdateAnimatorValues(new Vector2(0, forwardComponent));

        if (movementVector.magnitude > 0)
        {
            VelocityRegulatorBasedOnLimitsAndInputs();
            //VelocityRegulatorBasedOnLimitsTwo(projectedVectorForward, projectedVectorRight);
        }
    }

    private void MovementDeceleration()
    {
        if (m_stateMachine.RB.velocity.magnitude < 0.1f)
        {
            m_stateMachine.RB.velocity = Vector3.zero;
            return;
        }
        Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
        m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);
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
        if(m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxStrafeVelocity &&
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
        // Peut-�tre avec un abs serait mieux

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


