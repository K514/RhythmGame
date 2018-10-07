using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514EnemyPatternRollinghUp : k514EnemyPattern {

    k514SystemManager.DIRECT m_direct;
    float m_duration,m_elapsed,m_speed;

    public void Init(k514enemyController l_enemy, k514SystemManager.DIRECT p_direct,float p_duration, float p_speed, Vector3? p_Offset=null)
    {
        Vector3 l_offset = p_Offset ?? Vector3.zero;
        base.Init(l_enemy, l_offset);
        gameObject.name = "RollingUp";
        m_direct = p_direct;
        m_duration = p_duration;
        m_speed = p_speed;
    }

    public override void DoAct() {
        base.DoAct();
        m_renderer.flipX = (m_direct != k514SystemManager.DIRECT.RIGHT);
        m_animator.Play("RollingUp");
        m_rigid.gravityScale = 0f;
        m_rigid.velocity = Vector2.zero;
        m_rigid.AddForce(new Vector2((int)m_direct , 1f) * m_speed);
        m_elapsed += Time.deltaTime;
    }

    public override void EndProcess()
    {
        m_rigid.gravityScale = 1f;
        m_rigid.velocity = Vector2.zero;
    }

    public override bool EndTrigger()
    {

        bool l_result = m_elapsed > m_duration;
        if (l_result) EndProcess();
        return l_result;
    }
}
