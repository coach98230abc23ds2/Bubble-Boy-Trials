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
    public bool m_can_move = false;
    private bool m_move_up = false; 
    private bool m_just_switched = false;
    private bool m_Grounded;
    private bool m_incline;
    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    private Transform m_FrontCheck;
    private Animator m_anim;
    private Rigidbody2D m_rb2d;
    
    private void Awake(){
        m_GroundCheck = transform.Find("GroundCheck");
        m_FrontCheck = transform.Find("frontCheck");
        m_anim = gameObject.GetComponent<Animator>();
        m_rb2d = this.GetComponent<Rigidbody2D>();
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

       
        Collider2D[] colliders = ((Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround)) ?? (new Collider2D[1]));

        if (colliders != null){
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    Debug.Log("set grounded to be true.");
                    m_Grounded = true;
            }
        }

//        }catch(Exception e){
//            Debug.Log("no collider below enemy");
//        }

//      
        Transform FrontCheck = GameObject.Find("frontCheck").transform;
        RaycastHit2D hit = Physics2D.Raycast(FrontCheck.position,transform.forward);

        Debug.DrawRay(FrontCheck.position, transform.forward, Color.black, 5.0f);

        if(hit != null){
            if (hit.transform.tag == "Incline"){
                Debug.Log("set incline to be true");
                m_incline = true;
                Debug.Log("rotated 45 degrees");
               
                m_rb2d.MoveRotation(45f);
            }
        }
//        

        if (m_can_move == true){
            if (this.name == "enemy1" || this.name == "enemy1(Clone)"){
                 //need to fix this physics; enemy going up a slope
                if (!m_Grounded){
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -9.8f);

                }else if (m_incline && m_Grounded){

                    this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

//                    if(Vector2.Dot(Vector3.up,hit.normal)> 0.7){
//                        Debug.Log("rotated 45 degrees");
//                        this.gameObject.transform.Rotate(new Vector3 (0, 0, 45f));
//                    }

//                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-20f, 0f));
                }

                else if (!m_incline && m_Grounded ){
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 0);
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
