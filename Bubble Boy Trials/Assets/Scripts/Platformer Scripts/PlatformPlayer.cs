using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets._2D;

public class PlatformPlayer : MonoBehaviour {

    public float m_repeat_damage_period= 2f; // how frequently the player can be damaged.
    public float m_health = 100f; // the player's m_health
    public AudioClip[] m_ouch_clips;               // Array of clips to play when the player is damaged.
    public float m_hurt_force = 300f;               // The force with which the player is pushed when hurt.
    public float m_damage_amount = 10f;            // The amount of damage to take when enemies touch the player
    public float hit_height = 3f; //height at which player will hit the enemy's head 
    public bool is_dead = false;

    private int m_lives = 3; //player's remaining lives
    private int m_score = 0; //player's current score
    private int m_num_combos = 0; // player's current number of combos
    private bool m_touched_head = false;
    private float m_last_hit_time; // the time at which the player was last hit.
    private float m_score_penalty = .30f; // how much the player's score is reduced after dying
    private EnemySpawner m_spawner;
    private SpriteRenderer m_health_bar;           // Reference to the sprite renderer of the m_health bar.
    private Vector3 m_health_scale;                // The local scale of the m_health bar initially (with full m_health).
    private Platformer2DUserControl m_player_control;        // Reference to the PlayerControl script.
    private Animator m_anim;                      // Reference to the animator on the player
    private GameObject health_bar;

    public Text score_text; 

    void Awake (){
        // Setting up references.
        m_player_control = GetComponent<Platformer2DUserControl>();

        health_bar = GameObject.Instantiate(Resources.Load("HealthBar"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        health_bar.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
        health_bar.GetComponent<Slider>().value = m_health;

        m_spawner = Camera.main.GetComponent<EnemySpawner>(); // need to set this back to Camera.current for scene integration
    }

    private void RespawnPlayer(){
        GetComponent<Platformer2DUserControl>().enabled = true;
        GetComponentInChildren<Weapon>().enabled = true;
        m_health = 100;
        m_score -= (int) (m_score * m_score_penalty);
        Vector2 temp = this.gameObject.transform.position;
        temp.x = 5.3f;
        temp.y = 20.0f;
        this.gameObject.transform.position = temp;
    }

    //increases score by the increment number
    public void GainScore (int increment){
        m_num_combos++;
        m_score += increment * m_num_combos;
    }

	// Update is called once per frame
	void Update () {
        score_text.text = "Score: " + m_score;
        if (this.gameObject.transform.position.y <= 5 
            || m_lives <= 0){
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

    void FixedUpdate(){
        Vector2 cast_origin = GameObject.Find("CastOrigin").transform.position; 
        Vector2 down_dir = transform.TransformDirection(Vector2.down);
        Vector2 size = new Vector2(1.564927f, 0.5199999f);
        float angle = 180f;

        RaycastHit2D[] hit = Physics2D.RaycastAll(cast_origin, down_dir, hit_height, 1 << 13);

        if (hit != null){
            foreach (RaycastHit2D head_hit in hit){
                Debug.Log(head_hit.transform.tag);
                if(head_hit.transform.tag == "Enemy"){
                    this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1500));
                    GameObject parent_enemy = head_hit.transform.gameObject;
                    m_spawner.RemoveFromDict(parent_enemy.name, parent_enemy.transform.position.x);
                    Destroy(parent_enemy);
                    GainScore(10);
                }   
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D coll){      
        if (coll.gameObject.name == "enemy1(Clone)"){
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
    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.name == "enemy2(Clone)"){
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
    }

    void TakeDamage (Transform enemy)
    {
        m_player_control.m_Jump = false;

        Vector3 displacement = (transform.position - enemy.position);
        Vector3 new_displacement = new Vector3 (300 * displacement.x, 10* displacement.y);
        // Create a vector that's from the enemy to the player with an upwards boost.
        Vector3 hurt_vector = displacement + Vector3.up;

        // Add a force to the player in the direction of the vector and multiply by the m_hurt_force.
        GetComponent<Rigidbody2D>().AddForce(hurt_vector * m_hurt_force);

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
