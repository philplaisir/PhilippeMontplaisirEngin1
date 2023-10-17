public abstract class CharacterState : IState
{
    protected CharacterControllerSM m_stateMachine;

    public void OnStart()
    {     
        //throw new System.NotImplementedException();
    }

    public virtual void OnStart(CharacterControllerSM stateMachine)
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


