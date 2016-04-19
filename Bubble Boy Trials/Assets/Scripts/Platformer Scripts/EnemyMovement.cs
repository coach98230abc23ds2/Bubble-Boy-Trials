using UnityEngine;
using System.Collections;
using System;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] private LayerMask m_WhatIsGround;   

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded

    private int m_move_state = 0;
    private int m_direction = -1;
    private float m_speed = 5.0f;
    private float m_timer = 0.0f;
    private float curr_y_pos;
    private bool m_can_move = false;
    private bool m_move_up = false; 
    private bool m_just_switched = false;
    private bool m_Grounded;
    private bool m_incline;
    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    
    private void Awake(){
        m_GroundCheck = transform.Find("GroundCheck");
    }

    //moves enemy up and down;
    private void MoveUpAndDown(){
        transform.Translate(Vector2.up * m_speed * m_direction* Time.deltaTime);
        m_just_switched = false;
    }


    //switches enemy's current moving direction
    private void SwitchDirection(){
        m_direction *= -1;
        m_timer = 1.0f;
        m_just_switched = true;
    }
        
    //used for dealing with rigidbodies; called every physics step
    void FixedUpdate (){

        m_Grounded = false;
        m_incline = false;

        try{
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    Debug.Log("set grounded to be true.");
                    m_Grounded = true;

                if (colliders[i].gameObject.tag == "Incline")
                    Debug.Log("set incline to be true.");
                    m_incline = true;
            }
        }catch(Exception e){
            Debug.Log("no collider below enemy");
        }

        if (m_can_move == true){
            if (this.name == "enemy1" || this.name == "enemy1(Clone)"){
                if (!m_Grounded){
                    this.GetComponent<Rigidbody2D>().velocity.Normalize();
                }else if (m_incline){
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-15, 0));
                }else{
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-5, 0));
                }
            }
        }
    }

    void Update(){
        curr_y_pos = this.transform.position.y;

        if (m_timer >= 1.0f){
            m_can_move = true; 
        }

        m_timer += Time.deltaTime;

        if (m_can_move == true){
            if (this.name == "enemy2" || this.name == "enemy2(Clone)"){
                MoveUpAndDown();          
//                Debug.Log ("got to enemy2");
                if (m_timer > 3.0f){
                    SwitchDirection();
                }
            }
        }
    }
}
