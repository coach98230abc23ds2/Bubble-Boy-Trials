using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class StartButton : MonoBehaviour {

    private GameObject canvas;
    private GameObject camera;
    private GameObject event_system;
    private GameObject char_choice;

    void Awake()
    {   
        canvas = GameObject.Find("Canvas");
        camera = GameObject.Find("Main Camera");
        event_system = GameObject.Find("EventSystem");
        char_choice = GameObject.Find("CharacterChoice");
    }

    public void StartCharacterSelection ()
    {
        SceneManager.LoadScene("CharacterSelection", LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName("CharacterSelection");
        Destroy(char_choice);
        SceneManager.MoveGameObjectToScene(event_system, scene);
        Destroy(camera);
        SceneManager.SetActiveScene(scene);
    }
}
