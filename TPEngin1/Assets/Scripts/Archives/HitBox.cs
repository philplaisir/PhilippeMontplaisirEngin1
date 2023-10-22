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

/*
 * Il mettrait ça dans une librairie
 * PMM_Hitbox si dans une librairie
 Script hitbox du prof

[Serializefield]
protected bool m_canHit;
[Serializefield]
protected bool m_canReceiveHit;
//Si on veut une information seulement quand deux bool sont vrai, pas obligé
protected bool CanGiveAndReceiveHits {get{return m_canHit && m_canReceiveHits;}}
 
 [Serializefield]
protected EAgentType m_agentType = EAgentType.Count;
[Serializefield]
protected list<EAgentType> m_affectedAgentTypes = new List<EAgentType> 
 
 ^rivate void On triggerEnter(Collider other)

var otherHitbox = other.GetComponent<MFHitbox>();
if(otherhitbox = null) {return;}

//Other collider also is MFHItbox
// Le plusn intéressant devrait de mettre la logique d'agression ferait plus de sens
if(m_canHit && otherHitBox. m_canReceiveHits)
{
if (m_afectedAgentsTypes.Contains(otherHitBox.m_agentType))
{
    //effectuer le hit
//Et on peut transféer ca dans une autre méthode avec return true ou false
la méthode peut être CanHitOther
ensuite au retour de la fonction id CanHitOther on retourne true
}
}
 
 
 
 
 
 
 
 */

/*
 
 public enum EAgentType

Ally,
Enemy,
Neutral,
Count
 
 */