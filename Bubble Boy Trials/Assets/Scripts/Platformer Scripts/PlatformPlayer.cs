using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets._2D;

public class PlatformPlayer : MonoBehaviour {

    public float m_repeat_damage_period= 2f; // how frequently the player can be damaged.
    public float m_health = 100f; // the player's m_health
    public AudioClip[] m_ouch_clips;               // Array of clips to play when the player is damaged.
    public float m_hurt_force = 1000f;               // The force with which the player is pushed when hurt.
    public float m_damage_amount = 10f;            // The amount of damage to take when enemies touch the player
    public float hit_height = 3.0f; //height at which player will hit the enemy's head 
    public bool is_dead = false;
    public Coin coin;
    public Text score_text; 
    public bool collided = false;
    public GameObject health_bar;
    public AnimationClip enemy1_hit;
    public AnimationClip enemy2_hit;

    private int m_lives = 3; //player's remaining lives
    private int m_score = 0; //player's current score
    private int m_num_combos = 0; // player's current number of combos
    private bool m_touched_head = false;
    private bool m_touched_door = false;
    private bool health_bar_exists = false;
    private float m_last_hit_time; // the time at which the player was last hit.
    private float m_score_penalty = .50f; // decimal percentage the player's score is reduced after dying
    private EnemySpawner m_spawner;
    private SpriteRenderer m_health_bar;           // Reference to the sprite renderer of the m_health bar.
    private Vector3 m_health_scale;                // The local scale of the m_health bar initially (with full m_health).
    private Platformer2DUserControl m_player_control;        // Reference to the PlayerControl script.
    private Animator m_door_anim;                      // Reference to the animator on the door
    private Animator m_player_anim;                     // Reference to the animator on the player
    private Door m_door;
    private Animator m_emy_anim;
    private Platformer2DUserControl player_control;
    private Weapon weapon;

   

    void Awake ()
    {
        // Setting up references.
        m_player_control = GetComponent<Platformer2DUserControl>();
        m_spawner = Camera.main.GetComponent<EnemySpawner>(); // need to set this back to Camera.current for scene integration
        m_player_anim = this.gameObject.GetComponent<Animator>();
        m_door = GameObject.Find("BossDoor").GetComponent<Door>();
        m_door_anim = GameObject.Find("BossDoor").GetComponent<Animator>();
        player_control = this.gameObject.GetComponent<Platformer2DUserControl>();
        weapon = this.gameObject.GetComponent<Weapon>();
    }

    void Start()
    {       
          if (!health_bar_exists){
            health_bar = GameObject.Instantiate(Resources.Load("HealthBar"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            health_bar.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
            health_bar.GetComponent<Slider>().value = m_health;
            health_bar_exists = true;
          }
    }

    private void RespawnPlayer()
    {
        GetComponent<Platformer2DUserControl>().enabled = true;
        GetComponentInChildren<Weapon>().enabled = true;
        m_health = 100f;
        health_bar.GetComponent<Slider>().value = m_health;
        m_score -= (int) (m_score * m_score_penalty);
        Vector2 temp = this.gameObject.transform.position;
        temp.x = 5.3f;
        temp.y = 20.0f;
        this.gameObject.transform.position = temp;
    }

    //increases score by the increment number
    public void GainScore (int increment)
    {
//        m_num_combos++;-
        m_score += increment /* * m_num_combos*/;
    }

	// Update is called once per frame
	void Update () 
    {
        score_text.text = "Score: " + m_score;
        if (this.gameObject.transform.position.y <= 5 
            || m_lives <= 0)
        {
            RespawnPlayer();
        }
        // canvas is null when in platformer mode
        GameObject canvas = GameObject.Find("Canvas");
        if (health_bar != null && canvas != null)
        {
            // centers the health bar above the player
            health_bar.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
            RectTransform CanvasRect = canvas.GetComponent<RectTransform>();
            Vector2 ViewportPos = Camera.main.WorldToViewportPoint(transform.position);

            Vector2 ScreenPos = new Vector2(
                                    (ViewportPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f),
                                    (ViewportPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)
                                );

            health_bar.GetComponent<RectTransform>().anchoredPosition = ScreenPos + new Vector2(0, 50);
        }
	}

    public void HurtEnemy(GameObject curr_enemy, int score_increase)
    {
        transform.Rotate(Vector2.left);

        m_emy_anim = curr_enemy.GetComponentInChildren<Animator>();
        EnemyMovement movement = curr_enemy.GetComponent<EnemyMovement>();
        curr_enemy.transform.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezePositionX |
                                                                  RigidbodyConstraints2D.FreezePositionY) ;
        movement.m_can_move = false;
        m_emy_anim.SetTrigger("Hit");
//        StartCoroutine(WaitToDestroy(curr_enemy, score_increase));
        AnimationClip enemy_hit;
        float time_to_wait;
        if (curr_enemy.name == "enemy1(Clone)")
        {
            enemy_hit = enemy1_hit;
            time_to_wait = enemy_hit.length/4;
        }
        else
        {
            enemy_hit = enemy2_hit;
            time_to_wait = enemy_hit.length;
        }

        Destroy(curr_enemy, time_to_wait);
        GainScore(score_increase);
        collided = true;
    }

//    IEnumerator WaitToDestroy(GameObject curr_enemy, int score_increase)
//    {   
//        AnimationClip enemy_hit;
//        float time_to_wait;
//        if (curr_enemy.name == "enemy1(Clone)")
//        {
//            enemy_hit = enemy1_hit;
//            time_to_wait = enemy_hit.length/4;
//        }
//        else
//        {
//            enemy_hit = enemy2_hit;
//            time_to_wait = enemy_hit.length;
//        }
//
//        yield return new WaitForSeconds(time_to_wait);
//
//        Destroy(curr_enemy);
//        GainScore(score_increase);
//        collide = true;
//
//    }

    void FixedUpdate()
    {
        Vector2 cast_origin = GameObject.Find("CastOrigin").transform.position; 
        Vector2 down_dir = transform.TransformDirection(Vector2.down);
 
        RaycastHit2D[] hit = Physics2D.RaycastAll(cast_origin, down_dir, hit_height, 1 << 13);

        if (!m_touched_head)
        {
            if (hit != null)
            {
                foreach (RaycastHit2D collider_hit in hit)
                {
                    if(collider_hit.transform.tag == "Enemy")
                    {
                        m_touched_head = true;
                        collided = true;
                        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,500));
                        GameObject parent_enemy = collider_hit.transform.root.gameObject;
                        m_spawner.RemoveFromDict(parent_enemy.name, parent_enemy.transform.position.x);
                        HurtEnemy(parent_enemy, 10);
                    }   
                }
                
            }
        }

        Vector2 right_dir = transform.TransformDirection(Vector2.right);
        RaycastHit2D[] hit2 = Physics2D.RaycastAll(cast_origin, right_dir, 20f, 1 << 14);

        if (!m_touched_door)
        {
            //detects when player is near the exit boss door.
            if (hit2 != null)
            {
                foreach (RaycastHit2D collider_hit in hit2){
                    if(collider_hit.transform.gameObject.name == "BossDoor")
                    {   
                       
                        this.gameObject.transform.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezePositionX |
                                                                  RigidbodyConstraints2D.FreezePositionY) ;
                        player_control.m_can_move = false;
                        weapon.can_attack = false;
                        m_touched_door = true;
                        m_door_anim.SetBool("is_active", true);
                        StartCoroutine(m_door.WaitToSwitch(collider_hit.transform.position));
                    }
                }
            }
        }
    }


    void OnCollisionEnter2D(Collision2D coll)
    { 
        if (!collided){     
            if (coll.gameObject.name == "enemy1(Clone)" || coll.gameObject.tag == "Death")
            {
                if (Time.time > m_last_hit_time + m_repeat_damage_period) 
                {
                    if(m_health > 0f)
                    {
                        TakeDamage(coll.transform); 
                        m_last_hit_time = Time.time; 
                    }
                    else
                    {
                        // Find all of the colliders on the gameobject and set them all to be triggers.
                        Collider2D[] cols = GetComponents<Collider2D>();
                        foreach(Collider2D c in cols)
                        {
                            c.isTrigger = true;
                        }

                        // Move all sprite parts of the player to the front
                        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
                        foreach(SpriteRenderer s in spr)
                        {
                            s.sortingLayerName = "UI";
                        }

                        GetComponent<Platformer2DUserControl>().enabled = false;
                        GetComponentInChildren<Weapon>().enabled = false;
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name == "enemy2(Clone)")
        {
            // need to fix this; enemies go right through enemy2.
            if (Time.time > m_last_hit_time + m_repeat_damage_period) 
            {
                if(m_health > 0f)
                {
                    TakeDamage(coll.transform); 
                    m_last_hit_time = Time.time; 
                }
                else
                {
                    // Find all of the colliders on the gameobject and set them all to be triggers.
                    Collider2D[] cols = GetComponents<Collider2D>();
                    foreach(Collider2D c in cols)
                    {
                        c.isTrigger = true;
                    }

                    // Move all sprite parts of the player to the front
                    SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
                    foreach(SpriteRenderer s in spr)
                    {
                        s.sortingLayerName = "UI";
                    }

                    GetComponent<Platformer2DUserControl>().enabled = false;
                    GetComponentInChildren<Weapon>().enabled = false;
//                    m_anim.SetTrigger("Die");
                }
            }
        }
        if (coll.gameObject.tag == "Coin")
        {
            Destroy(coll.gameObject);
            coin.PlaySound(coll.transform.position);
            GainScore(40);
        }
    }

    void TakeDamage (Transform enemy)
    {   
        m_player_anim.SetTrigger("Defend");
        m_player_control.m_Jump = false;

        if (enemy.name == "enemy1(Clone)")
        {
            Vector2 displacement = (transform.position - enemy.position);
//            Debug.Log("Displacement: " + System.Convert.ToString(displacement));
            Vector2 new_displacement = new Vector2 (4000* displacement.x, 5 *displacement.y);
            // Create a vector that's from the enemy to the player with an upwards boost.
            Vector2 hurt_vector = new_displacement + Vector2.up;

            // Add a force to the player in the direction of the vector and multiply by the m_hurt_force.
            GetComponent<Rigidbody2D>().AddForce(hurt_vector);
        }

        // Update what the m_health bar looks like.
        UpdateHealthBar(m_damage_amount);

        // Play a random clip of the player getting hurt.
//        int i = Random.Range (0, m_ouch_clips.Length);
//        AudioSource.PlayClipAtPoint(m_ouch_clips[i], transform.position);
    }

    public void UpdateHealthBar (float damage)
    {
        m_health -= damage;

        if (health_bar != null)
        {
            health_bar.GetComponent<Slider>().value = m_health;
        }

        if (m_health <= 0)
        {
            Destroy(health_bar);
            is_dead = true;
        }
    }
}
