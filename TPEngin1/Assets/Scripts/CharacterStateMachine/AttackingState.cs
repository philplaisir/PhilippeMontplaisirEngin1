using UnityEngine;

public class AttackingState : CharacterState
{
    private Animator m_animator;

    

    //private bool m_isAttacking;
    

    private float m_delay;
    private bool m_hasAttackAnimationFinished = false;


    public override void OnEnter()
    {
        Debug.Log("Enter state: AttackingState");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        //m_animator.SetTrigger("Attack");
        m_animator.SetBool("Attacking", true);

        m_delay = 0.3f;

        m_hasAttackAnimationFinished = false;

        m_stateMachine.Attacking = true;
        //m_isAttacking = true;
    }

    public override void OnExit()
    {
        m_animator.SetBool("Attacking", false);


        Debug.Log("Exit state: AttackingState");

    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {

        //if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        //{
        //    m_hasAttackAnimationFinished = true;
        //}
        //
        //// Enter the if statement only when the animation is done
        //if (m_hasAttackAnimationFinished)
        //{
        //    m_stateMachine.Attacking = false;
        //    // Additional code specific to when the animation is done
        //}



        m_delay -= Time.deltaTime;
        
        if (m_delay <= 0)
        {
            m_stateMachine.Attacking = false;
        }

        //if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        //{
        //    m_stateMachine.Attacking = false;
        //
        //}

    }

    public override bool CanEnter(CharacterState currentState)
    {

        if (currentState is FreeState)
        {

            return Input.GetKeyDown(KeyCode.E);
        }
        return false;

    }

    public override bool CanExit()
    {
        if (!m_stateMachine.Attacking)
        {
            return true;
        }
        return false;


        //return !m_stateMachine.Attacking;
        //if (m_isAttacking)
        //{
        //    return true;
        //}
        //return false;

    }

}
