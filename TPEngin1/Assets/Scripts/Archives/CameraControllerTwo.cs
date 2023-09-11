using UnityEngine;

public class CameraControllerTwo : MonoBehaviour
{
    [SerializeField]
    private Transform m_cameraTarget;

    [SerializeField]
    private float m_mouseSensitivity = 10;
    [SerializeField]
    private float m_distanceFromTarget = 2;
    [SerializeField]
    private Vector2 m_verticalAngleMinMax = Vector2.one;

    [SerializeField]
    private float m_rotationSmoothTime = 1.2f;
    Vector3 m_rotationSmoothVelocity;
    Vector3 m_currentRotation;

    float m_horizontalRotation;
    float m_verticalRotation;




    void Update()
    {
        m_horizontalRotation += Input.GetAxis("Mouse X") * m_mouseSensitivity;
        m_verticalRotation -= Input.GetAxis("Mouse X") * m_mouseSensitivity;
        m_verticalRotation = Mathf.Clamp(m_verticalRotation, m_verticalAngleMinMax.x, m_verticalAngleMinMax.y);

        m_currentRotation = Vector3.SmoothDamp(m_currentRotation, new Vector3(m_verticalRotation, m_horizontalRotation), ref m_rotationSmoothVelocity, m_rotationSmoothTime);

        Vector3 targetRotation = new Vector3(m_verticalRotation, m_horizontalRotation);
        transform.eulerAngles = targetRotation;

        transform.position = m_cameraTarget.position - transform.forward * m_distanceFromTarget;


    }




}
