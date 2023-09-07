using UnityEngine;

public class FreeState : CharacterState
{
    public override void OnEnter()
    {
    }

    public override void OnUpdate()
    {

    }

    public override void OnFixedUpdate()
    {
        var vectorOnFloor = Vector3.ProjectOnPlane(m_stateMachine.Camera.transform.forward, Vector3.up);
        vectorOnFloor.Normalize();

        if (Input.GetKey(KeyCode.W))
        {
            m_stateMachine.RB.AddForce(vectorOnFloor * m_stateMachine.AccelerationValue, ForceMode.Acceleration);
        }
        if (m_stateMachine.RB.velocity.magnitude > m_stateMachine.MaxVelocity)
        {
            m_stateMachine.RB.velocity = m_stateMachine.RB.velocity.normalized;
            m_stateMachine.RB.velocity *= m_stateMachine.MaxVelocity;
        }

        //TODO 31 AO�T:
        //Appliquer les d�placements relatifs � la cam�ra dans les 3 autres directions
        //Avoir des vitesses de d�placements maximales diff�rentes vers les c�t�s et vers l'arri�re
        //Lorsqu'aucun input est mis, d�c�l�rer le personnage rapidement

        Debug.Log(m_stateMachine.RB.velocity.magnitude);
    }

    public override void OnExit()
    {
    }
}
