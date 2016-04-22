using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

    private PlatformPlayer m_player;
    private EnemySpawner spawner;
    private Animator m_anim;

    void GotHurt(GameObject enemy)
    {
        enemy.GetComponent<Rigidbody2D>().MoveRotation(45f);
        Animator m_anim = enemy.transform.Find("Collider").GetComponent<Animator>();
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        enemy.transform.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezePositionX |
                                                                  RigidbodyConstraints2D.FreezePositionY) ;
        movement.m_can_move = false;
        m_anim.SetTrigger("Hit");
        Destroy(enemy, 2.0f);
        StartCoroutine(WaitToDestroy(enemy));
    }

    IEnumerator WaitToDestroy(GameObject enemy)
    {
        yield return new WaitForSeconds(2f);
        m_player.GainScore(10);
    }

    //handles collision for when ammo collides with enemies of type enemy1
    void OnCollisionEnter2D(Collision2D coll)
    {
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<CircleCollider2D>(), m_player.gameObject.GetComponent<BoxCollider2D>());
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<CircleCollider2D>(), m_player.gameObject.GetComponent<CircleCollider2D>());
        float x_pos = coll.gameObject.transform.position.x;
        Debug.Log(coll.gameObject.tag);
        //destroys ammo and minion; increases player's score
        if (coll.gameObject.tag == "Enemy")
        {
                spawner.RemoveFromDict(coll.gameObject.name, x_pos);
                Destroy(this.gameObject);
                GotHurt(coll.gameObject);
        }
 
    }


    //handles collision for when ammo collides with enemies of type enemy2
    void OnTriggerEnter2D(Collider2D coll)
    {
        float x_pos = coll.gameObject.transform.position.x;

        if (coll.gameObject.tag == "Enemy")
        {
            spawner.RemoveFromDict(coll.gameObject.name, x_pos); 
            Destroy(coll.gameObject);
            Destroy(this.gameObject);
            m_player.GainScore(20);
        }
    }
    
    void Awake()
    {
        spawner = Camera.current.GetComponent<EnemySpawner>();
        GameObject player = GameObject.Find("Player");
        m_player = player.GetComponent<PlatformPlayer>(); 
    }

    void Update()
    {
        //destroys ammo when it goes out of the camera's view
        float viewport_x_pos = Camera.current.WorldToViewportPoint(this.transform.position).x;
        if (viewport_x_pos > 1 || viewport_x_pos < -1)
        {
            Destroy(this.gameObject);
        }
    }
}
