using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

    private PlatformPlayer m_player;
    private EnemySpawner spawner;

    //handles collision for when ammo collides with enemies
    void OnCollisionEnter2D(Collision2D coll){
        float x_pos = coll.gameObject.transform.position.x;
        Debug.Log(coll.gameObject.tag);
        //destroys ammo and minion; increases player's score
        if (coll.gameObject.tag == "Enemy"){
                spawner.RemoveFromDict(coll.gameObject.name, x_pos); 
                Destroy(coll.gameObject);
                Destroy(this.gameObject);
                m_player.GainScore(10);
        }
        //destroys ammo and boss; increases player's score
        if(coll.gameObject.tag == "Boss"){
                spawner.RemoveFromDict(coll.gameObject.name, x_pos); 
                Destroy(coll.gameObject);
                Destroy(this.gameObject);
                m_player.GainScore(50);
        }   
    }

    void OnTriggerEnter2D(Collider2D coll){
        float x_pos = coll.gameObject.transform.position.x;

        if (coll.gameObject.tag == "Enemy"){
            spawner.RemoveFromDict(coll.gameObject.name, x_pos); 
            Destroy(coll.gameObject);
            Destroy(this.gameObject);
            m_player.GainScore(20);
        }
    }
    
    void Awake(){
        spawner = Camera.current.GetComponent<EnemySpawner>();
        GameObject player = GameObject.Find("Player");
        m_player = player.GetComponent<PlatformPlayer>();
    }

    void Update(){
        //destroys ammo when it goes out of the camera's view
        float viewport_x_pos = Camera.current.WorldToViewportPoint(this.transform.position).x;
        if (viewport_x_pos > 1 || viewport_x_pos < -1){
            Destroy(this.gameObject);
        }
    }
}
