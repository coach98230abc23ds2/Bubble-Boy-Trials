using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class StartButton : MonoBehaviour {

    public void StartGame () 
    {
        SceneManager.LoadScene("MazeScene", LoadSceneMode.Single);
        Scene scene = SceneManager.GetSceneByName("MazeScene");
        SceneManager.SetActiveScene(scene);
    }
}
