using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    private Animator char_anim;
    private GameObject canvas;
    private GameObject text_canvas;
    private GameObject camera;
    private GameObject char_choice;
    private GameObject event_sys;

	// Use this for initialization
	void Start () 
    {
	    this.gameObject.transform.SetAsLastSibling();
        char_anim = this.gameObject.GetComponent<Animator>();
        canvas = GameObject.Find("Canvas");
        text_canvas = GameObject.Find("TextCanvas");
        camera = GameObject.Find("Camera");
        char_choice = GameObject.Find("CharacterChoice");
        event_sys = GameObject.Find("EventSystem");
	}
	
	void OnMouseEnter()
    {
        char_anim.SetFloat("Speed", 1f);    
    }

    void OnMouseExit()
    {
        char_anim.SetFloat("Speed", 0f);
    }

    public void MakeCharacterJump()
    {
        char_anim.SetTrigger("Jump");
        StartCoroutine(StartMaze());
    }

    IEnumerator StartMaze()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MazeScene", LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName("MazeScene");
        SceneManager.MoveGameObjectToScene(char_choice, scene);
        SceneManager.UnloadScene(6);
        SceneManager.SetActiveScene(scene);
    }
}
