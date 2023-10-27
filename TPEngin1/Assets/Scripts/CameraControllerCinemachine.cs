using Cinemachine;
using UnityEngine;

public class CameraControllerCinemachine : MonoBehaviour
{    
    private CinemachineVirtualCamera m_virtualCinemachineCamera;
    private float m_cameraDistance;

    [SerializeField]
    private Vector2 m_cameraScrollLimits = Vector2.zero;

    private void Awake()
    {
        m_virtualCinemachineCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        float scrollInput = Input.mouseScrollDelta.y;        

        m_virtualCinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance -= scrollInput;
        m_cameraDistance = m_virtualCinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;
        m_virtualCinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = Mathf.Clamp(m_cameraDistance, m_cameraScrollLimits.x, m_cameraScrollLimits.y);
    }
}
