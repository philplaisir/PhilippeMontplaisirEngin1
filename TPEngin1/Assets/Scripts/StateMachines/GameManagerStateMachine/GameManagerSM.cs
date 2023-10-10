using System.Collections.Generic;
using UnityEngine;

public class GameManagerSM : StateMachine
{
    public static GameManagerSM _Instance;
    // Pour le caller d'ailleurs GameManagerSM._Instance...

    private GameManagerState m_currentState;
    private List<GameManagerState> m_possibleStates;

    [field: SerializeField] public Camera MainCamera { get; set; }
    [field: SerializeField] public Camera CinematicCamera { get; set; }

    public bool IsCinematicMode { get; private set; } = false;


    public override void Awake()
    {
        if (_Instance == null)
        { 
            _Instance = this;
        }            

        m_possibleStates = new List<GameManagerState>();
        m_possibleStates.Add(new GameplayState());
        m_possibleStates.Add(new CinematicState());
        //m_possibleStates.Add(new SceneTransitionState());
    }

    public override void Start()
    {
        foreach (GameManagerState state in m_possibleStates)
        {
            state.OnStart(this);
        }

        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

        IsCinematicMode = false;
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
                if (state is CinematicState)
                {
                    IsCinematicMode = true;
                }
                else
                {
                    IsCinematicMode = false;

                }
                m_currentState.OnEnter();
                return;
            }
        }
    }
}
