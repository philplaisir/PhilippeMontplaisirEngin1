using UnityEngine;

public class GameplayState : GameManagerState
{
    public override void OnEnter()
    {
        Debug.Log("Enter GameManager state : GameplayState");

        //En ce moment ici m_stateMachine est à null ce qui cause prpblème, pas de ref
        //m_stateMachine.CinematicCamera.gameObject.SetActive(false);
        //m_stateMachine.MainCamera.gameObject.SetActive(true);
        //m_stateMachine.MainCamera = Camera.main;

        GameManagerSM._Instance.MainCamera.gameObject.SetActive(true);
        GameManagerSM._Instance.CinematicCamera.gameObject.SetActive(false);

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
        return true;
    }

    public override bool CanExit()
    {
        return true;
    }
}
