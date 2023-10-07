public abstract class EnemyState : IState
{
    protected EnemyControllerSM m_stateMachine;

    public void OnStart(EnemyControllerSM stateMachine)
    {
        m_stateMachine = stateMachine;
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {        
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnUpdate()
    {
    }    

    public virtual bool CanEnter(CharacterState currentState, EnemyState currentEnemyState)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CanExit()
    {
        throw new System.NotImplementedException();
    }
}
