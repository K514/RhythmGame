using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514SpectrumManager : MonoBehaviour {

    public bool m_Rtrig = false, m_Gtrig = false, m_Btrig = false, m_Wtrig = false;
    public Material[] mat;
    public Image m_GraphPanel;
    private List<Vector3> m_PointQueue, m_PointQueue2, m_PointQueue3, m_PointQueue4;
    private List<LineRenderer> m_LineRenderer,m_LineRenderer2,m_LineRenderer3,m_LineRenderer4;
    private int QUEUE_SIZE = 100;
    private int NOW_INDEX1 = 0,NOW_INDEX2 = 0, NOW_INDEX3 = 0, NOW_INDEX4 = 0;
    [System.NonSerialized]public float m_GraphDomainMax = 2000f;

	public static k514SpectrumManager singleton = null;
	void Awake(){
		if(singleton == null) singleton = this;
		else if(singleton != this) Destroy(gameObject);
    }

    void Start(){
        Init();
		Debug.Log("Spectrum Manager Loaded");
    }

    void Init() {
        m_PointQueue = new List<Vector3>();
        m_PointQueue2 = new List<Vector3>();
        m_PointQueue3 = new List<Vector3>();
        m_PointQueue4 = new List<Vector3>();
        m_LineRenderer = new List<LineRenderer>();
        m_LineRenderer2 = new List<LineRenderer>();
        m_LineRenderer3 = new List<LineRenderer>();
        m_LineRenderer4 = new List<LineRenderer>();

        LineRenderer l_tmp = null;
        GameObject l_go = null;

        for (int i = 0; i < QUEUE_SIZE-1; i++) {
            l_go = new GameObject();
            l_go.name = "Line_" + i;
            l_go.transform.parent = m_GraphPanel.transform;
            l_tmp = l_go.AddComponent<LineRenderer>();
            l_tmp.transform.position = Vector3.zero;
            l_tmp.sharedMaterial = mat[0];
            l_tmp.startWidth = 1.5f;
            l_tmp.endWidth = 1.5f;
            m_LineRenderer.Add(l_tmp);
        }
        for (int i = 0; i < QUEUE_SIZE - 1; i++)
        {
            l_go = new GameObject();
            l_go.name = "LineA_" + i;
            l_go.transform.parent = m_GraphPanel.transform;
            l_tmp = l_go.AddComponent<LineRenderer>();
            l_tmp.transform.position = Vector3.zero;
            l_tmp.sharedMaterial = mat[1];
            l_tmp.startWidth = 1.5f;
            l_tmp.endWidth = 1.5f;
            m_LineRenderer2.Add(l_tmp);
        }
        for (int i = 0; i < QUEUE_SIZE - 1; i++)
        {
            l_go = new GameObject();
            l_go.name = "LineB_" + i;
            l_go.transform.parent = m_GraphPanel.transform;
            l_tmp = l_go.AddComponent<LineRenderer>();
            l_tmp.transform.position = Vector3.zero;
            l_tmp.sharedMaterial = mat[2];
            l_tmp.startWidth = 1.5f;
            l_tmp.endWidth = 1.5f;
            m_LineRenderer3.Add(l_tmp);
        }
        for (int i = 0; i < QUEUE_SIZE - 1; i++)
        {
            l_go = new GameObject();
            l_go.name = "LineW_" + i;
            l_go.transform.parent = m_GraphPanel.transform;
            l_tmp = l_go.AddComponent<LineRenderer>();
            l_tmp.transform.position = Vector3.zero;
            l_tmp.sharedMaterial = mat[3];
            l_tmp.startWidth = 2f;
            l_tmp.endWidth = 2f;
            m_LineRenderer4.Add(l_tmp);
        }
    }


    public void AddPointR(Vector2 p_point)
    {
        if(m_Rtrig) return;
        
        if (m_PointQueue.Count < QUEUE_SIZE)
        {
            m_PointQueue.Add(p_point);
        }
        else
        {
            int index = NOW_INDEX1 % QUEUE_SIZE;
            m_PointQueue[index] = p_point;
        }
        NOW_INDEX1++;
        DrawGraph(m_LineRenderer, m_PointQueue,NOW_INDEX1);
    }

    public void AddPointG(Vector2 p_point)
    {
        if(m_Gtrig) return;

        if (m_PointQueue2.Count < QUEUE_SIZE)
        {
            m_PointQueue2.Add(p_point);
        }
        else
        {
            int index = NOW_INDEX2 % QUEUE_SIZE;
            m_PointQueue2[index] = p_point;
        }
        NOW_INDEX2++;
        DrawGraph(m_LineRenderer2, m_PointQueue2,NOW_INDEX2);
    }

    public void AddPointB(Vector2 p_point)
    {
        if(m_Btrig) return;

        if (m_PointQueue3.Count < QUEUE_SIZE)
        {
            m_PointQueue3.Add(p_point);
        }
        else
        {
            int index = NOW_INDEX3 % QUEUE_SIZE;
            m_PointQueue3[index] = p_point;
        }
        NOW_INDEX3++;
        DrawGraph(m_LineRenderer3,m_PointQueue3,NOW_INDEX3);
    }

    public void AddPointW(Vector2 p_point)
    {
        if(m_Wtrig) return;
        
        if (m_PointQueue4.Count < QUEUE_SIZE)
        {
            m_PointQueue4.Add(p_point);
        }
        else
        {
            int index = NOW_INDEX4 % QUEUE_SIZE;
            m_PointQueue4[index] = p_point;
        }
        NOW_INDEX4++;
        DrawGraph(m_LineRenderer4, m_PointQueue4,NOW_INDEX4);
    }

    public void DrawGraph(List<LineRenderer> p_List,List<Vector3> p_ListVector,int NOW_INDEX)
    {
        float l_Coord_x_of_graph = m_GraphPanel.rectTransform.anchoredPosition.x;
        float l_Coord_y_of_graph = m_GraphPanel.rectTransform.anchoredPosition.y;
        float panel_width = m_GraphPanel.rectTransform.rect.width;
        float panel_height = m_GraphPanel.rectTransform.rect.height;
        float interval = panel_width / QUEUE_SIZE;
        float ratio = panel_height / m_GraphDomainMax;
        int offset = NOW_INDEX % QUEUE_SIZE;
        Vector3[] l_data = new Vector3[QUEUE_SIZE];
        for (int i = 0; i < p_ListVector.Count; i++)
        {
            l_data[i] = new Vector3(l_Coord_x_of_graph - panel_width * 0.5f + i * interval,
                                                                    -panel_height * 0.5f + l_Coord_y_of_graph + (p_ListVector[i].x+p_ListVector[i].y)*0.5f * ratio,
                                                                       0f);
            if (i > 0)
            {
                p_List[i - 1].SetPosition(0, l_data[i - 1]);
                p_List[i - 1].SetPosition(1, l_data[i]);
            }
        }
    }

}
