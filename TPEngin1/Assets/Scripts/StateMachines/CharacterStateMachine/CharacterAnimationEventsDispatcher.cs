using UnityEngine;

public class CharacterAnimationEventsDispatcher : MonoBehaviour
{   
    private CharacterControllerSM m_characterControllerSM;

    private void Awake()
    {
        m_characterControllerSM = GetComponentInChildren<CharacterControllerSM>();
    }

    public void ActivateRightArmAttackHitbox()
    {        
        m_characterControllerSM.RightArmAttackHitBox.SetActive(true);        
    }

    public void DeactivateRightArmAttackHitbox()
    {        
        m_characterControllerSM.RightArmAttackHitBox.SetActive(false);
    }

    public void MakeRightFootStepFX()
    {
        CharacterSpecialFXManager._Instance.PlaySpecialEffect(ECharacterActionType.RunRightFootstep, Vector3.zero, 0.0f);        
    }

    public void MakeLeftFootStepFX()
    {
        CharacterSpecialFXManager._Instance.PlaySpecialEffect(ECharacterActionType.RunLeftFootstep, Vector3.zero, 0.0f);        
    }

    public void MakeJumpFootFX()
    {
        CharacterSpecialFXManager._Instance.PlaySpecialEffect(ECharacterActionType.Jump, Vector3.zero, 0.0f);
    }

    public void MakeJumpLandFootFX() 
    {
        CharacterSpecialFXManager._Instance.PlaySpecialEffect(ECharacterActionType.JumpLanding, Vector3.zero, 0.0f);
    }

}
