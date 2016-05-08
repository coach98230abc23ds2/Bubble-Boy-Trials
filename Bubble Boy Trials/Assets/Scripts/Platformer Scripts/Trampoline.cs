using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Trampoline : MonoBehaviour {

    private bool m_touched = false;
    private bool m_is_falling = true;
    private float jump_force;
    private float player_y_velocity;
    private Rigidbody2D player_rg2d;
    private GameObject player;
   
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
	    jump_force = player.GetComponent<PlatformerCharacter2D>().m_JumpForce/10; 
        player_rg2d = player.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {

        player_y_velocity = player_rg2d.velocity.y;

        if (player_y_velocity < 0f)
        {
            m_is_falling = true;
        }
        else
        {
            m_is_falling = false;
        }

        if (player_rg2d.position.y > 54.4f)
        {
//            player_rg2d.velocity.Normalize();
            float temp_y_velocity = -1f;
            player_rg2d.velocity = new Vector2(0, temp_y_velocity);
        }
	}
    

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {   

            if (m_is_falling)
            {
                this.gameObject.GetComponent<AudioSource>().Play();
                player_rg2d.AddForce(new Vector2(0, .2f* jump_force * Mathf.Abs(player_y_velocity)));
            }

//            if (player_y_velocity < 0f)
//            {   
//                float applied_force = (Mathf.Abs(player_y_velocity) + jump_force) * multiplier;
//                player_rg2d.AddForce(new Vector2(0, )); 
//            }

//            m_touched = true;
        }
    }
}
