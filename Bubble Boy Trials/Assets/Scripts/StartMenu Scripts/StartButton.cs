using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class StartButton : MonoBehaviour {

    private GameObject canvas;
    private GameObject camera;
    private GameObject event_system;

    void Awake()
    {   
        canvas = GameObject.Find("Canvas");
        camera = GameObject.Find("Main Camera");
        event_system = GameObject.Find("EventSystem");
    }

    public void StartCharacterSelection ()
    {
        SceneManager.LoadScene("CharacterSelection", LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName("CharacterSelection");

        try
        {
            GameObject char_choice = GameObject.Find("CharacterChoice");
            GameObject maze_system = GameObject.Find("MazeSystem");
            Destroy(char_choice); 
            Destroy(maze_system);

        }
        catch 
        {   
            Debug.LogError("There is no character selected.");
        }

        SceneManager.MoveGameObjectToScene(event_system, scene);
        Destroy(camera);
        SceneManager.SetActiveScene(scene);
    }
}
