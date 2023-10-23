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

    public void InstantiateVFX(EVFXType vfxType)
    {
        switch (vfxType) 
        {
            case EVFXType.RightFootStepDust:
                Debug.Log("rightFootStepDust switch entered");
                Instantiate(m_stepDustParticleSystem, m_rightFootStepDustEmitterPos.transform.position, Quaternion.identity, transform);
                break;
            case EVFXType.LeftFootStepDust:
                Debug.Log("leftFootStepDust switch entered");

                Instantiate(m_stepDustParticleSystem, m_leftFootStepDustEmitterPos.transform.position, Quaternion.identity, transform);
                break;
            default:
                break;
        
        }
    }
}
