using UnityEngine;

public class OnGroundState : CharacterState
{

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {
    }

    public override bool CanEnter(CharacterState currentState)
    {
        //if (currentState is )
        //{
        //    return !m_stateMachine.IsInContactWithFloor();
        //}

        return false;


    }

    public override bool CanExit()
    {
        throw new System.NotImplementedException();
    }

}
