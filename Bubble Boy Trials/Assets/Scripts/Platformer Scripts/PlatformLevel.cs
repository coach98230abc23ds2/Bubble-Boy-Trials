using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlatformLevel : MonoBehaviour {

    private bool m_level_started;
    public string platformer_id;

    void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
    }

    void Start()
    {
        AudioSource sound_track = GetComponent<AudioSource>();
        sound_track.Play();
        m_level_started = true;
    }

    void Update()
    {
        if (m_level_started)
        {
            StartLevel();
        }
    }

    void StartLevel()
    {
        SceneManager.LoadScene(platformer_id, LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName(platformer_id);
        SceneManager.SetActiveScene(scene);
        m_level_started = true;
    }

    public void LevelComplete()
    {
        m_level_started = false;
      

    }


//
//    public void LevelCompleted()
//    {
//        m_level_started = false;
//        MazeUI.gameObject.SetActive(true);
//        LeftButton.gameObject.SetActive(m_current_node.Left_Node != null);
//        UpButton.gameObject.SetActive(m_current_node.Up_Node != null);
//        RightButton.gameObject.SetActive(m_current_node.Right_Node != null);
//    }
}
