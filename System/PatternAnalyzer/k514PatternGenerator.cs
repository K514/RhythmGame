using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514PatternGenerator : MonoBehaviour {

	public static k514PatternGenerator singleton = null;
    public bool m_SpawnTrig = false;
	void Awake(){
		if(singleton == null) singleton = this;
		else if(singleton != this) Destroy(gameObject);
    }

    void Start(){
        Init();
		Debug.Log("Pattern Generator Loaded");
    }

    void Init() {
    }

    void Update()
    {
        if (m_SpawnTrig)
        {
            m_SpawnTrig = false;
            // WalkPattern(new Vector3(Random.Range(-24f, 24f), -3f, 0f), k514SystemManager.DIRECT.RIGHT);
            //if(Random.Range(0,2) == 0)
            //DashUpDownPattern(new Vector3(Random.Range(-24f, 24f), -3f, 0f), k514SystemManager.DIRECT.LEFT);
            //else
            //DashUpDownPattern(new Vector3(Random.Range(-24f, 24f), -3f, 0f), k514SystemManager.DIRECT.RIGHT);
            // WaitPattern(new Vector3(Random.Range(-24f, 24f), -3f, 0f));
            float l_timeoffset = 0f;
            k514SystemManager.DIRECT l_direction = Random.Range(0, 2) == 0 ? k514SystemManager.DIRECT.LEFT : k514SystemManager.DIRECT.RIGHT;
            k514enemyController l_tmp = k514SystemManager.SYSTEM_PATTERN_GENERATOR.RandomGenerateUpDownPattern(out l_timeoffset, Vector3.right * Random.Range(-24f, 24f), l_direction);
            l_tmp.StartPattern();
            Debug.Log(l_timeoffset);
        }

    }

    public void WaitPattern(Vector3 p_pos) {
        List<k514EnemyPattern> l_result = new List<k514EnemyPattern>();
        k514enemyController l_enemy = k514SystemManager.SYSTEM_ENEMY_SPAWNER.Spawn(p_pos);
        l_enemy.SetPattern(l_result);

        k514EnemyPatternWait l_waitPattern = new GameObject().AddComponent<k514EnemyPatternWait>();
        l_waitPattern.transform.parent = l_enemy.transform;
        l_waitPattern.Init(l_enemy, 5f);
        l_result.Add(l_waitPattern);
        l_enemy.StartPattern();
    }

    public void WalkPattern(Vector3 p_pos,k514SystemManager.DIRECT p_direct) {
        List<k514EnemyPattern> l_result = new List<k514EnemyPattern>();
        k514enemyController l_enemy = k514SystemManager.SYSTEM_ENEMY_SPAWNER.Spawn(p_pos);
        l_enemy.SetPattern(l_result);

        k514EnemyPatternWalk l_walkPattern = new GameObject().AddComponent<k514EnemyPatternWalk>();
        l_walkPattern.transform.parent = l_enemy.transform;
        l_walkPattern.Init(l_enemy, p_direct,10f,100f,Vector3.up*-0.14f);
        l_result.Add(l_walkPattern);
        l_enemy.StartPattern();
    }

    public void DashUpDownPattern(Vector3 p_pos, k514SystemManager.DIRECT p_direct)
    {
        List<k514EnemyPattern> l_result = new List<k514EnemyPattern>();
        k514enemyController l_enemy = k514SystemManager.SYSTEM_ENEMY_SPAWNER.Spawn(p_pos);
        l_enemy.SetPattern(l_result);

        k514EnemyPatternDashUp l_dashUpPattern = new GameObject().AddComponent<k514EnemyPatternDashUp>();
        l_dashUpPattern.transform.parent = l_enemy.transform;
        l_dashUpPattern.Init(l_enemy, p_direct, 0.125f, 400f);
        l_result.Add(l_dashUpPattern);

        k514EnemyPatternRollinghUp l_rollUpPattern = new GameObject().AddComponent<k514EnemyPatternRollinghUp>();
        l_rollUpPattern.transform.parent = l_enemy.transform;
        l_rollUpPattern.Init(l_enemy, p_direct, 0.125f, 400f);
        l_result.Add(l_rollUpPattern);

        k514EnemyPatternDashDown l_dashDownPattern = new GameObject().AddComponent<k514EnemyPatternDashDown>();
        l_dashDownPattern.transform.parent = l_enemy.transform;
        l_dashDownPattern.Init(l_enemy, 0.05f, p_pos + (int)p_direct*Vector3.right * 10f,Vector3.up*-0.3f);
        l_result.Add(l_dashDownPattern);

        k514EnemyPatternFaint l_faintPattern = new GameObject().AddComponent<k514EnemyPatternFaint>();
        l_faintPattern.transform.parent = l_enemy.transform;
        l_faintPattern.Init(l_enemy, 1f, Vector3.up*-0.42f);
        l_result.Add(l_faintPattern);

        k514EnemyPatternRaise l_raisePattern = new GameObject().AddComponent<k514EnemyPatternRaise>();
        l_raisePattern.transform.parent = l_enemy.transform;
        l_raisePattern.Init(l_enemy, 0.5f);
        l_result.Add(l_raisePattern);

        k514EnemyPatternRaise2 l_raisePattern2 = new GameObject().AddComponent<k514EnemyPatternRaise2>();
        l_raisePattern2.transform.parent = l_enemy.transform;
        l_raisePattern2.Init(l_enemy, 0.5f,new Vector3(1f*(float)p_direct,0.2f,0f));
        l_result.Add(l_raisePattern2);

        k514EnemyPatternPose l_posePattern = null;

        l_posePattern = new GameObject().AddComponent<k514EnemyPatternPose>();
        l_posePattern.transform.parent = l_enemy.transform;
        l_posePattern.Init(l_enemy, 1.5f,"Pose1",Vector3.up*0.18f);
        l_result.Add(l_posePattern);

        l_posePattern = new GameObject().AddComponent<k514EnemyPatternPose>();
        l_posePattern.transform.parent = l_enemy.transform;
        l_posePattern.Init(l_enemy, 0.5f,"Idle");
        l_result.Add(l_posePattern);

        k514EnemyPatternWalk l_walkPattern = new GameObject().AddComponent<k514EnemyPatternWalk>();
        l_walkPattern.transform.parent = l_enemy.transform;
        l_walkPattern.Init(l_enemy, p_direct,20f,300f,Vector3.up*-0.14f);
        l_result.Add(l_walkPattern);

        l_enemy.StartPattern();
    }

    // Ÿ�� �ɼ�,��ġ�� �� ��� [-24,24], ���ư� ����
    public k514enemyController RandomGenerateUpDownPattern(out float offset,Vector3 p_pos, k514SystemManager.DIRECT p_direct)
    {
        // ref name
        k514EnemyPatternDashUp l_dashUpPattern = null;
        k514EnemyPatternRollinghUp l_rollUpPattern = null;
        k514EnemyPatternDashDown l_dashDownPattern = null;
        k514EnemyPatternFaint l_faintPattern = null;
        k514EnemyPatternRaise l_raisePattern = null;
        k514EnemyPatternRaise2 l_raisePattern2 = null;
        k514EnemyPatternPose l_posePattern = null;
        k514EnemyPatternWalk l_walkPattern = null;
        k514EnemyPatternWalkInTime l_walkTimePattern = null;

        //
        Vector3 l_generatePlace = Vector3.down *3.3f + Vector3.left * (int)p_direct * 25f;
        float l_offset = 0f,l_rand = 0f;
        // �� �� ������ �����ϰ� ����.
        List<k514EnemyPattern> l_result = new List<k514EnemyPattern>();
        k514enemyController l_enemy = k514SystemManager.SYSTEM_ENEMY_SPAWNER.Spawn(l_generatePlace);
        l_enemy.SetPattern(l_result);

        // pattern regist
        l_rand = Random.Range(0.1f, 0.5f);
        l_walkTimePattern = new GameObject().AddComponent<k514EnemyPatternWalkInTime>();
        l_walkTimePattern.transform.parent = l_enemy.transform;
        l_walkTimePattern.Init(l_enemy, p_direct,0.5f*(p_pos.x + l_generatePlace.x), l_rand, Vector3.up * -0.14f);
        l_result.Add(l_walkTimePattern);
        l_offset += l_rand;

        l_rand = Random.Range(0.125f, 0.225f);
        l_dashUpPattern = new GameObject().AddComponent<k514EnemyPatternDashUp>();
        l_dashUpPattern.transform.parent = l_enemy.transform;
        l_dashUpPattern.Init(l_enemy, p_direct, l_rand, 400f);
        l_result.Add(l_dashUpPattern);
        l_offset += l_rand;

        l_rand = Random.Range(0.125f, 0.225f);
        l_rollUpPattern = new GameObject().AddComponent<k514EnemyPatternRollinghUp>();
        l_rollUpPattern.transform.parent = l_enemy.transform;
        l_rollUpPattern.Init(l_enemy, p_direct, l_rand, 400f);
        l_result.Add(l_rollUpPattern);
        l_offset += l_rand;

        l_rand = Random.Range(0.1f, 0.2f);
        l_dashDownPattern = new GameObject().AddComponent<k514EnemyPatternDashDown>();
        l_dashDownPattern.transform.parent = l_enemy.transform;
        l_dashDownPattern.Init(l_enemy, l_rand, p_pos + Vector3.down * 3.3f, Vector3.up * -0.3f);
        l_result.Add(l_dashDownPattern);
        l_offset += l_rand;

        l_rand = Random.Range(0.8f, 1.6f);
        l_faintPattern = new GameObject().AddComponent<k514EnemyPatternFaint>();
        l_faintPattern.transform.parent = l_enemy.transform;
        l_faintPattern.Init(l_enemy, l_rand, Vector3.up * -0.42f);
        l_result.Add(l_faintPattern);

        l_raisePattern = new GameObject().AddComponent<k514EnemyPatternRaise>();
        l_raisePattern.transform.parent = l_enemy.transform;
        l_raisePattern.Init(l_enemy, 0.5f);
        l_result.Add(l_raisePattern);

        l_raisePattern2 = new GameObject().AddComponent<k514EnemyPatternRaise2>();
        l_raisePattern2.transform.parent = l_enemy.transform;
        l_raisePattern2.Init(l_enemy, 0.5f, new Vector3(1f * (float)p_direct, 0.2f, 0f));
        l_result.Add(l_raisePattern2);

        l_posePattern = new GameObject().AddComponent<k514EnemyPatternPose>();
        l_posePattern.transform.parent = l_enemy.transform;
        l_posePattern.Init(l_enemy, 1.5f, "Pose1", Vector3.up * 0.18f);
        l_result.Add(l_posePattern);

        l_rand = Random.Range(0.8f, 1.6f);
        l_posePattern = new GameObject().AddComponent<k514EnemyPatternPose>();
        l_posePattern.transform.parent = l_enemy.transform;
        l_posePattern.Init(l_enemy, l_rand, "Idle");
        l_result.Add(l_posePattern);

        l_rand = Random.Range(225f, 325f);
        l_walkPattern = new GameObject().AddComponent<k514EnemyPatternWalk>();
        l_walkPattern.transform.parent = l_enemy.transform;
        l_walkPattern.Init(l_enemy, p_direct, 50f, l_rand, Vector3.up * -0.14f);
        l_result.Add(l_walkPattern);

        offset = l_offset;
        return l_enemy;
    }

}
