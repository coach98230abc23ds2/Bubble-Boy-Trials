using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Door : MonoBehaviour {
    public AudioClip audio;
    private Animator m_door_anim;
    private PlatformLevel level;

    void Awake ()
    {
        m_door_anim = this.gameObject.GetComponent<Animator>();
        level = GameObject.Find("Platform").GetComponent<PlatformLevel>();
    }
    public IEnumerator WaitToSwitch(Vector3 position)
    {
        PlaySound(position);
//        yield return new WaitUntil(x => x = (m_door_anim.IsInTransition(0) 
//              && m_door_anim.GetNextAnimatorStateInfo(0).IsName("Idle")));
////      
//        while (!(m_door_anim.IsInTransition(0) 
//              && m_door_anim.GetNextAnimatorStateInfo(0).IsName("Idle")))
//        {
//            yield return null;
//        }
       yield return new WaitForSeconds(2f);
//  
//       SceneManager.UnloadScene(level.platformer_name);
       level.LevelCompleted();


    }

    public void PlaySound(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audio, position);
    }
   
}
