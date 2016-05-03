using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(AudioSource))]

public class Cutscenes : MonoBehaviour {

    public MovieTexture boy_movie;
    public MovieTexture girl_movie;

    private MovieTexture movie;
    private AudioSource audio;
    private CharacterChoice char_choice;

	// Use this for initialization
	void Start () 
    {   
        char_choice = GameObject.Find("CharacterChoice").GetComponent<CharacterChoice>();
        SetMovie();
	    GetComponent<RawImage>().texture = movie as MovieTexture;
        audio = GetComponent<AudioSource>();
        audio.clip = movie.audioClip;
        movie.Play();
        audio.Play();
        StartCoroutine(WaitToStartMaze());
	}

    IEnumerator WaitToStartMaze()
    {
        yield return new WaitForSeconds(movie.duration);
        StartCutscene();
    }

    private void SetMovie()
    {
        if (char_choice.GetCharacter() == "Girl")
        {
            movie = girl_movie;
        }
        else
        {
            movie = boy_movie;
        }
    }

    void StartCutscene()
    {
        SceneManager.LoadScene("MazeScene", LoadSceneMode.Single);
        Scene scene = SceneManager.GetSceneByName("MazeScene");
        SceneManager.MoveGameObjectToScene(char_choice.gameObject, scene);
        SceneManager.SetActiveScene(scene);
    }

	// Update is called once per frame
	void Update () 
    {
//        if (Input.GetKeyDown(KeyCode.Space) && movie.isPlaying)
//        {
//            movie.Pause();
//        }
//        else if (Input.GetKeyDown(KeyCode.Space) && !movie.isPlaying)
//        {
//            movie.Play();
//        }
	}
}
