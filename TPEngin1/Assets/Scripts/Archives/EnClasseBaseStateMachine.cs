using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnClasseBaseStateMachine : MonoBehaviour
{
    //Une fois pr�sent ici on peut le retirer dans les state machines enfant

    protected CharacterState m_currentState;
    protected List<CharacterState> m_possibleStates;

    private void Awake()
    {
        CreatePossibleStates();
    }

    protected virtual void CreatePossibleStates()
    {
        //Mettre le code du pour cr�er possible states
    }

    protected void Start()
    {
        foreach (CharacterState state in m_possibleStates)
        {
            //state.OnStart(this); Ici on peut le changer pour BaseStateMachine
            //On va pouvoir le changer enlever le this lorsque dans la classe de base on change l'�tat from CharacterState to IState
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

        //Classe h�rit� mettre override
        //base.Start(); pour l'appel dans la classe h�rit�
    }

    protected virtual void Update()
    {
        m_currentState.OnUpdate();
        TryStateTransition();
    }

    public virtual void TryStateTransition()
    {
        if (!m_currentState.CanExit())
        {
            return;
        }

        //Je PEUX quitter le state actuel
        foreach (var state in m_possibleStates)
        {
            if (m_currentState == state)
            {
                continue;
            }
            if (state.CanEnter(m_currentState))
            {
                //Quitter le state actuel
                m_currentState.OnExit();
                m_currentState = state;
                //Rentrer dans le state state
                m_currentState.OnEnter();
                return;
            }
        }
    }

    //Fixed Update





}
