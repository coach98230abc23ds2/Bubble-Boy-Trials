using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    private int m_move_state = 0;
    private int m_direction = -1;
    private float m_speed = 2.0f;
    private float m_timer = 0.0f;
    private float curr_y_pos;
    private bool m_can_move = false;
    private bool m_move_up = false; 
    private bool m_just_switched = false;
  

    //zeros out speed when minion dies
//    public void ZeroOutSpeed (){
//        m_speed = 0.0f;
//        Debug.Log(System.Convert.ToString(m_speed));
//    }
//

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
        if (m_can_move == true){
            if (this.name == "enemy1" || this.name == "enemy1(Clone)"){
                if (curr_y_pos <= 21.0f){
                    this.GetComponent<Rigidbody2D>().gravityScale = 4.9f;
                }else{
                    this.GetComponent<Rigidbody2D>().gravityScale = 9.8f;
//                    Debug.Log ("got to enemy1");
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(-m_speed, 0);
                }
            }
            if (Time.time > 2.0f){
                m_speed = 5.0f;
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
