using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MazeSystem : MonoBehaviour
{
    private static float EPSILON = 0.2f;
    private static float SPEED = 1.0f;

    public Canvas MazeUI;
    public Button LeftButton;
    public Button UpButton;
    public Button RightButton;

    Vector2 m_target_position;
    MazeNode m_current_node;
    Vector2 m_player_current_position;
    Elevator m_elevator;

    bool m_level_started;
    bool m_path_chosen;

    void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        m_current_node = GameObject.FindGameObjectWithTag("Root Node").GetComponent<MazeNode>();
        m_target_position = m_current_node.transform.position;
        m_elevator = GameObject.FindGameObjectWithTag("Elevator").GetComponent<Elevator>();
        m_elevator.transform.position = new Vector2(0,0);
        m_level_started = false;
        m_path_chosen = true;
        MazeUI.gameObject.SetActive(false);
    }
	
    // Update is called once per frame
    void Update()
    {
        // we just got back from a level
        if (!m_level_started && m_path_chosen)
        {
            Vector2 movement_difference = m_target_position - (Vector2)m_elevator.transform.position;
            if (movement_difference.magnitude < EPSILON)
            {
                m_elevator.transform.position = m_target_position;
                m_level_started = true;
                StartLevel();
            }
            else
            {
                movement_difference.Normalize();
                m_elevator.Move(movement_difference * SPEED * Time.deltaTime);
            }
        }
    }

    void StartLevel()
    {
        SceneManager.LoadScene(m_current_node.Scene_Id, LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName(m_current_node.Scene_Id);
        SceneManager.SetActiveScene(scene);
        m_level_started = true;
        m_path_chosen = false;
    }

    public void LevelCompleted()
    {
        m_level_started = false;
        MazeUI.gameObject.SetActive(true);
        LeftButton.gameObject.SetActive(m_current_node.Left_Node != null);
        UpButton.gameObject.SetActive(m_current_node.Up_Node != null);
        RightButton.gameObject.SetActive(m_current_node.Right_Node != null);
    }

    public void MoveLeft()
    {
        m_current_node = m_current_node.Left_Node;
        m_target_position = m_current_node.transform.position;
        MazeUI.gameObject.SetActive(false);
        m_path_chosen = true;
    }

    public void MoveRight()
    {
        m_current_node = m_current_node.Right_Node;
        m_target_position = m_current_node.transform.position;
        MazeUI.gameObject.SetActive(false);
        m_path_chosen = true;
    }

    public void MoveUp()
    {
        m_current_node = m_current_node.Up_Node;
        m_target_position = m_current_node.transform.position;
        MazeUI.gameObject.SetActive(false);
        m_path_chosen = true;
    }
}
