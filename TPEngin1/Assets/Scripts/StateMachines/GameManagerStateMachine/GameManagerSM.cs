using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSM : BaseStateMachine<GameManagerState>
{
    public static GameManagerSM _Instance;    

    public IState DesiredState { get; private set; } = null;

    [field: SerializeField] public CinemachineVirtualCamera MainGameplayCamera { get; private set; }

    [field: Header("CINEMATIC")]
    [field: SerializeField] public GameObject IntroCinematic { get; private set; }
    [SerializeField] private GameObject m_explosionParticleSystem;
    [SerializeField] private AudioSource m_explosionAudioSource;
    [SerializeField] private List<GameObject> m_explosionEmitters = new List<GameObject>();
    
    public bool IsCinematicMode { get; set; } = false;

    protected override void CreatePossibleStates()
    {
        m_possibleStates = new List<GameManagerState>();
        //m_possibleStates.Add(new CinematicState());
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
    
    public void OnCinematicEnd()
    {
        Debug.Log("Entered OnCinematicEnd");

        DesiredState = m_possibleStates[1];
    }

    public bool CanTransitionOutOfCinematic()
    {
        return DesiredState != null;
    }

    public void InstantiateExplosion()
    {
        for (int i = 0; i < m_explosionEmitters.Count; i++) 
        {
            AudioSource newAudioSource = Instantiate(m_explosionAudioSource, m_explosionEmitters[i].transform.position, Quaternion.identity, transform);
            AudioClip clipToPlay = newAudioSource.clip;
            newAudioSource.PlayOneShot(clipToPlay);
            Instantiate(m_explosionParticleSystem, m_explosionEmitters[i].transform.position, Quaternion.identity, transform);
        }
    }
}
