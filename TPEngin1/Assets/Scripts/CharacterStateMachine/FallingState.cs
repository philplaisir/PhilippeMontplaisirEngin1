using UnityEngine;

public class FallingState : CharacterState
{
    

    public override void OnEnter()
    {
        Debug.Log("Enter state: FallingState\n");

    }

    public override void OnExit()
    {
        Debug.Log("Exit state: FallingState\n");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }

    public override bool CanEnter(/*CharacterState currentState*/)
    {
        //Debug.Log("FALLING STATE CANENTER ENTERED " + m_stateMachine.IsInContactWithFloor());

        return !m_stateMachine.IsInContactWithFloor();
    }

    public override bool CanExit()
    {
        return m_stateMachine.IsInContactWithFloor();
        
    }
}
