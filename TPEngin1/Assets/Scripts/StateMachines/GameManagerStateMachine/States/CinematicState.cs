using Cinemachine;
using UnityEngine;

public class CinematicState : GameManagerState
{
    private float m_cinematicTimerForTest;

    public override void OnEnter()
    {
        Debug.Log("Enter GameManager state : CinematicState");

        m_stateMachine.MainGameplayCamera.gameObject.SetActive(false);
        m_stateMachine.IntroCinematic.gameObject.SetActive(true);
        m_stateMachine.IsCinematicMode = true;
    }

    public override void OnExit()
    {
        m_stateMachine.IsCinematicMode = false;
        m_stateMachine.IntroCinematic.gameObject.SetActive(false);
        Debug.Log("Exit GameManager state : CinematicState");
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
        return m_stateMachine.CanTransitionOutOfCinematic();
        //return m_cinematicTimerForTest <= 0;
    }
}
