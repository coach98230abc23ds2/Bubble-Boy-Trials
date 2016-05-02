using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Door : MonoBehaviour {
    public AudioClip audio;
    private Animator m_door_anim;
    private PlatformLevel level;
    private Animator player_anim;

    public AnimationClip door_clip;
    public AudioClip[] boss_music;
    private AudioSource source;
    private Vector3 door_position;


    void Awake ()
    {
        m_door_anim = this.gameObject.GetComponent<Animator>();
        level = GameObject.Find("PlatformLevel").GetComponent<PlatformLevel>();
        source = this.gameObject.GetComponent<AudioSource>();
        door_position = this.gameObject.transform.position;
        player_anim = GameObject.Find("Player").GetComponent<Animator>();
    }

    public IEnumerator WaitToSwitch(Vector3 position)
    {
       PlaySound(position);
       yield return new WaitForSeconds(door_clip.length/4);
       level.PauseLevel(this.gameObject);
    }

    public IEnumerator WaitToSwitchMaze (Vector3 position)
    {
        PlaySound(position);
//        m_door_anim.SetTrigger("Active");
        m_door_anim.SetBool("active", true);
        yield return new WaitForSeconds(door_clip.length/2);
        player_anim.SetFloat("Speed", 1f);
        SceneManager.LoadScene("MazeScene", LoadSceneMode.Single);
        Scene scene = SceneManager.GetSceneByName("MazeScene");
        SceneManager.SetActiveScene(scene);
    }

    public void PlaySound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audio, position);
    }
   
}
