using UnityEngine;
using System.Collections;

public class PlatformEnemy : MonoBehaviour {
	
    public EnemySpawner spawner; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float y_pos = this.transform.position.y;
        if (y_pos <= -10){
            spawner.RemoveFromDict(this.name, y_pos); 
        }

	}
}
