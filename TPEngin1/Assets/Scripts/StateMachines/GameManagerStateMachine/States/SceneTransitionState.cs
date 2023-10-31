using UnityEngine;

public class SceneTransitionState : GameManagerState
{
    public override void OnEnter()
    {
        Debug.Log("GameManager entering state : SceneTransitionState");
    }

    public override void OnExit()
    {
        Debug.Log("GameManager exiting state : SceneTransitionState");
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
    }    

    public override bool CanEnter(IState currentState)
    {
        throw new System.NotImplementedException();
    }

    public override bool CanExit()
    {
        throw new System.NotImplementedException();
    }
}
