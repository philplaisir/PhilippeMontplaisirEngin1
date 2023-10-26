using UnityEngine;

public class VFXManager : MonoBehaviour
{
    //VFXMANAGER PLUS EN UTILISATION

    // Implémentation d'un singleton de style get set
    // Comme un singleton habituel on peut l'appeler à travers: VFXManager._Instance

    //TODO après refactorisation avec le CharacterSpecialEffectsManager cette classe sera peut-être retirée.
        
    //public static VFXManager _Instance
    //{
    //     get;
    //     protected set;
    //}
    //
    //[SerializeField] 
    //private GameObject m_explosionParticleSystem;
    //[SerializeField]
    //private GameObject m_explosionEmitterPos;    
    //
    //private void Awake()
    //{
    //    if (_Instance == null) 
    //    {
    //        _Instance = this;
    //    }
    //    else if (_Instance != this)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    //
    //public void InstantiateVFX(EVisualFXType vfxType, Vector3 position)
    //{
    //    switch (vfxType) 
    //    {            
    //        case EVisualFXType.Explosion:
    //            Instantiate(m_explosionParticleSystem, m_explosionEmitterPos.transform.position, Quaternion.identity, transform);
    //            break;
    //        default:
    //            break;        
    //    }
    //}
}
