using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514playerController : MonoBehaviour {

    public float m_speed;
    public float m_jumpPower;
    public enum PLAYER_MOTION {
           SHELL_OUT,ATTACK,CASTING,END
    }

    PLAYER_MOTION m_currentMotion = PLAYER_MOTION.SHELL_OUT;
    Animator m_animator = null;
    SpriteRenderer m_renderer = null;
    Rigidbody2D m_rigid2D = null;
    bool m_moveTrig = false,m_motionTrig = true,m_jumpAvailableTrig = false,m_jumpApplyOnceTrig = false,m_wallCollideTrig = false;
    float m_horiz = 0f,m_wall = 0f;
    Vector2 m_moveDirect;
    Coroutine m_motionTimer = null;

	// Use this for initialization
	void Start () {
        m_animator = GetComponentInChildren<Animator>();
        m_renderer = GetComponentInChildren<SpriteRenderer>();
        m_rigid2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        KeyCheck();

        if (m_jumpAvailableTrig)
        {
            AttackCheck();
            CastingCheck();
        }
        if (!m_motionTrig)
        {
            SightCheck();
            JumpCheck();
            ApplyHorizontal();
        }
        WallCheck();
        ApplyMove();
        ApplyAnimation();
	}

    void SightCheck() {
        if (m_horiz > 0) m_renderer.flipX = false;
        if (m_horiz < 0) m_renderer.flipX = true;
    }

    void KeyCheck() {
        m_horiz = Input.GetAxis("Horizontal");
        m_moveTrig = m_horiz != 0f;
    }

    void JumpCheck() {
        if (!m_jumpAvailableTrig || m_jumpApplyOnceTrig){
            return;
        }
        if (Input.GetButton("Jump"))
        {
            m_jumpApplyOnceTrig = true;
            m_moveDirect.y = m_jumpPower;
        }
    }

    void AttackCheck() {
        if (!m_motionTrig && Input.GetButtonDown("Fire2")) {
            m_motionTrig = true;
            m_currentMotion = PLAYER_MOTION.ATTACK;
        }
    }

    void CastingCheck()
    {
        if (!m_motionTrig && Input.GetButtonDown("Fire1"))
        {
            m_motionTrig = true;
            m_currentMotion = PLAYER_MOTION.CASTING;
        }
    }

    void ApplyHorizontal() {
        m_moveDirect.x = m_horiz * m_speed;
    }

    void WallCheck() {
        if (!m_wallCollideTrig) return;
        if (m_wall * m_moveDirect.x  > 0f)
        {
            m_moveDirect.x = 0f;
        }
    }

    void ApplyMove() {
        m_rigid2D.velocity = new Vector2(0f, m_rigid2D.velocity.y);
        m_rigid2D.AddForce(m_moveDirect);
        m_moveDirect = Vector2.zero;
    }

    void ApplyAnimation() {

        if (!m_motionTrig)
        {
            if (m_moveTrig)
            {
                m_animator.Play("Move");
            }
            else
            {
                m_animator.Play("Idle");
            }
        }
        else if(m_motionTimer==null)
        {
            switch (m_currentMotion)
            {
                case PLAYER_MOTION.SHELL_OUT:
                    if (!m_jumpAvailableTrig)
                    {
                        m_animator.Play("Init");
                        m_animator.speed = 0f;
                        return;
                    }
                    else {
                        m_animator.speed = 1f;
                    }
                    break;
                case PLAYER_MOTION.ATTACK:
                    Debug.Log("attack");
                    m_animator.Play("Attack");
                    break;
                case PLAYER_MOTION.CASTING:
                    Debug.Log("cast");
                    m_animator.Play("Casting");
                    break;
            }
            m_motionTimer = StartCoroutine(MotionTrigTimer());
        }

    }

    IEnumerator MotionTrigTimer() {
        yield return null;
        AnimatorStateInfo info = m_animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(info.length+0.5f);
        m_motionTrig = false;
        m_motionTimer = null;   
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Debug.Log("Platform in");
            m_jumpAvailableTrig = true;
            m_jumpApplyOnceTrig = false;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            // Debug.Log("Wall in");
            m_wallCollideTrig = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Debug.Log("Platform out");
            m_jumpAvailableTrig = false;
            m_wall = transform.position.x - collision.transform.position.x;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            // Debug.Log("Wall out");
            m_wallCollideTrig = false;
        }
    }

}
