using UnityEngine;

public class CinematicState : GameManagerState
{
    public override void OnEnter()
    {
        Debug.Log("GameManager entering state : CinematicState");

        m_stateMachine.DesiredState = null;
        m_stateMachine.MainGameplayCamera.gameObject.SetActive(false);
        m_stateMachine.IntroCinematic.gameObject.SetActive(true);
        m_stateMachine.IsCinematicMode = true;
    }

    public override void OnExit()
    {
        m_stateMachine.OnCinematicEnd();
        m_stateMachine.IsCinematicMode = false;
        m_stateMachine.IntroCinematic.gameObject.SetActive(false);
        Debug.Log("GameManager exiting state : CinematicState");
    }

    public override void OnUpdate()
    {        
    }

    public override void OnFixedUpdate()
    {
    }    

    public override bool CanEnter(IState currentState)
    {
        return Input.GetKeyDown(KeyCode.C);
    }

    public override bool CanExit()
    {
        return m_stateMachine.CanTransitionOutOfCinematic() || Input.GetKeyDown(KeyCode.C);        
    }
}
