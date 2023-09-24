
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed = 5.0f;
    [SerializeField]
    private float m_maxHeight = 20.0f;
    [SerializeField]
    private float m_minHeight = 0.0f;

    private bool m_movingUp = false;
    private bool m_movingDown = false;

    private void Update()
    {
        if (m_movingUp)
        {
            MoveUp();
        }
        else if (m_movingDown)
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        if (transform.position.y < m_maxHeight)
        {
            float step = m_moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.up * step);
        }
    }

    private void MoveDown()
    {
        if (transform.position.y > m_minHeight)
        {
            float step = m_moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.down * step);
        }
    }

    public void StartMovingUp()
    {
        m_movingUp = true;
        m_movingDown = false;
    }

    public void StartMovingDown()
    {
        m_movingUp = false;
        m_movingDown = true;
    }

    
}
