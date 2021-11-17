using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class DICOMViewer : MonoBehaviour
{
    [SerializeField]
    private GameObject m_OpenPanel = null;

    [SerializeField]
    private TMP_InputField m_ImputField = null;

    [SerializeField]
    private Button m_Button = null;

    [SerializeField]
    private Renderer m_VolumeRenderer = null;

    [SerializeField]
    private Texture3D m_Texture3D = null;

    [SerializeField]
    private Scrollbar m_MinXScrollbar = null;
    
    [SerializeField]
    private Scrollbar m_MaxXScrollbar = null;

    [SerializeField]
    private Scrollbar m_MinYScrollbar = null;

    [SerializeField]
    private Scrollbar m_MaxYScrollbar = null;

    [SerializeField]
    private Scrollbar m_MinZScrollbar = null;

    [SerializeField]
    private Scrollbar m_MaxZScrollbar = null;


    private Vector3 m_DefaultPosition = new Vector3();

    private Quaternion m_DefaultQuaternion = Quaternion.identity;
    
    private void Awake()
    {
        m_MinXScrollbar.value = m_VolumeRenderer.sharedMaterial.GetFloat("_MinX");
        m_MaxXScrollbar.value = m_VolumeRenderer.sharedMaterial.GetFloat("_MaxX");
        m_MinYScrollbar.value = m_VolumeRenderer.sharedMaterial.GetFloat("_MinY");
        m_MaxYScrollbar.value = m_VolumeRenderer.sharedMaterial.GetFloat("_MaxY");
        m_MinZScrollbar.value = m_VolumeRenderer.sharedMaterial.GetFloat("_MinZ");
        m_MaxZScrollbar.value = m_VolumeRenderer.sharedMaterial.GetFloat("_MaxZ");

        m_DefaultPosition = transform.position;
        m_DefaultQuaternion = transform.rotation;
    }

    private void Update()
    {
        m_VolumeRenderer.sharedMaterial.SetFloat("_MinX", m_MinXScrollbar.value);
        m_VolumeRenderer.sharedMaterial.SetFloat("_MaxX", m_MaxXScrollbar.value);
        m_VolumeRenderer.sharedMaterial.SetFloat("_MinY", m_MinYScrollbar.value);
        m_VolumeRenderer.sharedMaterial.SetFloat("_MaxY", m_MaxYScrollbar.value);
        m_VolumeRenderer.sharedMaterial.SetFloat("_MinZ", m_MinZScrollbar.value);
        m_VolumeRenderer.sharedMaterial.SetFloat("_MaxZ", m_MaxZScrollbar.value);

        if (string.IsNullOrEmpty(m_ImputField.text))
        {
            m_Button.interactable = false;
        }
        else
        {
            m_Button.interactable = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = m_DefaultPosition;
            transform.rotation = m_DefaultQuaternion;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            m_OpenPanel.SetActive(true);
        }

        if (Input.GetMouseButton(0) && !m_OpenPanel.activeSelf)
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -5.0f, 0, Input.GetAxis("Mouse X") * 5.0f));
        }
    }
    public void OpenDICOM()
    {
        if (!string.IsNullOrEmpty(m_ImputField.text))
        {
            m_Texture3D = DICOMReader.ReadDICOM(m_ImputField.text);

            if (m_Texture3D != null)
            {
                m_VolumeRenderer.sharedMaterial.SetTexture("_Volume", m_Texture3D);
                m_OpenPanel.SetActive(false);
            }
            else
            {
                Debug.LogError("File can't open.");
            }
        }
    }
}
