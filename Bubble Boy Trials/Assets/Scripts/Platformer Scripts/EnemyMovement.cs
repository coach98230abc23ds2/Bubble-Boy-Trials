using UnityEngine;
using System.Collections;
using System;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] private LayerMask m_WhatIsIncline;   
    [SerializeField] private LayerMask m_WhatIsGround; 

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded

    private int m_move_state = 0;
    private int m_direction = -1;
    private float m_speed = 5.0f;
    private float m_timer = 0.0f;
    private float curr_y_pos;
    public bool m_can_move = false;
    private bool m_move_up = false; 
    private bool m_just_switched = false;
    private bool m_Grounded;
    private bool m_incline;
    private bool m_platform_switched = false;
    private Animator m_anim;
    private Rigidbody2D m_rb2d;
    
    private void Awake(){

        if (this.gameObject.name == "enemy1(Clone)"){
            m_anim = gameObject.transform.Find("Collider").GetComponent<Animator>();
        }

        m_rb2d = this.GetComponent<Rigidbody2D>();
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
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

    private void RotateEnemy (RaycastHit2D hit){
       float temp = Vector3.Dot(Vector3.up,hit.normal);
       if(temp >0.7 && temp < 1.0){
           m_rb2d.MoveRotation(30f);
       }
       
    }
        
    //used for dealing with rigidbodies; called every physics step
    void FixedUpdate (){

//        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        m_Grounded = false;
        m_incline = false;

        Transform m_GroundCheck = transform.Find("GroundCheck").transform;
       
        Collider2D[] colliders = ((Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround)) ?? (new Collider2D[1]));

        if (colliders != null){
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    Debug.Log("set grounded to be true.");
                    m_Grounded = true;
            }
        }

        Transform FrontCheck = GameObject.Find("frontCheck").transform;

        Collider2D[] colliders2 = ((Physics2D.OverlapCircleAll(FrontCheck.position, k_GroundedRadius, m_WhatIsGround)) ?? (new Collider2D[1]));

        if (colliders2 != null){
            for (int i = 0; i < colliders2.Length; i++)
            {
                if (colliders2[i].gameObject.tag == "Incline")
                    Debug.Log("set incline to be true.");
                    m_incline = true;
            }
        }

//        Transform FrontCheck = GameObject.Find("frontCheck").transform;
//        RaycastHit2D hit = Physics2D.Raycast(FrontCheck.position,transform.forward);
//
//        Debug.DrawRay(FrontCheck.position, transform.forward, Color.black, 5.0f);
//
//        if(hit != null){
//            if (hit.transform.tag == "Incline"){
//                Debug.Log("set incline to be true");
//                m_incline = true;
////                transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
//            }
//        }
      

        if (m_can_move == true){
            if (this.name == "enemy1" || this.name == "enemy1(Clone)"){
                 //need to fix this physics; enemy going up a slope
                if (!m_Grounded){
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -9.8f);

                }else if (m_incline && m_Grounded){
                    
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(-2.5f, 0);
//                    RotateEnemy(hit);
                    m_rb2d.MoveRotation(45f);
                    
                }

                else if (!m_incline && m_Grounded ){
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 0f);   
                }
//
                m_anim.SetFloat("x_velocity", Mathf.Abs(this.GetComponent<Rigidbody2D>().velocity.x));
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
