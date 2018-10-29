using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514enemyController : MonoBehaviour {

    public Animator m_animator = null;
    public SpriteRenderer m_renderer = null;
    public Rigidbody2D m_rigid2D = null;
    List<k514EnemyPattern> m_PatternList = null;
    Coroutine m_Pattern = null;
    Color m_initColor;
    Vector3 m_spriteOffset;

	public void SetPattern (List<k514EnemyPattern> l_list) {
        m_animator = GetComponentInChildren<Animator>();
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        m_rigid2D = GetComponent<Rigidbody2D>();
        m_PatternList = l_list;
        m_initColor = m_renderer.color;
    }

    public void SetSpriteOffset(Vector3 p_offset) {
        m_spriteOffset = p_offset;
    }

    public void StartPattern(){
        m_Pattern = StartCoroutine(DoAct());
    }

    IEnumerator DoAct(){
        yield return null;
        k514EnemyPattern c_Pattern = null;
        int index = m_PatternList.Count;
        for (int i = 0; i <index; i++)
        {
            c_Pattern = m_PatternList[i];
            while (!c_Pattern.EndTrigger()) {
                c_Pattern.DoAct();
                m_renderer.transform.localPosition = m_spriteOffset;
                yield return null;
            }
        }
        EndProcess();
    }

    void EndProcess() {
        m_PatternList = null;
        m_Pattern = null;
        m_rigid2D.velocity = Vector2.zero;
        m_renderer.color = m_initColor;
        m_renderer.flipX = false;
        m_animator.Play("Idle");

        k514EnemyPattern[] l_patterns = GetComponentsInChildren<k514EnemyPattern>();
        for (int i = 0; i < l_patterns.Length; i++) Destroy(l_patterns[i].gameObject);

        k514SystemManager.SYSTEM_ENEMY_SPAWNER.Return(this);

    }

}
