using UnityEngine;
using System.Collections;

public class PlatformEnemy : MonoBehaviour {
	
    private EnemySpawner spawner; 
    private float m_x_pos;

    // Use this for initialization
    void Start () {
        spawner = Camera.main.GetComponent<EnemySpawner>();
        m_x_pos = this.transform.position.x;
    }

    // Update is called once per frame
    void Update () {
        float y_pos = this.transform.position.y;
        if (y_pos <= 0){
            spawner.RemoveFromDict(this.name, m_x_pos); 
        }
    }
}
