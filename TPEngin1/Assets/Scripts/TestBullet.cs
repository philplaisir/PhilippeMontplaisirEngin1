using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 5.0f;
    [SerializeField] private float m_lifetime = 5.0f;

    private Rigidbody rb;

    private bool m_hitPlayer = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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

        rb.useGravity = true;

        // Apply a force to simulate gravity
        rb.AddForce(Vector3.down * 5.0f, ForceMode.Impulse);

        

        //Destroy(gameObject);
    }
}








/*

[SerializeField] private float m_moveSpeed = 100.0f;
    [SerializeField] private float m_lifetime = 10.0f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(-transform.right * m_moveSpeed, ForceMode.VelocityChange);
    }

    private void Update()
    {
        m_lifetime -= Time.deltaTime;
        if (m_lifetime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");

        rb.useGravity = true;
        rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);

        // Destroy(gameObject); // You may choose to destroy the bullet here, or elsewhere depending on your game logic.
    


*/