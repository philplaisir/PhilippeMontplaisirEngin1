public abstract class GameManagerState : IState
{
    protected GameManagerSM m_stateMachine;

    public void OnStart()
    {
        UnityEngine.Debug.Log("Entrer dans OnStart GameManagerState");

        //throw new System.NotImplementedException();
    }

    public virtual void OnStart(GameManagerSM stateMachine)
    {
        UnityEngine.Debug.Log("Entrer dans OnStart GameManagerState avec paramètre(GameManagerSM stateMachine, c'est ici que m_stateMachine s'initialise)");

        //Ce on start ne se fait pas appeller
        m_stateMachine = stateMachine;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }
   
    public virtual bool CanEnter(IState currentState)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CanExit()
    {
        throw new System.NotImplementedException();
    }

}
