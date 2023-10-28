using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSM : BaseStateMachine<GameManagerState>
{
    public static GameManagerSM _Instance;
    // Pour le caller d'ailleurs GameManagerSM._Instance...

    [field: SerializeField] public CinemachineVirtualCamera MainCamera { get; set; }
    [field: SerializeField] public Camera CinematicCamera { get; set; }

    public bool IsCinematicMode { get; private set; } = false;

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<GameManagerState>();
        m_possibleStates.Add(new CinematicState());
        m_possibleStates.Add(new GameplayState());
        //m_possibleStates.Add(new CinematicState());
        //m_possibleStates.Add(new SceneTransitionState());
    }

    protected override void Awake()
    {        
        base.Awake();

        if (_Instance == null)
        { 
            _Instance = this;
        }          
    }

    protected override void Start()
    {
        foreach (GameManagerState state in m_possibleStates)
        {           
            state.OnStart(this);
        }
        m_currentState = m_possibleStates[0];
        m_currentState.OnEnter();

        IsCinematicMode = false;
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
