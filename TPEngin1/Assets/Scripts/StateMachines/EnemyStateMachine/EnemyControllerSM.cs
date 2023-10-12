using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerSM : StateMachine
{
    //private CharacterState m_playerCharacterState;   

    private EnemyState m_currentState;
    private List<EnemyState> m_possibleStates;

    [field:SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public bool IsHit { get; set; }

    public override void Awake()
    {
        m_possibleStates = new List<EnemyState>();
        m_possibleStates.Add(new EnemyFreeState());
        m_possibleStates.Add(new EnemyHitState());
    }

    public override void Start()
    {
        foreach (EnemyState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }

    public override void Update()
    {
        m_currentState.OnUpdate();
        TryStateTransition();
    }

    public override void FixedUpdate()
    {
        m_currentState.OnFixedUpdate();
    }

    public override void TryStateTransition()
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

}