using UnityEngine;

public class TestBullet : MonoBehaviour
{
    private Rigidbody m_rb;
    private bool m_hitPlayer = false;

    [SerializeField] 
    private float m_moveSpeed = 5.0f;
    [SerializeField] 
    private float m_lifetime = 5.0f;
    


    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!m_hitPlayer)
        {
            transform.Translate(-Vector3.right * m_moveSpeed * Time.deltaTime, Space.World);
        }
        
        m_lifetime -= Time.deltaTime;
        if (m_lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
        m_hitPlayer = true;
        m_rb.useGravity = true;
        
        m_rb.AddForce(Vector3.down * 5.0f, ForceMode.Impulse);
    }
}

