using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514EnemyPatternWalkInTime : k514EnemyPattern {

    k514SystemManager.DIRECT m_direct;
    float m_time,m_reversedTime, m_elapsed, m_targetX;

    public void Init(k514enemyController l_enemy, k514SystemManager.DIRECT p_direct,float p_target,float p_time, Vector3? p_Offset=null)
    {
        Vector3 l_offset = p_Offset ?? Vector3.zero;
        base.Init(l_enemy, l_offset);
        gameObject.name = "WalkT";
        m_direct = p_direct;
        m_time = p_time;
        m_reversedTime = 1f/p_time;
        m_targetX = p_target;
    }

    public override void DoAct() {
        base.DoAct();
        m_renderer.flipX = (m_direct != k514SystemManager.DIRECT.RIGHT);
        m_animator.Play("Walk");
        m_target.position = new Vector3(Mathf.Lerp(m_initPlace.x, m_targetX, m_elapsed* m_reversedTime),m_target.position.y,0f);
        m_elapsed += Time.deltaTime;
    }

    public override void EndProcess()
    {
        m_rigid.velocity = Vector2.zero;
    }

    public override bool EndTrigger() {

        bool l_result = m_elapsed > m_time;
        if (l_result) EndProcess();
        return l_result;
    }
    
}
