using UnityEngine;

public class AutoDestroyVisualFXAfterParticleSystemsDone : MonoBehaviour
{
    private ParticleSystem[] m_particleSystems;

    void Start()
    {
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        foreach (ParticleSystem particleSystem in m_particleSystems)
        {
            if (particleSystem.IsAlive())
            {
                return;
            }
        }

        Destroy(gameObject);
    }
}
