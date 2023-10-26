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
        m_enemyControllerSM.RightArmAttackHitBox.SetActive(true);        
    }

    public void DeactivateRightArmAttackHitbox()
    {
        m_enemyControllerSM.RightArmAttackHitBox.SetActive(false);        
    }
}
