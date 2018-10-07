using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514EnemySpawner : MonoBehaviour {

    public bool m_SpawnTrig = false;
    public k514enemyController[] prefab;
    private List<k514enemyController> m_pooledEnemy;

	public static k514EnemySpawner singleton = null;
	void Awake(){
		if(singleton == null) singleton = this;
		else if(singleton != this) Destroy(gameObject);
    }

    void Start(){
        Init();
		Debug.Log("Enemy Spawner Loaded");
    }

    void Init() {
        m_pooledEnemy = new List<k514enemyController>();
    }

    void Update()
    {
        if (m_SpawnTrig) {
            m_SpawnTrig = false;
            Spawn(new Vector3(Random.Range(-24f, 24f), Random.Range(-4f, 1f), 0f));
        }
    }

    public void Return(k514enemyController p_enemy) {
        p_enemy.transform.parent = transform;
        p_enemy.gameObject.SetActive(false);
        m_pooledEnemy.Add(p_enemy);
    }

    public k514enemyController Spawn(Vector3 p_pos,k514SystemManager.ENEMY_TYPE p_type = k514SystemManager.ENEMY_TYPE.REMILIA) {
        k514enemyController l_result = null;
        if (m_pooledEnemy.Count < 1)
        {
            l_result = Instantiate<k514enemyController>( prefab[(int)p_type] );
        }
        else {
            int l_lastIndex = m_pooledEnemy.Count - 1;
            l_result = m_pooledEnemy[l_lastIndex];
            m_pooledEnemy.RemoveAt(l_lastIndex);
            l_result.gameObject.SetActive(true);
        }
        l_result.transform.position = p_pos;
        l_result.transform.parent = null;
        return l_result;
    }

}
