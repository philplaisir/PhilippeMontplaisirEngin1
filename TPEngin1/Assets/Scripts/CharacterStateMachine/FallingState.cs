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

    public override bool CanEnter()
    {
        throw new System.NotImplementedException();
    }

    public override bool CanExit()
    {
        throw new System.NotImplementedException();
    }
}
