using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
		Vector2 m_transform_position; 

		void Awake ()
		{
				GameObject.DontDestroyOnLoad (this);
		}

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		public void Move (Vector2 move_distance)
		{
				/* This line has a Vector2 & Vector3 conflict */
				m_transform_position  = (Vector2) transform.position + move_distance;

				// keeps the camera centered on the elevator
				Camera.main.transform.position = m_transform_position;
		}
}
