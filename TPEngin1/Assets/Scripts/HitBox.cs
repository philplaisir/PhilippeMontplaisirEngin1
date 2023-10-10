using UnityEngine;

public class HitBox : MonoBehaviour
{
    private EnemyControllerSM m_enemyControllerSM;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");

        m_enemyControllerSM = other.GetComponentInChildren<EnemyControllerSM>();

        if (m_enemyControllerSM != null)
        {
            m_enemyControllerSM.IsHit = true;
        }            
    }
}
