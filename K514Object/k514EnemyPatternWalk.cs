using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514EnemyPatternWalk : k514EnemyPattern {

    k514SystemManager.DIRECT m_direct;
    float m_distance,m_speed;

    public void Init(k514enemyController l_enemy, k514SystemManager.DIRECT p_direct,float p_distance,float p_speed, Vector3? p_Offset=null)
    {
        Vector3 l_offset = p_Offset ?? Vector3.zero;
        base.Init(l_enemy, l_offset);
        gameObject.name = "Walk";
        m_direct = p_direct;
        m_distance = p_distance;
        m_speed = p_speed;
    }

    public override void DoAct() {
        base.DoAct();
        m_renderer.flipX = (m_direct != k514SystemManager.DIRECT.RIGHT);
        m_animator.Play("Walk");
        m_rigid.velocity = Vector2.Scale(m_rigid.velocity, Vector2.up);
        m_rigid.AddForce((int)m_direct * m_speed * Vector2.right);
    }

    public override void EndProcess()
    {
        m_rigid.velocity = Vector2.zero;
    }

    public override bool EndTrigger() {

        bool l_result = Vector2.Distance(Vector2.Scale(m_target.position,Vector2.right), m_initPlace) > m_distance;
        if (l_result) EndProcess();
        return l_result;
    }
    
}
