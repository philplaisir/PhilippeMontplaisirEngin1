using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed = 5.0f;
    [SerializeField]
    private float m_lifetime = 5.0f;


    private void Awake()
    {
        
    }

    private void Update()
    {        
        transform.Translate(-Vector3.right * m_moveSpeed * Time.deltaTime, Space.World);
        m_lifetime -= Time.deltaTime;
        if (m_lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
    }
}
