using Cinemachine;
using UnityEngine;

public class CameraControllerCinemachine : MonoBehaviour
{
    private CinemachineVirtualCamera m_virtualCinemachineCamera;
    private float m_cameraDistance;
    private float m_targetCameraDistance;

    [SerializeField]
    private float m_scrollLerpSpeed = 0.1f;
    [SerializeField]
    private Vector2 m_cameraScrollLimits = Vector2.zero;    

    private void Awake()
    {
        m_virtualCinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        m_cameraDistance = m_virtualCinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;
        m_targetCameraDistance = m_cameraDistance;
    }

    private void Update()
    {
        float scrollInput = Input.mouseScrollDelta.y;

        m_targetCameraDistance -= scrollInput;

        m_targetCameraDistance = Mathf.Clamp(m_targetCameraDistance, m_cameraScrollLimits.x, m_cameraScrollLimits.y);

        m_cameraDistance = m_virtualCinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;

        m_cameraDistance = Mathf.Lerp(m_cameraDistance, m_targetCameraDistance, m_scrollLerpSpeed);

        m_virtualCinemachineCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = m_cameraDistance;
    }    
}
