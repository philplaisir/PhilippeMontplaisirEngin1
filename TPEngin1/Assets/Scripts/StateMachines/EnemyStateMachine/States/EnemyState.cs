public abstract class EnemyState : IState
{
    protected EnemyControllerSM m_stateMachine;

    public void OnStart()
    {                
    }

    public virtual void OnStart(EnemyControllerSM stateMachine)
    {
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
