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
        //Debug.Log("Right arm attack hitbox activated");
        //Peut-être plus encapsuler ça et tout faire dans state machine
        //Faire attention à la situation ou quand ça c'est call la hitbox est déjà dans 
        //l'autre collider donc le on trigger enter/on enter trigger truc ne va pas se déclencher
        m_characterControllerSM.RightArmAttackHitBox.SetActive(true);
        //m_stateMachine.OnEnableAttackHitBox
    }

    public void DeactivateRightArmAttackHitbox()
    {
        //Debug.Log("Right arm attack hitbox deactivated");
        m_characterControllerSM.RightArmAttackHitBox.SetActive(false);
    }

    public void MakeRightFootStepDustParticles()
    {        
        VFXManager._Instance.InstantiateVFX(EVisualFXType.RightfootStepDust, Vector3.zero);
    }

    public void MakeLeftFootStepDustParticles()
    {        
        VFXManager._Instance.InstantiateVFX(EVisualFXType.LeftfootStepDust, Vector3.zero);
    }

}
