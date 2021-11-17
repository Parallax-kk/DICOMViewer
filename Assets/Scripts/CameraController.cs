using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_OpenPanel = null;

    [SerializeField]
    private float m_MoveStep = 0.1f;

    private Vector3 m_DefaultPosition = new Vector3();

    private Quaternion m_DefaultQuaternion = Quaternion.identity;

    private void Awake()
    {
        m_DefaultPosition = transform.position;
        m_DefaultQuaternion = transform.rotation;
    }

    private void Update()
    {
        if (!m_OpenPanel.activeSelf)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.up * m_MoveStep;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position -= transform.up * m_MoveStep;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position -= transform.right * m_MoveStep;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += transform.right * m_MoveStep;
            }

            transform.position += transform.forward * Input.mouseScrollDelta.y * m_MoveStep;

            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.position = m_DefaultPosition;
                transform.rotation = m_DefaultQuaternion;
            }
        }
    }
}