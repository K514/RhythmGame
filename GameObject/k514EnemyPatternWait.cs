using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514EnemyPatternWait : k514EnemyPattern {

    float m_time,m_elapsed;

    public void Init(k514enemyController l_enemy, float p_time, Vector3? p_Offset=null)
    {
        Vector3 l_offset = p_Offset ?? Vector3.zero;
        base.Init(l_enemy, l_offset);
        gameObject.name = "Wait";
        m_time = p_time;
    }

    public override void DoAct() {
        base.DoAct();
        m_elapsed += Time.deltaTime;
    }

    public override bool EndTrigger() {
        return m_elapsed > m_time;
    }
    
}
