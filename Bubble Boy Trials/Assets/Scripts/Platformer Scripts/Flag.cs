using UnityEngine;
using System.Collections;

public class Flag : MonoBehaviour {
    public Sprite active_flag;

    private PlatformPlayer player;

	// Use this for initialization
	void Start () 
    {
	    player = GameObject.Find("Player").GetComponent<PlatformPlayer>();	
    }

    void OnTriggerEnter2D(Collider2D coll)
    {   
        Vector3 position = this.gameObject.transform.position;
        player.AddToDict(position.x, position.y);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = active_flag;
    }
}
