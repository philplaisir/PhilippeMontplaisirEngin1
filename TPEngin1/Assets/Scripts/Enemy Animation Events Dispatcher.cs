using UnityEngine;

public class EnemyAnimationEventsDispatcher : MonoBehaviour
{
    private EnemyControllerSM m_enemyControllerSM;

    private void Awake()
    {
        m_enemyControllerSM = GetComponentInChildren<EnemyControllerSM>();
    }

    public void ActivateRightArmAttackHitbox()
    {
        
        //m_characterControllerSM.RightArmAttackHitBox.SetActive(true);
        //m_stateMachine.OnEnableAttackHitBox
    }

    public void DeactivateRightArmAttackHitbox()
    {
        //Debug.Log("Right arm attack hitbox deactivated");
        //m_characterControllerSM.RightArmAttackHitBox.SetActive(false);
    }
}
