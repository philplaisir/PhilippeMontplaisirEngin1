using System.Collections.Generic;
using UnityEngine;

public class CharacterSpecialEffectsManager : MonoBehaviour
{
    public static CharacterSpecialEffectsManager _Instance
    {
        get;
        protected set;
    }

    [SerializeField]
    private List<SpecialEffectsGroup> m_specialFXGroups = new List<SpecialEffectsGroup>();
    [SerializeField]
    private AudioSource m_punchHitAudioSource;
    [SerializeField]
    private GameObject m_legsActionsEffectsAudioSource;

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
                    //Debug.Log("On est entré dans le audio effect du punch");
                    int randomAudioIndex = Random.Range(0, specialFXGroup.audioClips.Count);
                    AudioClip clipToPlay = specialFXGroup.audioClips[randomAudioIndex];
                    AudioSource newAudioSource = Instantiate(m_punchHitAudioSource, position, Quaternion.identity, transform);
                    newAudioSource.PlayOneShot(clipToPlay);
                }
                else
                {
                    Debug.Log("No audio clips found for the special effect");
                }

                if (specialFXGroup.visualEffects.Count > 0)
                {
                    //Debug.Log("On est entré dans le visual effect du punch");
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
                break;
            case ECharacterActionType.RunLeftFootstep:
                break;
            case ECharacterActionType.Jump:
                break;
            case ECharacterActionType.JumpLanding:
                break;
            case ECharacterActionType.Count:
                break;
            default:
                break;
        }







        if (specialFXGroup.audioClips.Count > 0)
        {
            int randomAudioIndex = Random.Range(0, specialFXGroup.audioClips.Count);
            AudioClip clipToPlay = specialFXGroup.audioClips[randomAudioIndex];
            //audioSource.PlayOneShot(clipToPlay);

        }
        else
        {
            Debug.Log("No audio clips found for the special effect");
        }

        if (specialFXGroup.visualEffects.Count > 0)
        {
            // Choose a random visual effect from the list and instantiate it
            //int randomVisualIndex = Random.Range(0, fxGroup.visualEffects.Count);
            //GameObject vfxToPlay = fxGroup.visualEffects[randomVisualIndex];
            //Instantiate(vfxToPlay, transform.position, transform.rotation);
        }
        else
        {
            Debug.Log("No visual effect found for the special effect");
        }















        //SpecialEffectsGroup specialFXGroup = new SpecialEffectsGroup();
        //bool found = false;
        //
        //foreach (SpecialEffectsGroup group in m_specialFXGroups)
        //{
        //    if (group.actionType == characterAction)
        //    {
        //        specialFXGroup = group; // probablement à delete
        //        found = true;
        //        break;
        //    }
        //}
        //
        //if (found)
        //{
        //    switch (characterAction)
        //    {
        //        case ECharacterActionType.PunchRight:
        //            if (specialFXGroup.audioClips.Count > 0)
        //            {
        //                int randomAudioIndex = Random.Range(0, specialFXGroup.audioClips.Count);
        //                AudioClip clipToPlay = specialFXGroup.audioClips[randomAudioIndex];
        //
        //                //audioSource.PlayOneShot(clipToPlay);
        //
        //            }
        //            else
        //            {
        //                Debug.Log("No audio clips found for the special effect");
        //            }
        //
        //            break;
        //        case ECharacterActionType.RunRightFootstep:
        //            break;
        //        case ECharacterActionType.RunLeftFootstep:
        //            break;
        //        case ECharacterActionType.Jump:
        //            break;
        //        case ECharacterActionType.JumpLanding:
        //            break;
        //        case ECharacterActionType.Count:
        //            break;
        //        default:
        //            break;
        //    }
        //
        //
        //
        //
        //
        //
        //
        //    if (specialFXGroup.audioClips.Count > 0)
        //    {
        //        int randomAudioIndex = Random.Range(0, specialFXGroup.audioClips.Count);
        //        AudioClip clipToPlay = specialFXGroup.audioClips[randomAudioIndex];
        //        //audioSource.PlayOneShot(clipToPlay);
        //                        
        //    }
        //    else
        //    {
        //        Debug.Log("No audio clips found for the special effect");
        //    }
        //
        //    if (specialFXGroup.visualEffects.Count > 0)
        //    {
        //        // Choose a random visual effect from the list and instantiate it
        //        //int randomVisualIndex = Random.Range(0, fxGroup.visualEffects.Count);
        //        //GameObject vfxToPlay = fxGroup.visualEffects[randomVisualIndex];
        //        //Instantiate(vfxToPlay, transform.position, transform.rotation);
        //    }
        //    else
        //    {
        //        Debug.Log("No visual effect found for the special effect");
        //    }
        //}
        //else
        //{
        //    Debug.Log("No SpecialEffectsGroup found for actionType: " + characterAction.ToString());
        //}


    }
}

[System.Serializable]
public struct SpecialEffectsGroup
{
    public ECharacterActionType actionType;
    public List<AudioClip> audioClips;
    public List<GameObject> visualEffects;
}
