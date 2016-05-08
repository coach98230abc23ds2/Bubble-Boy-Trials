using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

    private PlatformPlayer m_player;
    private EnemySpawner spawner;
    private Animator ammo_anim;
    private AudioSource[] ammo_audio;
    private float alive_time = 2f; //time allowed for bubble to be alive
    private bool ammo_hit = false;
    private bool ammo_alive = true;

    public AnimationClip enemy1_hit;
    public AnimationClip enemy2_hit;
    public AnimationClip bubble_burst;

    IEnumerator BurstBubble (float time_to_wait)
    {
        ammo_anim.SetTrigger("Burst");
        ammo_audio[1].Play();
        yield return new WaitForSeconds(bubble_burst.length);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        ammo_alive = false;
        StartCoroutine(DelayHit(time_to_wait));
    }

    IEnumerator DelayHit (float time_to_wait)
    {   
        yield return new WaitForSeconds(time_to_wait);
        m_player.GainScore(10);
        m_player.SetCollide(false);
        Destroy(this.gameObject);
    }

    void GotHurt(GameObject enemy, float time_to_wait)
    {
        ammo_hit = true;
        m_player.SetCollide(true);
        enemy.GetComponentInChildren<Rigidbody2D>().MoveRotation(45f);
        Animator m_anim = enemy.GetComponentInChildren<Animator>();
        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        enemy.GetComponentInChildren<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezePositionX |
                                                                  RigidbodyConstraints2D.FreezePositionY) ;
        movement.m_can_move = false;
        m_anim.SetTrigger("Hit");
      
        Destroy(enemy, time_to_wait);
        StartCoroutine(BurstBubble(time_to_wait));
    }

    //handles collision for when ammo collides with enemies of type enemy1
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (!ammo_hit)
        {
            float x_pos = coll.transform.root.gameObject.transform.position.x;
            //destroys ammo and minion; increases player's score
            GameObject current_obj = coll.transform.root.gameObject;
            Debug.Log(current_obj.name);
            if (current_obj.tag == "Enemy")
            {
                spawner.RemoveFromDict(current_obj.name, x_pos);

                AnimationClip enemy_hit;
                float time_to_wait = 0f;
                if (current_obj.name == "enemy1(Clone)")
                {
                    enemy_hit = enemy1_hit;
                    time_to_wait = enemy_hit.length/6.4f;
                }
                else if (current_obj.name == "enemy2(Clone)")
                {
                    enemy_hit = enemy2_hit;
                    time_to_wait = enemy_hit.length;
                }
                GotHurt(current_obj, time_to_wait);
            }
        }
 
    }

    //handles collision for when ammo collides with enemies of type enemy2
    void OnTriggerEnter2D(Collider2D coll)
    {   
        if (!ammo_hit)
        {
            float x_pos = coll.transform.root.gameObject.transform.position.x;
            GameObject parent_object = coll.transform.root.gameObject;
            Debug.Log(parent_object.name);
            if (parent_object.tag == "Enemy")
            {
                spawner.RemoveFromDict(parent_object.name, x_pos);
                 
                AnimationClip enemy_hit;
                float time_to_wait = 0f;
                if (parent_object.name == "enemy1(Clone)")
                {
                    enemy_hit = enemy1_hit;
                    time_to_wait = enemy_hit.length/4;
                }
                else if (parent_object.name == "enemy2(Clone)")
                {
                    enemy_hit = enemy2_hit;
                    time_to_wait = enemy_hit.length;
                }
                GotHurt(parent_object, time_to_wait);
            }
        }
    }

    
    void Awake()
    {
        spawner = Camera.main.GetComponent<EnemySpawner>();
        GameObject player = GameObject.Find("Player");
        m_player = player.GetComponent<PlatformPlayer>(); 
        ammo_anim = this.gameObject.GetComponent<Animator>();
        ammo_audio = this.gameObject.GetComponents<AudioSource>();
        StartCoroutine(DestroyBubble());
    }

    void Update()
    {  
        //destroys ammo when it goes out of the camera's view
        float viewport_x_pos = Camera.main.WorldToViewportPoint(this.transform.position).x;
        if (viewport_x_pos > 1 || viewport_x_pos < -1)
        {
            StartCoroutine(DestroyBubble());  
        }
    }

    IEnumerator DestroyBubble ()
    {   
        yield return new WaitForSeconds(alive_time);
        ammo_anim.SetTrigger("Burst");
        Debug.Log("destroyed bubble");
        if (!ammo_hit)
        {
            ammo_audio[1].Play();
        }
        yield return new WaitForSeconds(bubble_burst.length);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Destroy(this.gameObject);
    }
   
}
