using System.Collections.Generic;
using UnityEngine;

public class PMM_HitBox : MonoBehaviour
{
    [SerializeField]
    protected bool m_canHit;
    [SerializeField]
    protected bool m_canGetHit;
    [SerializeField]
    protected EAgentType m_currentHitBoxAgentType;
    [SerializeField]
    protected List<EAgentType> m_agentTypesAffected = new List<EAgentType>();

    protected void OnTriggerEnter(Collider other)
    {
        var otherHitBox = other.GetComponent<PMM_HitBox>();
        if (otherHitBox == null) { return; }

        if (CanHitOther(otherHitBox))
        {
            other.ClosestPoint(transform.position);
            otherHitBox.GetHit(this);
        }
    }

    protected bool CanHitOther(PMM_HitBox other)
    {
        return (m_canHit &&
                other.m_canGetHit &&
                m_agentTypesAffected.Contains(other.m_currentHitBoxAgentType));
    }

    protected void GetHit(PMM_HitBox otherHitBox)
    {
        // Changer le nom de cette méthode
        Debug.Log(gameObject.name + " got hit by " + otherHitBox);
    }



}


