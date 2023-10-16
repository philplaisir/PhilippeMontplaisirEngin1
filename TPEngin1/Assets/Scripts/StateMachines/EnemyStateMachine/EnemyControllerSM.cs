using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerSM : BaseStateMachine<IState>
{
    //private CharacterState m_playerCharacterState;   

    //private EnemyState m_currentState;
    //private List<EnemyState> m_possibleStates;

    [field:SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public bool IsHit { get; set; }

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<IState>();
        m_possibleStates.Add(new EnemyFreeState());
        m_possibleStates.Add(new EnemyHitState());
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        //je ne suis pas sûr pour le base start et for each et tout
        base.Start();

        foreach (EnemyState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        //m_currentState = m_possibleStates[0];
        //m_currentState.OnEnter();
    }

    protected override void Update()
    {
        base.Update();

        //m_currentState.OnUpdate();
        //TryStateTransition();
    }

    protected override void FixedUpdate()
    {
        base.Update();
        //m_currentState.OnFixedUpdate();
    }

     

}
