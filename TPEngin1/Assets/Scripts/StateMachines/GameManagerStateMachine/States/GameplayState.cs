using UnityEngine;

public class GameplayState : GameManagerState
{
    public override void OnEnter()
    {
        Debug.Log("Enter GameManager state : GameplayState");

        m_stateMachine.CinematicCamera.gameObject.SetActive(false);
        m_stateMachine.MainCamera.gameObject.SetActive(true);
        //m_stateMachine.MainCamera = Camera.main;
    }

    public override void OnExit()
    {
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
        return Input.GetKeyDown(KeyCode.C);
    }

    public override bool CanExit()
    {
        return true;
    }
}
