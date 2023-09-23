using UnityEngine;

public class AttackingState : CharacterState
{
    private Animator m_animator;

    //private bool m_attackAnimationFinished = false;
    //private float m_animationTimer = 0.0f;
    

    public override void OnEnter()
    {
        Debug.Log("Enter state: AttackingState");
        m_animator = m_stateMachine.GetComponentInParent<Animator>();
        m_animator.SetTrigger("Attack");
        //m_animator.SetBool("Attacking", true);

        m_stateMachine.Attacking = true;

        //
        //m_animator.SetTrigger("Attack");
        //m_animator.SetBool("Attacking", true);
        ////m_attackAnimationFinished = false;
        //m_animationTimer = 0;
        ////m_animator.SetFloat("AttackTimer", 0);

    }

    public override void OnExit()
    {
        
        
        Debug.Log("Exit state: AttackingState");

    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnUpdate()
    {

        if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            m_stateMachine.Attacking = false;

        }





        //m_animationTimer = m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        //
        ////m_animationTimer = m_animator.GetFloat("AttackTimer");
        //
        //if (m_animationTimer >= 1 )
        //{
        //    m_animator.SetBool("Attacking", false);
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


        //return m_animator.GetFloat("AttackTimer") >= 1;

        //return m_animator.GetBool("Attacking");
        //return !m_animator.GetBool("Attacking");
        //return true;
        //throw new System.NotImplementedException();
    }

}
