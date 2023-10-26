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
    // TODO checker pour réduire le scope pour que ce ne soit plus public...
    public UnityEvent WasHit;
    public UnityEvent<Vector3, PMM_HitBox, PMM_HitBox> IsHitting = new UnityEvent<Vector3, PMM_HitBox, PMM_HitBox>();

    protected void OnTriggerEnter(Collider other)
    {
        var otherHitBox = other.GetComponent<PMM_HitBox>();
        if (otherHitBox == null) { return; }

        if (CanHitOther(otherHitBox))
        {
            //TODO peut-être à switch pour que ce soit celui qui reçoit qui garde les prefab d'effet
            Vector3 hitPosition = other.ClosestPoint(transform.position);
            IsHitting?.Invoke(hitPosition, this, otherHitBox);
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
        WasHit?.Invoke();
        Debug.Log(gameObject.name + " got hit by " + otherHitBox);        
    }
}


