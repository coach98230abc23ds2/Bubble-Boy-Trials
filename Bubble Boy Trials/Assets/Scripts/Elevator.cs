using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
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
				transform.position += move_distance;

				// keeps the camera centered on the elevator
				Camera.main.transform.position = transform.position;
		}
}
