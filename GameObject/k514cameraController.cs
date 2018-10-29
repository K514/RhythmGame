using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514cameraController : MonoBehaviour {

    GameObject m_player = null;
    Vector3 m_targetView,m_initPlace,m_velocity;
    // Update is called once per frame

    void Start()
    {
        m_initPlace = transform.position;
    }

	void Update () {
        if (m_player == null)
        {
            m_player = GameObject.FindGameObjectWithTag("Player");
            m_targetView = m_initPlace;
        }
        else {
            if (m_player.transform.position.x < 8f && m_player.transform.position.x > -8f) {
                m_targetView = Vector3.Scale(new Vector3(0, 1, 1), m_targetView) + Vector3.right * m_player.transform.position.x;
            }
        }
        transform.position = Vector3.SmoothDamp(transform.position, m_targetView,ref m_velocity, .45f);
    }
}
