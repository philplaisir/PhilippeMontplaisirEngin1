using UnityEngine;

public class GettingUpState : CharacterState
{
    private Animator m_animator;
    private float m_delayTimer;



    public override void OnEnter()
    {
        Debug.Log("Enter state: GettingUpState\n");

        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        m_animator.SetTrigger("BackUp");
        m_delayTimer = 0.3f;
    }

    public override void OnExit()
    {
        Debug.Log("Exit state: GettingUpState\n");
    }

    public override void OnFixedUpdate()
    {
        m_delayTimer -= Time.deltaTime;
    }

    public override void OnUpdate()
    {
    }

    public override bool CanEnter(CharacterState currentState, EnemyState currentEnemyState)
    {
        if (currentState is OnGroundState || currentState is StunInAirState)
        {
            return true;
        }

        return false;
    }

    public override bool CanExit()
    {
        return m_delayTimer <= 0;
    }
}
