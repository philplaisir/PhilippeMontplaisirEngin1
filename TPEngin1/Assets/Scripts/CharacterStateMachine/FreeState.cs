using UnityEngine;

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
        Vector3 movementVector = Vector3.zero;
        //Vector3 projectedVector = Vector3.zero;
        //projectedVector = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);

        if (Input.GetKey(KeyCode.W))
        {
            movementVector += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementVector += Vector3.ProjectOnPlane(-(m_stateMachine.Camera.transform.forward), Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementVector += Vector3.ProjectOnPlane(-(m_stateMachine.Camera.transform.right), Vector3.up);
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementVector += Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.right, Vector3.up);
        }

        if (Input.anyKey)
        {
            m_stateMachine.GameObject.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(m_stateMachine.Transform.transform.eulerAngles.y, m_stateMachine.Camera.transform.eulerAngles.y, ref m_turnSmoothVelocity, m_stateMachine.TurnSmoothTime);
        }
        //if (!Input.anyKey)
        //{
        //    Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
        //    m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);
        //    Debug.Log("Velocity after max" + m_stateMachine.RB.velocity.magnitude);
        //    return;
        //}

        movementVector.Normalize();

        m_stateMachine.RB.AddForce(movementVector * m_stateMachine.ForwardAccelerationValue, ForceMode.Acceleration);

        //Debug.Log("Velocity before max" + m_stateMachine.RB.velocity.magnitude);

        //if (movementVector.z < 0)
        //{
        //    if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxBackwardVelocity)
        //    {
        //        m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
        //        m_stateMachine.RB.velocity *= m_stateMachine.MaxBackwardVelocity;
        //    }
        //}
        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxForwardVelocity)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxForwardVelocity;

            if (m_stateMachine.RB.velocity.magnitude == m_stateMachine.MaxForwardVelocity)
            {
                m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized * m_stateMachine.MaxForwardVelocity;
            }

            Debug.Log("Velocity in thing" + m_stateMachine.RB.velocity.magnitude);
            return;
            //Debug.Log("Velocity in thing" + m_stateMachine.RB.velocity.magnitude);
        }

        //if (m_stateMachine.RB.velocity.magnitude > 0.1f)
        //{
        //    Vector3 vector3 = m_stateMachine.RB.velocity.normalized;
        //    m_stateMachine.RB.AddForce(-vector3 * m_stateMachine.DecelerationValue, ForceMode.Acceleration);
        //}

        //Par exemple, si vous allez à un angle nord-nord-ouest (3/4 du déplacement 	vers l'avant, 1/4 vers la gauche),
        //et que votre vitesse maximale de 	déplacement avant est 20 et vers les côtés 5, votre vitesse maximale calculée 	
        //à ce moment devrait être de ((3/4) * 20 + (1/4) * 5) == 15 + 1.25 == 16.25



        // TODO 230831
        // Apliquer les déplacements relatifs à la caméra dans les 3 autres directions
        // Avoir des vitesse de déplacement différentes maximales vers les côtés et vers l'arrière
        // Lorsqu'aucun input est détecté décélérer le personnage rapidement

        // TODO 230831
        // Essayer d'implémenter d'autres types de déplacements (relatif au personnag, tank control)
        // Essayer d'ajouter contrôle avec manette

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

        /*
         Application de la vélocité sur 
        float forwardComponent  = Vector3.Dot(m_stateMachine.RB.velocity, vectorOnFloor(forward truc truc))

         m_stateMachine.UpdateAnimatorValues(new Vector2(0, forwardComponent));

         */



        // À CONSERVER UTILE POUR CONNAÎTRE LA VELOCITÉ
        //Debug.Log("Velocity after max" + m_stateMachine.RB.velocity.magnitude);
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










    


}

















/*
 


// TEST



















 
 
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