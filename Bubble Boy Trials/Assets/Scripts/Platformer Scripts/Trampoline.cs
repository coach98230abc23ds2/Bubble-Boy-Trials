using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Trampoline : MonoBehaviour {

    private bool m_touched = false;
    private bool m_is_rested = true;
    private bool m_is_accelerating = false;
    private float jump_force;
    private float player_y_velocity;
    private Rigidbody2D player_rg2d;
    private GameObject player;
   
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	    jump_force = player.GetComponent<PlatformerCharacter2D>().m_JumpForce; 
        player_rg2d = player.GetComponent<Rigidbody2D>();
        player_y_velocity = player_rg2d.velocity.y;
	}
	
	// Update is called once per frame
	void Update () {

	    if (player_y_velocity > 0f) 
        {
            m_is_rested = false;
            m_is_accelerating = true;
        }
        else if (player_y_velocity < 0f)
        {
            m_is_accelerating = false;
            m_is_rested = false;
        }
        else
        {
            m_is_rested = true;
            m_is_accelerating = false;
        }

        if (player_rg2d.position.y > 50.2f)
        {
//            player_rg2d.velocity.Normalize();
            player_rg2d.velocity = Vector3.zero;
        }
	}

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {   
           coll.gameObject.GetComponent<AudioSource>().Play();
//           m_touched = false;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {   

            if (m_is_rested)
            {
                player_rg2d.AddForce(new Vector2(0, .4f * jump_force));
            }
            if (m_is_accelerating)
            {
                player_rg2d.AddForce(new Vector2(0, .6f * jump_force));
            }
            else
            {
                player_rg2d.AddForce(new Vector2(0, .5f * jump_force));
            }

            this.gameObject.GetComponent<AudioSource>().Play();
//            m_touched = true;
        }
    }
}
