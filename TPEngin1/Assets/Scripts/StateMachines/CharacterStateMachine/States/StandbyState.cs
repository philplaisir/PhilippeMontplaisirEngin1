using UnityEngine;

public class StandbyState : CharacterState
{
    public override void OnEnter()
    {
        Debug.Log("Character entering state: StandbyState\n");        
    }

    public override void OnExit()
    {
        Debug.Log("Character exiting state: StandbyState\n");        
    }

    public override void OnUpdate()
    {        
    }

    public override void OnFixedUpdate()
    {        
    }

    public override bool CanEnter(IState currentState)
    {        
        return GameManagerSM._Instance.IsCinematicMode;
    }

    public override bool CanExit()
    {
        return !GameManagerSM._Instance.IsCinematicMode;
    }
}
