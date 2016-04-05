using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

    private PlatformPlayer m_player;
    private EnemySpawner spawner;

    //handles collision for when ammo collides with enemies
    void OnTriggerEnter2D(Collider2D other){
        float x_pos = other.gameObject.transform.position.x;
        //destroys ammo and minion; increases player's score
        if (other.tag == "Enemy"){
                spawner.RemoveFromDict(other.gameObject.name, x_pos); 
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                m_player.GainScore(10);
        }
        //destroys ammo and boss; increases player's score
        if(other.tag == "Boss"){
                spawner.RemoveFromDict(other.gameObject.name, x_pos); 
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                m_player.GainScore(50);
        }   
    }

    void Start(){
        spawner = Camera.main.GetComponent<EnemySpawner>();
        GameObject player = GameObject.Find("Player");
        m_player = player.GetComponent<PlatformPlayer>();
    }

    void Update(){
        //destroys ammo when it goes out of the camera's view
        float viewport_x_pos = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (viewport_x_pos > 1 || viewport_x_pos < -1){
            Destroy(this.gameObject);
        }
    }
}
