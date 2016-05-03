using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MazeSystem : MonoBehaviour
{
    private static float EPSILON = 0.2f;
    private static float SPEED = 4.0f;

    public Canvas MazeUI;
    public Button LeftButton;
    public Button UpButton;
    public Button RightButton;
    public RuntimeAnimatorController boy_anim_control;
    public RuntimeAnimatorController girl_anim_control;
    public Sprite boy_sprite;
    public Sprite girl_sprite;

    Vector2 m_target_position;
    MazeNode m_current_node;
    MazeNode m_prev_node;
    Vector2 m_player_current_position;
    Elevator m_elevator;
    GameObject maze_player;
    CharacterChoice char_choice;

    bool m_level_started;
    bool m_path_chosen;
    bool m_retreating;

    int m_score;

    void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        m_current_node = GameObject.FindGameObjectWithTag("Root Node").GetComponent<MazeNode>();
        m_prev_node = m_current_node;
        m_target_position = m_current_node.transform.position;
        m_elevator = GameObject.FindGameObjectWithTag("PlayerElevator").GetComponent<Elevator>();
        m_level_started = false;
        m_path_chosen = true;
        m_retreating = false;
        MazeUI.gameObject.SetActive(false);
        maze_player = GameObject.Find("MazePlayer");
        char_choice = GameObject.Find("CharacterChoice").GetComponent<CharacterChoice>();
        SetSpriteAndAnimator();
    }

    void SetSpriteAndAnimator()
    {   
        Debug.Log(char_choice.GetCharacter());
        if (char_choice.GetCharacter() == "Girl")
        {
            maze_player.GetComponent<SpriteRenderer>().sprite = girl_sprite;
            maze_player.GetComponent<Animator>().runtimeAnimatorController = girl_anim_control;
        }
        else
        {
            maze_player.GetComponent<SpriteRenderer>().sprite = boy_sprite;
            maze_player.GetComponent<Animator>().runtimeAnimatorController = boy_anim_control;
        }
    }
	
    // Update is called once per frame
    void Update()
    {
        // we just got back from a level
        if (!m_level_started && m_path_chosen)
        {
            Vector2 movement_difference = m_target_position - (Vector2)m_elevator.transform.position + new Vector2(-0.1f, 1.0f);
            if (movement_difference.magnitude < EPSILON)
            {
                if (!m_retreating)
                {
                    m_elevator.transform.position = m_target_position + new Vector2(-0.1f, 1.0f);
                    m_level_started = true;
                    StartLevel();
                }
                else
                {
                    m_path_chosen = false;
                    m_retreating = false;
                    LeftButton.gameObject.SetActive(m_current_node.Left_Node != null);
                    UpButton.gameObject.SetActive(m_current_node.Up_Node != null);
                    RightButton.gameObject.SetActive(m_current_node.Right_Node != null);
                }
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
        // this is the last scene, so destroy the maze and let it take over
        if (m_current_node.Scene_Id == "PrimeScene")
        {
            SceneManager.LoadScene(m_current_node.Scene_Id, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(m_current_node.Scene_Id, LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName(m_current_node.Scene_Id);
            SceneManager.SetActiveScene(scene);
            m_level_started = true;
            m_path_chosen = false;
        }
    }

    public void LevelCompleted(bool success, int score=0)
    {
        m_level_started = false;
        MazeUI.gameObject.SetActive(true);
        if (success)
        {
            if (score > 0)
            {
                UpdateScore(score);
            }
            LeftButton.gameObject.SetActive(m_current_node.Left_Node != null);
            UpButton.gameObject.SetActive(m_current_node.Up_Node != null);
            RightButton.gameObject.SetActive(m_current_node.Right_Node != null);
        }
        else
        {
            m_current_node = m_prev_node;
            m_target_position = m_current_node.transform.position;
            m_path_chosen = true;
            m_retreating = true;
        }
    }

    public void UpdateScore(int score)
    {
        m_score = score;
        GameObject.Find("/Canvas/ScoreText").GetComponent<Text>().text = "Score: " + m_score;
    }

    public int GetScore()
    {
        return m_score;
    }

    public void MoveLeft()
    {
        m_prev_node = m_current_node;
        m_current_node = m_current_node.Left_Node;
        m_target_position = m_current_node.transform.position;
        MazeUI.gameObject.SetActive(false);
        m_path_chosen = true;
    }

    public void MoveRight()
    {
        m_prev_node = m_current_node;
        m_current_node = m_current_node.Right_Node;
        m_target_position = m_current_node.transform.position;
        MazeUI.gameObject.SetActive(false);
        m_path_chosen = true;
    }

    public void MoveUp()
    {
        m_prev_node = m_current_node;
        m_current_node = m_current_node.Up_Node;
        m_target_position = m_current_node.transform.position;
        MazeUI.gameObject.SetActive(false);
        m_path_chosen = true;
    }
}
