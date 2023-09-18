using UnityEngine;

// TODO
// Checker pour quand 3 touches en même temps
// Facultatif Essayer d'implémenter d'autres types de déplacements (relatif au personnag, tank control)
// Facultatif Essayer d'ajouter contrôle avec manette

public class FreeState : CharacterState
{
    private float m_turnSmoothVelocity;



    public override void OnEnter()
    {
        Debug.Log("Enter state: FreeState\n");
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnFixedUpdate()
    {
        Debug.Log("Velocity before" + m_stateMachine.RB.velocity.magnitude);
        
        // CHARACTER MOVEMENT RELATIVE TO CAMERA
        CharacterControllerFU();
        
        //Debug.Log("Velocity " + m_stateMachine.RB.velocity.magnitude);
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FreeState\n");
    }

    public override bool CanEnter()
    {
        //Je ne peux entrer dans le FreeState que si je touche le sol
        return m_stateMachine.IsInContactWithFloor();
        //return false; // à retirer
    }

    public override bool CanExit()
    {
        return true;
    } 
    


    //88888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888888



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

        m_stateMachine.RB.AddForce(movementVector * m_stateMachine.GeneralAccelerationValue, ForceMode.Acceleration);

        if (movementVector.magnitude > 0)
        {
            VelocityRegulatorBasedOnLimitsAndInputs();
            //VelocityRegulatorBasedOnLimitsTwo(projectedVectorForward, projectedVectorRight);
        }
    }

    private void MovementDeceleration()
    {
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

        float forwardRatio = forwardComponent / (forwardComponent + sideComponent);
        float sideRatio = sideComponent / (forwardComponent + sideComponent);

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

















/*
 


// TEST








//if (m_stateMachine.RB.velocity.magnitude > 0.1f)
        //{
        //    Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
        //    m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);
        //}

        //if (!Input.anyKey)
        //{
        //    Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
        //    m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);
        //    Debug.Log("Velocity after max" + m_stateMachine.RB.velocity.magnitude);
        //    return;
        //}

        //if (m_stateMachine.RB.velocity.magnitude == m_stateMachine.MaxForwardVelocity)
        //{
        //    m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized * m_stateMachine.MaxForwardVelocity;
        //}






//if (Input.anyKey)
        //{
        //    if (IsTwoOrMoreReverseInputsInputedSimultaneouslyOneRelativeToCamera())
        //    {
        //        CharacterControllerDeceleration();
        //        return;
        //    }
        //    CharacterControllerRelativeToCameraFUpdate();
        //}
        //else
        //{
        //    CharacterControllerDeceleration();
        //}

        
      //   Application de la vélocité sur 
      //  float forwardComponent  = Vector3.Dot(m_stateMachine.RB.velocity, vectorOnFloor(forward truc truc))
      //
      //   m_stateMachine.UpdateAnimatorValues(new Vector2(0, forwardComponent));

         





private Vector3 GetNormalizedVectorProjectedOnFloor(Vector3 direction)
    {
        Vector3 projectedVector;

        if (direction == Vector3.forward)
        {
            projectedVector = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        }
        else if (direction == Vector3.right)
        {
            projectedVector = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up);
        }
        else
        {
            Debug.LogWarning("Invalid direction! Must be Vector3.forward or Vector3.right.");
            return Vector3.zero; // Return some default value or handle the error accordingly
        }

        projectedVector.Normalize();
        return projectedVector;
    }

    private void CharacterControllerDeceleration()
    {
        if (m_stateMachine.RB.velocity.magnitude > 0.1f)
        {
            Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);
        }
    }



    // CHARACTER CONTROLLER RELATIVE TO CAMERA METHODS
    private void CharacterControllerRelativeToCameraFUpdate()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCameraDiagonals(m_stateMachine.ForwardDiagonalsAccelerationValue, m_stateMachine.MaxForwardDiagonalsVelocity, -1);
            return;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCameraDiagonals(m_stateMachine.ForwardDiagonalsAccelerationValue, m_stateMachine.MaxForwardDiagonalsVelocity, 1);
            return;
        }
        if (Input.GetKey(KeyCode.W))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCamera(Vector3.forward, m_stateMachine.ForwardAccelerationValue, m_stateMachine.MaxForwardVelocity, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCamera(Vector3.forward, m_stateMachine.BackwardAccelerationValue, m_stateMachine.MaxBackwardVelocity, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCamera(Vector3.right, m_stateMachine.StrafeAccelerationValue, m_stateMachine.MaxStrafeVelocity, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ReorientCharacterTowardsChameraDirection();
            CharacterControllerRelativeToCamera(Vector3.right, m_stateMachine.StrafeAccelerationValue, m_stateMachine.MaxStrafeVelocity, 1);
        }
    }

    private bool IsTwoOrMoreReverseInputsInputedSimultaneouslyOneRelativeToCamera()
    {
        // TODO à revérifier, ne fonctionne pas lorsqu'on fait WAS,WSD
        bool returnValue = false;

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

    private void ReorientCharacterTowardsChameraDirection()
    {
        m_stateMachine.GameObject.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(m_stateMachine.Transform.transform.eulerAngles.y, m_stateMachine.Camera.transform.eulerAngles.y, ref m_turnSmoothVelocity, m_stateMachine.TurnSmoothTime);
    }

    private void CharacterControllerRelativeToCamera(Vector3 direction, float accelerationValue, float maxVelocity, int isVectorReversed)
    {
        Vector3 vectorProjectedOnFloorForward = GetNormalizedVectorProjectedOnFloor(direction);

        m_stateMachine.RB.AddForce((vectorProjectedOnFloorForward * isVectorReversed) * accelerationValue, ForceMode.Acceleration);

        if (m_stateMachine.RB.velocity.magnitude > maxVelocity)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= maxVelocity;
        }
    }

    private void CharacterControllerRelativeToCameraDiagonals(float accelerationValue, float maxVelocity, int isVectorReversed)
    {
        Vector3 vectorProjectedOnFloorForward = GetNormalizedVectorProjectedOnFloor(Vector3.forward);
        m_stateMachine.RB.AddForce(vectorProjectedOnFloorForward * accelerationValue, ForceMode.Acceleration);

        Vector3 vectorProjectedOnFloorRight = GetNormalizedVectorProjectedOnFloor(Vector3.right);
        m_stateMachine.RB.AddForce((vectorProjectedOnFloorRight * isVectorReversed) * accelerationValue, ForceMode.Acceleration);

        if (m_stateMachine.RB.velocity.magnitude > maxVelocity)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= maxVelocity;
        }
    }










    // CHARACTER CONTROLLER RELATIVE TO CHARACTER METHODS



















































































































ARCHIVE


            //m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized * Mathf.Min(m_stateMachine.RB.velocity.magnitude, m_stateMachine.MaxForwardVelocity);


//movementVector += -Vector3.Cross(Vector3.up, projectedVector);
//float sideComponent = Vector3.Dot(movementVector, Vector3.Cross(Vector3.up, projectedVectorForward));


        //float ratio = movementVector.x / movementVector.z;
        //
        //if (movementVector.y != 0)
        //{
        //    float velocityLimit = Mathf.Lerp(m_stateMachine.MaxStrafeVelocity, m_stateMachine.MaxForwardVelocity, Mathf.Abs(ratio));
        //    m_stateMachine.MaxForwardDiagonalsVelocity = velocityLimit;            
        //}
        //Debug.Log("Velocity max diagonal" + m_stateMachine.MaxForwardDiagonalsVelocity);

        //float value = Vector3.Dot(movementVector, projectedVector);
        //
        //if (value == 1) 
        //{
        //    m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
        //    m_stateMachine.RB.velocity *= m_stateMachine.MaxForwardVelocity;
        //    return;
        //}
        //else if (value == -1) 
        //{
        //    m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
        //    m_stateMachine.RB.velocity *= m_stateMachine.MaxForwardVelocity;
        //    return;
        //}


        //if (movementVector == projectedVector) 
        //{
        //    m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
        //    m_stateMachine.RB.velocity *= m_stateMachine.MaxForwardVelocity;
        //    return;
        //}



//if (movementVector.z < 0)
        //{
        //    if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxBackwardVelocity)
        //    {
        //        m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
        //        m_stateMachine.RB.velocity *= m_stateMachine.MaxBackwardVelocity;
        //    }
        //}   


//Vector3 projectedVector = Vector3.zero;
        //projectedVector = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);

        //if (Input.GetKey(KeyCode.W))
        //{
        //    movementVector += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    movementVector += Vector3.ProjectOnPlane(-(m_stateMachine.Camera.transform.forward), Vector3.up);
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    movementVector += Vector3.ProjectOnPlane(-(m_stateMachine.Camera.transform.right), Vector3.up);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    movementVector += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up);
        //}


//var vectorOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
            //vectorOnFloor.Normalize();
            //
            //Vector3 movementVector = Vector3.zero;
            //
            //if (Input.GetKey(KeyCode.W))
            //{
            //    movementVector = new Vector3(0, 0, 1) ;
            //    movementVector += vectorOnFloor;
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    movementVector = new Vector3(0, 0, -1);
            //    movementVector += vectorOnFloor;
            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //    movementVector = new Vector3(-1, 0, 0);
            //    movementVector += vectorOnFloor;
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    movementVector = new Vector3(1, 0, 0);
            //    movementVector += vectorOnFloor;
            //}        
            //
            //
            //
            //m_stateMachine.RB.AddForce(movementVector * m_stateMachine.ForwardAccelerationValue, ForceMode.Acceleration);
            //
            //if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxForwardVelocity)
            //{
            //    m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            //    m_stateMachine.RB.velocity *= m_stateMachine.MaxForwardVelocity;
            //}

            //Debug.Log("Input W: " + Input.GetKey(KeyCode.W));
            //Debug.Log("Movement Vector: " + movementVector);
            //Debug.Log("RB Velocity: " + m_stateMachine.RB.velocity);

            //float forwardComponent = Vector3.Dot(m_stateMachine.RB.velocity, vectorOnFloor);



            //Vector3 inputVector = Vector3.zero; // Vector to store input directions
            //
            //// Check input for movement
            //if (Input.GetKey(KeyCode.W))
            //{
            //    inputVector += Vector3.forward;
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            //    inputVector += Vector3.back;
            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //    inputVector += Vector3.left;
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //    inputVector += Vector3.right;
            //}
            //
            //// Normalize the input vector if it's not zero
            //if (inputVector != Vector3.zero)
            //{
            //    inputVector.Normalize();
            //}
            //
            //// Take camera direction into account
            //Vector3 directionVectorParallelToFloorFromCameraDirection = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
            //directionVectorParallelToFloorFromCameraDirection.Normalize();
            //
            //// Apply force based on the input vector and camera direction
            //Vector3 forceVector = new Vector3(
            //directionVectorParallelToFloorFromCameraDirection.x * inputVector.x,
            //directionVectorParallelToFloorFromCameraDirection.y * inputVector.y,
            //directionVectorParallelToFloorFromCameraDirection.z * inputVector.z
            //);
            //
            //m_stateMachine.RB.AddForce(forceVector * m_stateMachine.ForwardAccelerationValue, ForceMode.Acceleration);











            //Vector3 movementVector = Vector3.zero;
            //Vector3 directionVectorParallelToFloorFromCameraDirection = Vector3.zero;
            //
            //directionVectorParallelToFloorFromCameraDirection = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
            //directionVectorParallelToFloorFromCameraDirection.Normalize(); // On s'assure ici que le vecteur est égal à 1, faire attention avec .normalized qui est différent et ne rend pas égal à 1 mais renvois un vecteur normalisé
            //
            //
            //
            //if (Input.GetKey(KeyCode.W))
            //{
            //    movementVector += new Vector3(0, 1, 0);
            //    //m_stateMachine.RB.AddForce(vectorOnFloor * m_stateMachine.AccelerationValue, ForceMode.Acceleration);
            //}
            //if (Input.GetKey(KeyCode.S)) 
            //{
            //    
            //}
            //if (Input.GetKey(KeyCode.A))
            //{
            //
            //}
            //if (Input.GetKey(KeyCode.D))
            //{
            //
            //}
            //
            //m_stateMachine.RB.AddForce(directionVectorParallelToFloorFromCameraDirection * movementVector * m_stateMachine.ForwardAccelerationValue, ForceMode.Acceleration);











            //projectedVector = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
            //projectedVector.Normalize();

            //Vector3 vectorProjectedOnFloorForward = GetNormalizedVectorProjectedOnFloor(direction);
            //
            //m_stateMachine.RB.AddForce((vectorProjectedOnFloorForward * isVectorReversed) * accelerationValue, ForceMode.Acceleration);
            //
            //if (m_stateMachine.RB.velocity.magnitude > maxVelocity)
            //{
            //    m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            //    m_stateMachine.RB.velocity *= maxVelocity;
            //}


            //float forwardComponent = Vector3.Dot(m_stateMachine.RB.velocity, vectorOnFloor);
            //m_stateMachine.UpdateAnimatorValues(new Vector2(0, forwardComponent));












 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 */