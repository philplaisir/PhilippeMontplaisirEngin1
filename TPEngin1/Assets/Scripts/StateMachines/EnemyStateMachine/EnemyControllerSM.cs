using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerSM : BaseStateMachine<EnemyState>
{
    [field:SerializeField]
    public Animator Animator { get; private set; }

    [field: SerializeField] public GameObject RightArmAttackHitBox { get; private set; }

    [SerializeField]
    private List<PMM_HitBox> m_hittingHitBoxes = new List<PMM_HitBox>();
    [SerializeField]
    private List<PMM_HitBox> m_receivingHitBoxes = new List<PMM_HitBox>();
    [SerializeField]
    private EnemySpecialFXManager m_enemySpecialFXManager;
        
    public bool IsHit { get; set; }

    public bool Attacking { get; set; } = false;

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<EnemyState>();
        m_possibleStates.Add(new EnemyFreeState());
        m_possibleStates.Add(new EnemyHitState());
        m_possibleStates.Add(new EnemyAttackingState());

    }

    protected override void Awake()
    {
        base.Awake();

        InitializeHittingHitBoxListeners();
        InitializeReceivingHitBoxListeners();
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

    private void InitializeHittingHitBoxListeners()
    {
        for (int i = 0; i < m_hittingHitBoxes.Count; i++)
        {
            if (m_hittingHitBoxes[i] != null)
            {
                m_hittingHitBoxes[i].IsHitting.AddListener(IsHitting);
            }
        }
    }

    private void IsHitting(Vector3 position, PMM_HitBox self, PMM_HitBox other)
    {        
        m_enemySpecialFXManager.PlaySpecialEffect(ECharacterActionType.PunchRight, position, 3.0f);
    }

    private void InitializeReceivingHitBoxListeners()
    {
        for (int i = 0; i < m_receivingHitBoxes.Count; i++)
        {
            if (m_receivingHitBoxes[i] != null)
            {
                m_receivingHitBoxes[i].WasHit.AddListener(WasHit);
            }
        }
    }

    private void WasHit()
    {
        IsHit = true;
    }
}
