using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514EnemyPatternDashDown : k514EnemyPattern {

    float m_reversedDuration,m_elapsed;
    Vector3 m_targetPlace,m_tangent;

    public void Init(k514enemyController l_enemy,float p_duration, Vector3 p_targetPlace, Vector3? p_Offset = null) {
        Vector3 l_offset = p_Offset??Vector3.zero;
        base.Init(l_enemy, l_offset);
        gameObject.name = "DashDown";
        m_reversedDuration = 1f/p_duration;
        m_targetPlace = p_targetPlace;
    }

    public override void DoAct() {
        base.DoAct();
        m_renderer.flipX = m_targetPlace.x < m_initPlace.x;
        m_animator.Play("DashDown");
        m_rigid.gravityScale = 0f;
        m_rigid.velocity = Vector2.zero;
        m_elapsed += Time.deltaTime * m_reversedDuration;
        m_target.position = Vector3.Lerp(m_target.position, m_targetPlace, m_elapsed);
    }

    public override void EndProcess()
    {
        m_rigid.gravityScale = 1f;
        m_rigid.velocity = Vector2.zero;
    }

    public override bool EndTrigger()
    {

        bool l_result = m_elapsed > 1f;
        if (l_result) EndProcess();
        return l_result;
    }
}
