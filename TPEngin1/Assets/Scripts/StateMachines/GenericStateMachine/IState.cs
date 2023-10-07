
public interface IState
{
    public void OnEnter();
    public void OnUpdate();
    public void OnFixedUpdate();
    public void OnExit();
    public bool CanEnter(CharacterState currentState, EnemyState currentEnemyState);

    public bool CanExit();
}