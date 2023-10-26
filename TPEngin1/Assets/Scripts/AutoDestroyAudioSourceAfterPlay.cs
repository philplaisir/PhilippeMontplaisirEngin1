using UnityEngine;

public class AutoDestroyAudioSourceAfterPlay : MonoBehaviour
{
    private AudioSource m_audioSource;

    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (m_audioSource.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
