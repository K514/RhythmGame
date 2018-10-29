using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514EnemyPatternFaint : k514EnemyPattern {

    float m_duration,m_elapsed;
    bool OnceTrig = true;
    public void Init(k514enemyController l_enemy,float p_duration,Vector3? p_Offset=null) {
        Vector3 l_offset = p_Offset ?? Vector3.zero;
        base.Init(l_enemy, l_offset);
        gameObject.name = "Faint";
        m_duration = p_duration;
    }

    public override void DoAct() {
        base.DoAct();
        if(OnceTrig){
                OnceTrig = false;
                k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.REMIDOWN, null);
        } 
        m_animator.Play("Faint");
        m_elapsed += Time.deltaTime;
    }

    public override void EndProcess()
    {
    }

    public override bool EndTrigger()
    {

        bool l_result = m_elapsed > m_duration;
        if (l_result) EndProcess();
        return l_result;
    }
}
