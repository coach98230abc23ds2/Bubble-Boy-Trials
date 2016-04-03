using UnityEngine;
using System.Collections;

public class PlatformEnemy : MonoBehaviour {


    public GameObject current_enemy;
    private EnemySpawner spawner; 
    private float m_x_pos;

    // Use this for initialization
    void Start () {
        spawner = Camera.main.GetComponent<EnemySpawner>();
        m_x_pos = current_enemy.transform.position.x;
    }

    // Update is called once per frame
    void Update () {
        float y_pos = current_enemy.transform.position.y;
        if (y_pos <= 0){
            spawner.RemoveFromDict(current_enemy.name, m_x_pos); 
            Destroy(current_enemy);
        }
    }
}

