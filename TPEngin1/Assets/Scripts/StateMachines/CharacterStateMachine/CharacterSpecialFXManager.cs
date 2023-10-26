using System.Collections.Generic;
using UnityEngine;

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

    public void PlaySpecialEffect(ECharacterActionType characterAction, Vector3 position)
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
                break;
            case ECharacterActionType.RunRightFootstep:
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
                break;
            case ECharacterActionType.RunLeftFootstep:
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
                break;
            case ECharacterActionType.Jump:
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
                break;
            case ECharacterActionType.JumpLanding:
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
                break;
            case ECharacterActionType.Count:
                break;
            default:
                break;
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
