using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PMM_HitBox : MonoBehaviour
{
    [SerializeField]
    protected bool m_canHit;
    [SerializeField]
    protected bool m_canGetHit;
    [SerializeField]
    protected EAgentType m_currentHitBoxAgentType;
    [SerializeField]
    protected List<EAgentType> m_agentTypesAffectedByThis = new List<EAgentType>();

    // Bien checker si c'est une bonne pratique utiliser le invoke et UnityEvent
    public UnityEvent OnHit;
    
    protected void OnTriggerEnter(Collider other)
    {
        var otherHitBox = other.GetComponent<PMM_HitBox>();
        if (otherHitBox == null) { return; }

        if (CanHitOther(otherHitBox))
        {
            other.ClosestPoint(transform.position);
            otherHitBox.GotHit(this);
        }
    }

    protected bool CanHitOther(PMM_HitBox other)
    {
        return (m_canHit &&
                other.m_canGetHit &&
                m_agentTypesAffectedByThis.Contains(other.m_currentHitBoxAgentType));
    }

    protected void GotHit(PMM_HitBox otherHitBox)
    {
        // Bien checker si c'est une bonne pratique utiliser le invoke et UnityEvent
        OnHit?.Invoke();

        Debug.Log(gameObject.name + " got hit by " + otherHitBox);
        
    }



}


