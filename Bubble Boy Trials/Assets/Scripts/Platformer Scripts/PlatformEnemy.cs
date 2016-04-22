using UnityEngine;
using System.Collections;

public class PlatformEnemy : MonoBehaviour {

    private EnemySpawner spawner; 
    private GameObject hidden_obj;
    private float m_x_pos;
    private Animator m_anim;
    private PlatformPlayer player;

    // Use this for initialization
    void Awake () 
    {
        spawner = Camera.main.GetComponent<EnemySpawner>();
        m_x_pos = this.gameObject.transform.position.x;
        m_anim = this.gameObject.GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<PlatformPlayer>();
    }

    void Start()
    {
        hidden_obj = GameObject.Find("Hidden");
    }

    void Update () 
    {
        float y_pos = this.gameObject.transform.position.y;
        if (y_pos <= 0)
        {
            spawner.RemoveFromDict(this.gameObject.name, m_x_pos); 
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {   
        if (coll.gameObject.tag == "Death")
        {
            player.collide = false;
            GameObject parent_enemy = this.transform.gameObject;
            spawner.RemoveFromDict(parent_enemy.name, parent_enemy.transform.position.x);
            player.HurtEnemy(this.transform.gameObject, 0);

        }
    }
}

