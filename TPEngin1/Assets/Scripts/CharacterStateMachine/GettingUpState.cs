

using UnityEngine;

public class GettingUpState : CharacterState
{
    private Animator m_animator;
    private float m_delayTimer;

    public override void OnEnter()
    {
        m_animator = m_stateMachine.GetComponentInParent<Animator>();

        m_delayTimer = 0.3f;
    }

    public override void OnExit()
    {
    }

    public override void OnFixedUpdate()
    {
        m_delayTimer -= Time.deltaTime;
    }

    public override void OnUpdate()
    {
    }

    public override bool CanEnter(CharacterState currentState)
    {
        if (currentState is OnGroundState)
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
