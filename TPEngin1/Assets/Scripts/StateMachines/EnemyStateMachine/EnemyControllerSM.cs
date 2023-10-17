using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerSM : BaseStateMachine<EnemyState>
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
        //UnityEngine.Debug.Log("Entrer dans Start EnemyControllerSM");

        //je ne suis pas sûr pour le base start et for each et tout
        base.Start();
        //Debug.Log("Test");
        foreach (EnemyState state in m_possibleStates)
        {
            //UnityEngine.Debug.Log("Entrer dans ForEach du start de EnemyControllerSM");

            //Debug.Log("Test");

            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();
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
