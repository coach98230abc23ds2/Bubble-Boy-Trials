using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

  public int m_speed2 = 5;
  private float m_timer = 0.0f;
	
	void FixedUpdate()
  {	
  	GetComponent<Rigidbody2D>().AddForce(Vector3.left * m_speed2);
   //  // moves the game object 
   //  if (m_timer > 5.0f){
			// GetComponent<Rigidbody2D>().AddForce(Vector3.left * m_speed2);
   //  }
  }
}
