using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityStandardAssets._2D;

public class PlatformLevel : MonoBehaviour {

    public bool m_level_started;

    public string platformer_name = "PrimeScene";
    private GameObject m_platform;
    private GameObject m_player;
    private GameObject m_main_camera;
    private GameObject m_canvas;
    private GameObject m_health_bar;
    private GameObject new_elevator;
    private GameObject new_camera;
    private AudioSource[] source;
    private GameObject platform_door;
    private Platformer2DUserControl m_player_control; 
    private Animator door_anim;
    private CharacterChoice char_choice;


    public GameObject m_elevator;
    public GameObject maze_camera;
    public GameObject plat_camera;
    public GameObject player_prefab;
    public RuntimeAnimatorController boy_anim_control;
    public RuntimeAnimatorController girl_anim_control;
    public Sprite boy_sprite;
    public Sprite girl_sprite;



    void Start()
    {
        source = GetComponents<AudioSource>();
        source[0].Play();
        m_level_started = true;
        m_platform = GameObject.Find("Platform");
        m_player = GameObject.Find("Player");
        m_main_camera = GameObject.Find("MainCamera");
        m_canvas = GameObject.Find("Canvas");
        m_health_bar = GameObject.Find("HealthBar");
        m_player_control = GameObject.Find("Player").GetComponent<Platformer2DUserControl>();
        door_anim = GameObject.Find("BossDoor").GetComponent<Animator>();
        SetSpriteAndAnimator();
    }

    void SetSpriteAndAnimator()
    {   
        try
        {
            char_choice = GameObject.Find("CharacterChoice").GetComponent<CharacterChoice>();
            if (char_choice.GetCharacter() == "Girl")
            {
                m_player.GetComponent<SpriteRenderer>().sprite = girl_sprite;
                m_player.GetComponent<Animator>().runtimeAnimatorController = girl_anim_control;
            }
            else
            {
                m_player.GetComponent<SpriteRenderer>().sprite = boy_sprite;
                m_player.GetComponent<Animator>().runtimeAnimatorController = boy_anim_control;
            }
        }
        catch
        {
            Debug.LogError("There is no character selected.");
        }
    }

    public void ResumeLevel(AudioSource music)
    {   
//        SceneManager.LoadScene(platformer_name, LoadSceneMode.Additive);
//        Resources.UnloadUnusedAssets();
        music.Play();
        m_player.GetComponent<PlatformPlayer>().health_bar.SetActive(true);
        m_player.SetActive(true);
        m_platform.SetActive(true);
        m_main_camera.SetActive(true);
        platform_door.SetActive(true);
        door_anim.SetTrigger("Idle");
        m_player.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        m_player_control.m_can_move = true;

        Scene scene = SceneManager.GetSceneByName(platformer_name);
        SceneManager.SetActiveScene(scene);
        m_level_started = true;
    }

    public void PauseLevel(GameObject door)
    {   
        if (m_level_started)
        {
            m_level_started = false;
            source[0].Stop();
            SceneManager.LoadScene("BossBattleScene", LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName("BossBattleScene");
            SceneManager.SetActiveScene(scene);
            platform_door = door;
            StartCoroutine(HideDoor(door));
        }

    }

    public IEnumerator HideDoor(GameObject door)
    {
        yield return new WaitForSeconds(2.0f);
        door.SetActive(false);

    }

    public void SwitchToMaze(GameObject door)
    {   
        m_player.GetComponent<PlatformPlayer>().RenderPlayerImmobile();
        m_player.transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        m_player.GetComponent<Platformer2DUserControl>().m_can_move = false;
        Door door_script = door.GetComponent<Door>();
        StartCoroutine(door_script.WaitToSwitchMaze(door.transform.position));
//        Debug.Log("switching to maze");
    }

}
