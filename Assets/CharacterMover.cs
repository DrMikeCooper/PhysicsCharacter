using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float m_speed = 5;
    public float m_jumpForce = 5;

    // horizontal movement input in x and z
    Vector3 m_movement;
    // do we want to jump this frame?
    bool m_jump;

    public bool m_grounded = true;

    CharacterController cc;
    Vector3 m_velocity;
    CollisionFlags m_flags;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_movement.x = Input.GetAxis("Horizontal") *m_speed;
        m_movement.y = 0;
        m_movement.z = Input.GetAxis("Vertical") * m_speed;
        m_jump = Input.GetButtonDown("Jump");

        animator.SetBool("Jump", !m_grounded);
        animator.SetFloat("Forwards", m_movement.magnitude);
        animator.SetBool("Crouch", Input.GetKey(KeyCode.C));
    }

    void FixedUpdate()
    {
        m_grounded = cc.isGrounded;

        // if we're on the ground and press jump, start jumping up
        if (m_grounded && m_jump)
        {
            m_velocity.y = m_jumpForce;
        }

        // don't let velocity accumulate massively, and don't zero it if we're going upwards or we can't jump
        if (m_grounded && m_velocity.y < 0)
        {
            m_velocity.y = 0;
        }

        //always apply gravity to get a reliable grounded flag
        m_velocity += Physics.gravity* Time.deltaTime;
        m_movement.y = m_velocity.y;

        // change direction or stop (only on ground)
        if (m_grounded || m_movement.x != 0)
            m_velocity.x = m_movement.x;
        if (m_grounded || m_movement.z != 0)
            m_velocity.z = m_movement.z;

        if (!m_grounded)
            hitDirection = Vector3.zero;

        // slide objects off surfaces they're hanging on to
        if (m_movement.x == 0 && m_movement.z == 0)
        {
            Vector3 horizontalHitDirection = hitDirection;
            horizontalHitDirection.y = 0;
            float displacement = horizontalHitDirection.magnitude;
            if (displacement > 0)
                m_velocity -= 0.2f * horizontalHitDirection / displacement;
        }

        m_flags = cc.Move(m_velocity * Time.deltaTime);
    }

    public Vector3 hitDirection;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitDirection = hit.point - transform.position;


    }
}
