using UnityEngine;

public class FreeState : CharacterState
{
    public float TurnSmoothVelocity;




    public override void OnEnter()
    {
        Debug.Log("Enter state: FreeState\n");
    }

    public override void OnUpdate()
    {
        if (Input.anyKey)
        {
            if (IsTwoOrMoreReverseInputsInputedSimultaneouslyOneRelativeToCamera())
            {
                CharacterControllerDeceleration();
                return;
            }
            CharacterControllerRelativeToCameraFUpdate();
        }
        else
        {
            CharacterControllerDeceleration();
        }
    }

    public override void OnFixedUpdate()
    {
        // CHARACTER MOVEMENT RELATIVE TO CAMERA
        

        // TODO 230831
        // Apliquer les déplacements relatifs à la caméra dans les 3 autres directions
        // Avoir des vitesse de déplacement différentes maximales vers les côtés et vers l'arrière
        // Lorsqu'aucun input est détecté décélérer le personnage rapidement

        // TODO 230831
        // Essayer d'implémenter d'autres types de déplacements (relatif au personnag, tank control)
        // Essayer d'ajouter contrôle avec manette



        // À CONSERVER UTILE POUR CONNAÎTRE LA VELOCITÉ
        //Debug.Log(m_stateMachine.RB.velocity.magnitude);
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
        m_stateMachine.Transform.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(m_stateMachine.Transform.transform.eulerAngles.y, m_stateMachine.Camera.transform.eulerAngles.y, ref TurnSmoothVelocity, m_stateMachine.TurnSmoothTime);
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





}