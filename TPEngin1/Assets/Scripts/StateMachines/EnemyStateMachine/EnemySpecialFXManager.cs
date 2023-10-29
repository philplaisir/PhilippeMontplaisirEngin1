using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialFXManager : MonoBehaviour
{
    public static EnemySpecialFXManager _Instance
    {
        get;
        protected set;
    }

    [SerializeField]
    private AudioSource m_newAudioSource;    

    [SerializeField]
    private List<SpecialEffectsGroup> m_specialFXGroups = new List<SpecialEffectsGroup>();
    // Struct dans le CharcterControllerSM

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
                break;            
            case ECharacterActionType.Count:
                break;
            default:
                break;
        }
    }
}