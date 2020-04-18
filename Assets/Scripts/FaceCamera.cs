using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Canvas m_canvas;
    private Camera m_camera = null;

    void Awake()
    {
        m_camera = Camera.main;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = m_camera.transform.position - m_canvas.transform.position;
        m_canvas.transform.LookAt(-direction * 1000f, Vector3.up);

        Vector3 position = m_canvas.transform.localPosition;
        position.z = -1.0f;
        m_canvas.transform.localPosition = position;
    }
}
