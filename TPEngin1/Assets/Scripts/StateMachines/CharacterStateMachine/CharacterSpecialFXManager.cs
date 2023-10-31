using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterSpecialFXManager : MonoBehaviour
{
    public static CharacterSpecialFXManager _Instance
    {
        get;
        protected set;
    }

    [SerializeField]
    private AudioSource m_newAudioSource;
    [SerializeField]
    private GameObject m_legsActionsEffectsAudioSource;
    [SerializeField]
    private GameObject m_rightFootStepDustEmitterPos;
    [SerializeField]
    private GameObject m_leftFootStepDustEmitterPos;

    [SerializeField]
    private List<SpecialEffectsGroup> m_specialFXGroups = new List<SpecialEffectsGroup>();
       
    private CinemachineImpulseSource m_impulseSource;

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

    private void Start()
    {        
        m_impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void PlaySpecialEffect(ECharacterActionType characterAction, Vector3 position, float intensity)
    {
        SpecialEffectsGroup specialFXGroup = new SpecialEffectsGroup();
        foreach (SpecialEffectsGroup group in m_specialFXGroups)
        {
            if (group.actionType == characterAction)
            {
                specialFXGroup = group;
                break;
            }
        }

        switch (specialFXGroup.actionType)
        {
            case ECharacterActionType.PunchRight:
                PunchRightHand(specialFXGroup, position, intensity);
                break;
            case ECharacterActionType.RunRightFootstep:
                RunRightFootstep(specialFXGroup);
                break;
            case ECharacterActionType.RunLeftFootstep:
                RunLeftFootstep(specialFXGroup);
                break;
            case ECharacterActionType.Jump:
                Jump(specialFXGroup);
                break;
            case ECharacterActionType.JumpLanding:
                JumpLanding(specialFXGroup);
                break;
            case ECharacterActionType.Count:
                break;
            default:
                break;
        }
    }

    private void PunchRightHand(SpecialEffectsGroup specialFXGroup, Vector3 position, float intensity)
    {
        if (specialFXGroup.audioClips.Count > 0)
        {
            int randomAudioIndex = Random.Range(0, specialFXGroup.audioClips.Count);
            AudioClip clipToPlay = specialFXGroup.audioClips[randomAudioIndex];
            AudioSource newAudioSource = Instantiate(m_newAudioSource, position, Quaternion.identity, transform);
            newAudioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.Log("No audio clips found for the special effect");
        }

        if (specialFXGroup.visualEffects.Count > 0)
        {
            int randomVisualIndex = Random.Range(0, specialFXGroup.visualEffects.Count);
            GameObject vfxToPlay = specialFXGroup.visualEffects[randomVisualIndex];
            Instantiate(vfxToPlay, position, Quaternion.identity, transform);
        }
        else
        {
            Debug.Log("No visual effect found for the special effect");
        }

        intensity = Random.Range(intensity - 0.5f, intensity + 0.5f);
        m_impulseSource.GenerateImpulse(intensity);

        GameManagerSM._Instance.IsSlowMoed = true;
    }

    private void RunRightFootstep(SpecialEffectsGroup specialFXGroup)
    {
        if (specialFXGroup.audioClips.Count > 0)
        {
            int randomAudioIndex = Random.Range(0, specialFXGroup.audioClips.Count);
            AudioClip clipToPlay = specialFXGroup.audioClips[randomAudioIndex];
            AudioSource newAudioSource = Instantiate(m_newAudioSource, m_legsActionsEffectsAudioSource.transform.position, Quaternion.identity, transform);
            newAudioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.Log("No audio clips found for the special effect");
        }

        if (specialFXGroup.visualEffects.Count > 0)
        {
            int randomVisualIndex = Random.Range(0, specialFXGroup.visualEffects.Count);
            GameObject vfxToPlay = specialFXGroup.visualEffects[randomVisualIndex];
            Instantiate(vfxToPlay, m_rightFootStepDustEmitterPos.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No visual effect found for the special effect");
        }
    }

    private void RunLeftFootstep(SpecialEffectsGroup specialFXGroup)
    {
        if (specialFXGroup.audioClips.Count > 0)
        {
            int randomAudioIndex = Random.Range(0, specialFXGroup.audioClips.Count);
            AudioClip clipToPlay = specialFXGroup.audioClips[randomAudioIndex];
            AudioSource newAudioSource = Instantiate(m_newAudioSource, m_legsActionsEffectsAudioSource.transform.position, Quaternion.identity, transform);
            newAudioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.Log("No audio clips found for the special effect");
        }

        if (specialFXGroup.visualEffects.Count > 0)
        {
            int randomVisualIndex = Random.Range(0, specialFXGroup.visualEffects.Count);
            GameObject vfxToPlay = specialFXGroup.visualEffects[randomVisualIndex];
            Instantiate(vfxToPlay, m_leftFootStepDustEmitterPos.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No visual effect found for the special effect");
        }
    }

    private void Jump(SpecialEffectsGroup specialFXGroup)
    {
        if (specialFXGroup.audioClips.Count > 0)
        {
            int randomAudioIndex = Random.Range(0, specialFXGroup.audioClips.Count);
            AudioClip clipToPlay = specialFXGroup.audioClips[randomAudioIndex];
            AudioSource newAudioSource = Instantiate(m_newAudioSource, m_legsActionsEffectsAudioSource.transform.position, Quaternion.identity, transform);
            newAudioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.Log("No audio clips found for the special effect");
        }

        if (specialFXGroup.visualEffects.Count > 0)
        {
            int randomVisualIndex = Random.Range(0, specialFXGroup.visualEffects.Count);
            GameObject vfxToPlay = specialFXGroup.visualEffects[randomVisualIndex];
            Instantiate(vfxToPlay, m_leftFootStepDustEmitterPos.transform.position, Quaternion.identity, transform);
        }
        else
        {
            Debug.Log("No visual effect found for the special effect");
        }
    }

    private void JumpLanding(SpecialEffectsGroup specialFXGroup)
    {
        if (specialFXGroup.audioClips.Count > 0)
        {
            int randomAudioIndex = Random.Range(0, specialFXGroup.audioClips.Count);
            AudioClip clipToPlay = specialFXGroup.audioClips[randomAudioIndex];
            AudioSource newAudioSource = Instantiate(m_newAudioSource, m_legsActionsEffectsAudioSource.transform.position, Quaternion.identity, transform);
            newAudioSource.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.Log("No audio clips found for the special effect");
        }

        if (specialFXGroup.visualEffects.Count > 0)
        {
            int randomVisualIndex = Random.Range(0, specialFXGroup.visualEffects.Count);
            GameObject vfxToPlay = specialFXGroup.visualEffects[randomVisualIndex];
            Instantiate(vfxToPlay, m_rightFootStepDustEmitterPos.transform.position, Quaternion.identity, transform);
            Instantiate(vfxToPlay, m_leftFootStepDustEmitterPos.transform.position, Quaternion.identity, transform);
        }
        else
        {
            Debug.Log("No visual effect found for the special effect");
        }
    }
}

[System.Serializable]
public struct SpecialEffectsGroup
{
    public ECharacterActionType actionType;
    public List<AudioClip> audioClips;
    public List<GameObject> visualEffects;
}
