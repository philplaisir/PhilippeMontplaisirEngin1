using Cinemachine;
using UnityEngine;

public class CinematicState : GameManagerState
{
    private float m_cinematicTimerForTest;

    public override void OnEnter()
    {
        Debug.Log("Enter GameManager state : CinematicState");

        //m_stateMachine.CinematicCamera.gameObject.SetActive(true);
        m_stateMachine.MainCamera.gameObject.SetActive(false);
        //m_stateMachine.CinematicCamera = Camera.main;

        //CinemachineBrain cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        //cinemachineBrain.ActiveVirtualCamera.Priority = 20;

        //m_cinematicTimerForTest = 3.0f;
    }

    public override void OnExit()
    {
        Debug.Log("Exit GameManager state : CinematicState");
    }

    public override void OnUpdate()
    {
        //m_cinematicTimerForTest -= Time.deltaTime;
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
