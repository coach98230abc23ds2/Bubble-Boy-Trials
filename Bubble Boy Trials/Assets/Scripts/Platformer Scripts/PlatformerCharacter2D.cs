using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        private bool m_double_jump = false; // For checking if a player can double jump.
        private bool scroll_left = false;
        private bool scroll_right = false;

        public static bool can_move = true;

        private BGScroller m_scroller;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
//            m_scroller = GameObject.Find("Background").GetComponent<BGScroller>();
        }

        private void Update()
        {   
//            Debug.Log(System.Convert.ToString(scroll_left) + System.Convert.ToString(scroll_right));
//            if (scroll_left)
//            {
//                m_scroller.ScrollBG(-m_Rigidbody2D.velocity.x);
//                scroll_left = false;
//            }
//            else if (scroll_right)
//            {
//                m_scroller.ScrollBG(m_Rigidbody2D.velocity.x);
//                scroll_right = false;
//            }

        }
        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
//            Debug.Log("Setting grounded as" + System.Convert.ToString(m_Grounded));
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            // m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }


        public void Move(float move, bool jump)
        {

            //only control the player if grounded or airControl is turned on
            if ((m_Grounded || m_AirControl) && can_move)
            {
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip(); 
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }

                if (m_Rigidbody2D.velocity.x < 0)
                {
                    scroll_left = true;
                }
                else if (m_Rigidbody2D.velocity.x > 0)
                {
                    scroll_right = true;
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground") && can_move)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                m_double_jump = true;
            }else if (jump && !m_Grounded && can_move){

                //Allows the player to jump again.
                if (m_double_jump){
                    m_double_jump = false;
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce/2));
                }
            }

        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
