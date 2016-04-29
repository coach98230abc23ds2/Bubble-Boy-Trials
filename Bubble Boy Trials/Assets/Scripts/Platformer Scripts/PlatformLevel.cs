using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    private AudioSource source;
    private GameObject platform_door;

    public GameObject m_elevator;
    public GameObject maze_camera;
    public GameObject plat_camera;
    public GameObject player_prefab;



    void Start()
    {
        source = GetComponent<AudioSource>();
        source.Play();
        m_level_started = true;
        m_platform = GameObject.Find("Platform");
        m_player = GameObject.Find("Player");
        m_main_camera = GameObject.Find("MainCamera");
        m_canvas = GameObject.Find("Canvas");
        m_health_bar = GameObject.Find("HealthBar");
    }

    public void ResumeLevel()
    {   
//        SceneManager.LoadScene(platformer_name, LoadSceneMode.Additive);
        Resources.UnloadUnusedAssets();
        m_player.GetComponent<PlatformPlayer>().health_bar.SetActive(true);
        m_player.SetActive(true);
        m_platform.SetActive(true);
        m_main_camera.SetActive(true);
        platform_door.SetActive(true);


        Scene scene = SceneManager.GetSceneByName(platformer_name);
        SceneManager.SetActiveScene(scene);
        m_level_started = true;
    }

    public void PauseLevel(GameObject door)
    {   
        if (m_level_started)
        {
            m_level_started = false;
            source.Stop();
            SceneManager.LoadScene("BossBattleScene", LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName("BossBattleScene");
            SceneManager.SetActiveScene(scene);
            platform_door = door;
            StartCoroutine(HideDoor(door));
        }

    }

    IEnumerator HideDoor(GameObject door)
    {
        yield return new WaitForSeconds(2.0f);
        door.SetActive(false);
    }

    public void LevelCompleted()
    { 
        SceneManager.LoadScene("MazeScene", LoadSceneMode.Single);
        Scene scene = SceneManager.GetSceneByName("MazeScene");
        SceneManager.SetActiveScene(scene);
    }



    void ClearScripts(GameObject obj)
    {
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
     
    }

}
