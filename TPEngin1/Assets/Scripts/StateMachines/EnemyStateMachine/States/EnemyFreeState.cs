using UnityEngine;

public class EnemyFreeState : EnemyState
{    
    public override void OnEnter()
    {
        Debug.Log("Enemy entering state : EnemyFreeState");
    }

    public override void OnExit()
    {
        Debug.Log("Enemy exiting state : EnemyFreeState");
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
