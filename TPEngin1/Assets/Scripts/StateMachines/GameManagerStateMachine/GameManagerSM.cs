using System.Collections.Generic;
using UnityEngine;

public class GameManagerSM : BaseStateMachine<IState>
{
    public static GameManagerSM _Instance;
    // Pour le caller d'ailleurs GameManagerSM._Instance...

    //private GameManagerState m_currentState;
    //private List<GameManagerState> m_possibleStates;

    [field: SerializeField] public Camera MainCamera { get; set; }
    [field: SerializeField] public Camera CinematicCamera { get; set; }

    public bool IsCinematicMode { get; private set; } = false;

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<IState>();
        m_possibleStates.Add(new GameplayState());
        m_possibleStates.Add(new CinematicState());
    }

    protected override void Awake()
    {
        //Pas sûr pour le base.awake ici
        base.Awake();

        if (_Instance == null)
        { 
            _Instance = this;
        }            

        
        //m_possibleStates.Add(new SceneTransitionState());
    }

    protected override void Start()
    {
        //je ne suis pas sûr pour le base start et for each et tout
        base.Start();
        foreach (GameManagerState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        //m_currentState = m_possibleStates[0];
        //m_currentState.OnEnter();

        IsCinematicMode = false;
    }

    protected override void Update()
    {
        base.Update();
        //m_currentState.OnUpdate();
        //TryStateTransition();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //m_currentState.OnFixedUpdate();
    }

    
}
