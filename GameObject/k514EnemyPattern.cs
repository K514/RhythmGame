using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class k514EnemyPattern : MonoBehaviour {
    protected Transform m_target;
    protected k514enemyController m_targetScomponent = null;
    protected Animator m_animator;
    protected Rigidbody2D m_rigid;
    protected SpriteRenderer m_renderer;
    protected bool m_endTrig = false;
    protected Vector3 m_initPlace;
    protected Vector3 m_Offset;

    protected virtual void Init(k514enemyController l_enemy,Vector3 p_Offset) {
        m_targetScomponent = l_enemy;
        m_animator = m_targetScomponent.m_animator;
        m_rigid = m_targetScomponent.m_rigid2D;
        m_renderer = m_targetScomponent.m_renderer;
        m_target = m_targetScomponent.transform;
        m_initPlace = m_target.position;
        m_Offset = p_Offset;
    }

    public virtual void DoAct() {
        m_targetScomponent.SetSpriteOffset(m_Offset);
    }

    public virtual void EndProcess() {

    }

    public virtual bool EndTrigger() {
        return m_endTrig;
    }
    
}
