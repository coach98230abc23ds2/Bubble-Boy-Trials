using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlatformLevel : MonoBehaviour {

    void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
        AudioSource sound_track = GetComponent<AudioSource>();
        sound_track.Play();
    }

//    void StartLevel()
//    {
//        SceneManager.LoadScene(m_current_node.Scene_Id, LoadSceneMode.Additive);
//        Scene scene = SceneManager.GetSceneByName(m_current_node.Scene_Id);
//        SceneManager.SetActiveScene(scene);
//        m_level_started = true;
//        m_path_chosen = false;
//    }
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
