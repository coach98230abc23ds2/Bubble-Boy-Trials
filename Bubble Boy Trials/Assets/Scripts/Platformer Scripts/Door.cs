using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Door : MonoBehaviour {
    public AudioClip audio;
    private Animator m_door_anim;
    private PlatformLevel level;

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
    }

    public IEnumerator WaitToSwitch(Vector3 position)
    {
       PlaySound(position);
       yield return new WaitForSeconds(door_clip.length);
       level.LevelCompleted(this.gameObject);
    }


    public void PlaySound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audio, position);
    }
   
}
