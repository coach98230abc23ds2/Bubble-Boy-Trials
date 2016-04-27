using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

    private PlatformPlayer m_player;
    private EnemySpawner spawner;
    private Animator m_anim;

    public AnimationClip enemy1_hit;
    public AnimationClip enemy2_hit;

    void GotHurt(GameObject enemy)
    {
        if (!m_player.collided)
        {
            enemy.GetComponentInChildren<Rigidbody2D>().MoveRotation(45f);
            Animator m_anim = enemy.GetComponentInChildren<Animator>();
            EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
            enemy.GetComponentInChildren<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezePositionX |
                                                                      RigidbodyConstraints2D.FreezePositionY) ;
            movement.m_can_move = false;
            m_anim.SetTrigger("Hit");


            AnimationClip enemy_hit;
            float time_to_wait;
            if (enemy.name == "enemy1(Clone)")
            {
                enemy_hit = enemy1_hit;
                time_to_wait = enemy_hit.length/4;
            }
            else
            {
                enemy_hit = enemy2_hit;
                time_to_wait = enemy_hit.length;
            }


            Destroy(enemy, time_to_wait);
            m_player.GainScore(10);
        }

    }

    //handles collision for when ammo collides with enemies of type enemy1
    void OnCollisionEnter2D(Collision2D coll)
    {
        try
        {
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<CircleCollider2D>(), m_player.gameObject.GetComponent<BoxCollider2D>());
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<CircleCollider2D>(), m_player.gameObject.GetComponent<CircleCollider2D>());
        }
        catch
        {
            Debug.Log("Object is not a bubble.");
        }
        float x_pos = coll.transform.root.gameObject.transform.position.x;
        //destroys ammo and minion; increases player's score
        GameObject current_obj = coll.transform.root.gameObject;
        Debug.Log(current_obj.name);
        if (current_obj.tag == "Enemy")
        {
            spawner.RemoveFromDict(current_obj.name, x_pos);
            Destroy(this.gameObject);
            GotHurt(current_obj);
        }
 
    }


    //handles collision for when ammo collides with enemies of type enemy2
    void OnTriggerEnter2D(Collider2D coll)
    {
        float x_pos = coll.transform.root.gameObject.transform.position.x;
        GameObject parent_object = coll.transform.root.gameObject;
        Debug.Log(parent_object.name);
        if (parent_object.tag == "Enemy")
        {
            spawner.RemoveFromDict(parent_object.name, x_pos); 
            Destroy(this.gameObject);
            GotHurt(parent_object);
        }
    }
    
    void Awake()
    {
        spawner = Camera.main.GetComponent<EnemySpawner>();
        GameObject player = GameObject.Find("Player");
        m_player = player.GetComponent<PlatformPlayer>(); 
    }

    void Update()
    {
        //destroys ammo when it goes out of the camera's view
        float viewport_x_pos = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (viewport_x_pos > 1 || viewport_x_pos < -1)
        {
            Destroy(this.gameObject);
        }
    }
}
