using UnityEngine;
using System.Collections;

public class PlatformEnemy : MonoBehaviour {

    private EnemySpawner spawner; 
    private GameObject hidden_obj;
    private float m_x_pos;

    // Use this for initialization
    void Start () {
        spawner = Camera.main.GetComponent<EnemySpawner>();
        m_x_pos = this.gameObject.transform.position.x;
        hidden_obj = GameObject.Find("Hidden");
        //hides 'hidden' object used for detecting if the player jumped on the enemy's head
        hidden_obj.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (hidden_obj != null){
            hidden_obj.GetComponent<SpriteRenderer>().enabled = false;
        }
        float y_pos = this.gameObject.transform.position.y;
        if (y_pos <= 0){
            spawner.RemoveFromDict(this.gameObject.name, m_x_pos); 
            Destroy(this.gameObject);
        }
    }

}

