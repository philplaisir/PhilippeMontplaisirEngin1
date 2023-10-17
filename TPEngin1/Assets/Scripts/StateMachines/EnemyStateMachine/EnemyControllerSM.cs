using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerSM : BaseStateMachine<EnemyState>
{
    [field:SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField]
    public bool IsHit { get; set; }

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<EnemyState>();
        m_possibleStates.Add(new EnemyFreeState());
        m_possibleStates.Add(new EnemyHitState());
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {               
        foreach (EnemyState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();        
    }
}
