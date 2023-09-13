public abstract class CharacterState : IState
{
    protected CharacterController m_stateMachine;

    public void OnStart(CharacterController stateMachine)
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

    public virtual bool CanEnter()
    {
        throw new System.NotImplementedException();
    }

    public virtual bool CanExit()
    {
        throw new System.NotImplementedException();
    }
}