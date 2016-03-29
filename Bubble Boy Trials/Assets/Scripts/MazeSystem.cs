using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MazeSystem : MonoBehaviour
{
		private static float EPSILON = 1.0f;
		private static float SPEED = 5.0f;

		Vector2 m_target_position;
		MazeNode m_current_node;
		Vector2 m_player_current_position;
		Elevator m_elevator;

		bool m_level_started;

		void Awake ()
		{
				GameObject.DontDestroyOnLoad (this);
		}

		// Use this for initialization
		void Start ()
		{
				m_current_node = GameObject.FindGameObjectWithTag ("Root Node").GetComponent<MazeNode> ();
				m_target_position = m_current_node.transform.position;
				m_elevator = GameObject.FindGameObjectWithTag ("Elevator").GetComponent<Elevator> ();
				m_elevator.transform.position = m_target_position;
				m_level_started = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				// we just got back from a level
				if (m_level_started) {
						m_level_started = false;
				}
				Vector2 movement_difference = m_target_position - m_elevator.transform.position;
				if (movement_difference.magnitude < EPSILON) {
						m_target_position = m_elevator.transform.position;
						m_level_started = true;
						StartLevel ();
				} else {
						movement_difference.Normalize ();
						m_elevator.Move (movement_difference * SPEED);
				}
		}

		void StartLevel ()
		{
				SceneManager.LoadScene (m_current_node.Scene_Id);

				// get the latest added scene!
				Scene scene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
				SceneManager.SetActiveScene (scene);
		}

		void MoveLeft ()
		{
				m_current_node = m_current_node.Left_Node;
				m_target_position = m_current_node.transform.position;
		}

		void MoveRight ()
		{
				m_current_node = m_current_node.Right_Node;
				m_target_position = m_current_node.transform.position;
		}

		void MoveUp ()
		{
				m_current_node = m_current_node.Up_Node;
				m_target_position = m_current_node.transform.position;
		}
}
