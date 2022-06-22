using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;

    public Transform m_target;

    private Vector3 m_MoveVelocity;
    private Vector3 m_DesiredPosition;

    private float m_ScrollSpeed = 10f;
    private Camera m_ZoomCamera;

    private void Awake()
    {
        m_target = GameObject.FindGameObjectWithTag("Player").transform;

        m_ZoomCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Move();

        if (m_ZoomCamera.orthographic)
        {
            m_ZoomCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * m_ScrollSpeed; // uses mose wheel to zoom at the set scroll speed
            m_ZoomCamera.orthographicSize = Mathf.Clamp(m_ZoomCamera.orthographicSize, 5, 20); // locks camera zoom to min and max values
        }
    }

    private void Move()
    {
        m_DesiredPosition = m_target.position;
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }
    
}
