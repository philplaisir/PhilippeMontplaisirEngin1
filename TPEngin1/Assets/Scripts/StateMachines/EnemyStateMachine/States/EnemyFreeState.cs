using UnityEngine;

public class EnemyFreeState : EnemyState
{    
    public override void OnEnter()
    {
        Debug.Log("Enter enemy state : FreeState");
    }

    public override void OnExit()
    {
        Debug.Log("Exit enemy state : FreeState");
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {        
    }    

    public override bool CanEnter(IState currentState)
    {
        return true;
    }

    public override bool CanExit()
    {
        return true;
    }
}