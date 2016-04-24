using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour {

    private float m_scroll_speed;
    public float tile_size;

    private Vector3 start_position;

	// Use this for initialization
	void Start () 
    {
	    start_position = transform.position;
	}

	public void ScrollBG (float speed) 
    {
        m_scroll_speed = speed/10;
	    float newPosition = Mathf.Repeat(Time.time * m_scroll_speed, tile_size);
        transform.position = start_position + Vector3.right * newPosition;
//        start_position = transform.position;
	}
   
}
