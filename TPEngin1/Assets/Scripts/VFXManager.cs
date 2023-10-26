using UnityEngine;

public class VFXManager : MonoBehaviour
{
    // Implémentation d'un singleton de style get set
    // Comme un singleton habituel on peut l'appeler à travers: VFXManager._Instance
    public static VFXManager _Instance
    {
         get;
         protected set;
    }

    [SerializeField]
    private GameObject m_hitParticleSystem;
    [SerializeField] 
    private GameObject m_explosionParticleSystem;
    [SerializeField]
    private GameObject m_explosionEmitterPos;
    [SerializeField]
    private GameObject m_stepDustParticleSystem;
    [SerializeField]
    private GameObject m_rightFootStepDustEmitterPos;
    [SerializeField]
    private GameObject m_leftFootStepDustEmitterPos;

    private void Awake()
    {
        if (_Instance == null) 
        {
            _Instance = this;
        }
        else if (_Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void InstantiateVFX(EVisualFXType vfxType, Vector3 position)
    {
        switch (vfxType) 
        {
            case EVisualFXType.RightfootStepDust:                
                Instantiate(m_stepDustParticleSystem, m_rightFootStepDustEmitterPos.transform.position, Quaternion.identity, transform);
                break;
            case EVisualFXType.LeftfootStepDust:                
                Instantiate(m_stepDustParticleSystem, m_leftFootStepDustEmitterPos.transform.position, Quaternion.identity, transform);
                break;
            case EVisualFXType.Hit:
                //Instantiate(m_hitParticleSystem, position, Quaternion.identity, transform);
                break;
            case EVisualFXType.Explosion:
                Instantiate(m_explosionParticleSystem, m_explosionEmitterPos.transform.position, Quaternion.identity, transform);
                break;
            default:
                break;        
        }
    }
}
