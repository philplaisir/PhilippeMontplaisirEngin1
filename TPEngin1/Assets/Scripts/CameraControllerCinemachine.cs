using Cinemachine;
using UnityEngine;

public class CameraControllerCinemachine : MonoBehaviour
{
    
    private CinemachineVirtualCamera m_virtualCinemachineCamera;

    private void Awake()
    {
        m_virtualCinemachineCamera = GetComponent<CinemachineVirtualCamera>();
    }


    private void Update()
    {
        // Get mouse inputs
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");



        // Apply horizontal (yaw) rotation
        //transform.RotateAround(lookAtTarget.position, Vector3.up, horizontalInput * rotationSpeed);
        //
        //// Apply vertical (pitch) rotation
        //transform.RotateAround(lookAtTarget.position, transform.right, -verticalInput * rotationSpeed);
    }



    //m_vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance

    //public float rotationSpeed = 1.0f;
    //public Transform lookAtTarget;  // This should be your character



    //private CinemachineVirtualCamera m_virtualCinemachineCamera;
    //[SerializeField]
    //private float m_rotationSpeed = 5.0f;
    //
    //private void Awake()
    //{
    //    m_virtualCinemachineCamera = GetComponent<CinemachineVirtualCamera>();
    //}
    //
    //private void LateUpdate()
    //{
    //    UpdateHorizontalMovements();
    //    UpdateVerticalMovements();
    //    
    //}
    //
    //private void UpdateHorizontalMovements()
    //{
    //    float currentAngleX = Input.GetAxis("Mouse X") * m_rotationSpeed;
    //    transform.RotateAround(m_virtualCinemachineCamera.m_LookAt.position, Vector3.up, currentAngleX);
    //}
    //
    //private void UpdateVerticalMovements()
    //{
    //    //float currentAngleY = Input.GetAxis("Mouse Y") * m_rotationSpeed;
    //    //float eulersAngleX = transform.rotation.eulerAngles.x;
    //    //
    //    //float comparisonAngle = eulersAngleX + currentAngleY;
    //    //
    //    //comparisonAngle = ClampAngle(comparisonAngle);
    //    //
    //    //if ((currentAngleY < 0 && comparisonAngle < m_clampingXRotationValues.x)
    //    //    || (currentAngleY > 0 && comparisonAngle > m_clampingXRotationValues.y))
    //    //{
    //    //    return;
    //    //}
    //    //transform.RotateAround(m_objectToLookAt.position, transform.right, currentAngleY);
    //}
}
