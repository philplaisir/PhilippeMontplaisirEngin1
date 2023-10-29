using UnityEngine;

public class GameplayState : GameManagerState
{
    public override void OnEnter()
    {
        Debug.Log("Enter GameManager state : GameplayState");

        m_stateMachine.MainGameplayCamera.gameObject.SetActive(true);
        m_stateMachine.IntroCinematic.gameObject.SetActive(false);
    }

    public override void OnExit()
    {
        m_stateMachine.MainGameplayCamera.gameObject.SetActive(false);
        Debug.Log("Exit GameManager state : GameplayState");
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
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
