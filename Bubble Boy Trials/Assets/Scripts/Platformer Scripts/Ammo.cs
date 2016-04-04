using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

    public PlatformPlayer m_player;
    public PlatformEnemy plat_enemy;

    //handles collision for when bullet collides with enemies
    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Enemy"){
//            plat_enemy = other.gameObject;
//            if(plat_enemy.m_enemy_alive){
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                plat_enemy.m_enemy_alive = false;
                m_player.GainScore(10);
//            }
        }
        if(other.GetComponent<Collider2D>().tag == "Boss"){
//            plat_enemy = other.gameObject;
//            if(plat_enemy.m_enemy_alive){
                Destroy(other.gameObject);
                Destroy(this.gameObject);
                plat_enemy.m_enemy_alive = false;
                m_player.GainScore(50);
//            }
        }   
    }
}
