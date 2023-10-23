using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyControllerSM : BaseStateMachine<EnemyState>
{
    [field:SerializeField]
    public Animator Animator { get; private set; }

    [SerializeField]
    private List<PMM_HitBox> m_hitBoxes = new List<PMM_HitBox>();

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

        // Bien checker si c'est une bonne pratique utiliser le invoke et UnityEvent
        InitializeHitBoxListeners();
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

    private void InitializeHitBoxListeners()
    {
        for (int i = 0; i < m_hitBoxes.Count; i++)
        {
            if (m_hitBoxes[i] != null)
            {
                m_hitBoxes[i].OnHit.AddListener(OnHit);
            }
        }
    }

    private void OnHit()
    {
        IsHit = true;
    }
}
