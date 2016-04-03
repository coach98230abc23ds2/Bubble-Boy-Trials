using UnityEngine;
using System.Collections;

public class PlatformEnemy : MonoBehaviour {

    private EnemySpawner spawner; 
    private float m_x_pos;
    public bool m_enemy_alive; 

    // Use this for initialization
    void Start () {
        spawner = Camera.main.GetComponent<EnemySpawner>();
        m_x_pos = this.gameObject.transform.position.x;
        m_enemy_alive = true;
    }

    // Update is called once per frame
    void Update () {
        float y_pos = this.gameObject.transform.position.y;
        if (y_pos <= 0){
            spawner.RemoveFromDict(this.gameObject.name, m_x_pos); 
            m_enemy_alive = false;
            Destroy(this.gameObject);
        }
    }
}

