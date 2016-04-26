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

    public GameObject m_elevator;
    public GameObject maze_camera;
    public GameObject plat_camera;
    public GameObject player_prefab;



    void Awake()
    {

    }

    void Start()
    {
        AudioSource sound_track = GetComponent<AudioSource>();
        sound_track.Play();
        m_level_started = true;
        m_platform = GameObject.Find("Platform");
        m_player = GameObject.Find("Player");
        m_main_camera = GameObject.Find("MainCamera");
        m_canvas = GameObject.Find("Canvas");
        m_health_bar = GameObject.Find("HealthBar");
    }

    void Update()
    {
        if (!m_level_started)
        {   
            m_level_started = true;
            StartLevel();
        }
    }

    public void StartLevel()
    {   
//        SceneManager.LoadScene(platformer_name, LoadSceneMode.Additive);
        m_player.GetComponent<PlatformPlayer>().health_bar.SetActive(true);
        m_player.SetActive(true);
        m_platform.SetActive(true);
        m_main_camera.SetActive(true);

        Scene scene = SceneManager.GetSceneByName(platformer_name);
        SceneManager.SetActiveScene(scene);
        m_level_started = true;
    }

    public void LevelCompleted()
    {
        SceneManager.LoadScene("BossBattleScene", LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName("BossBattleScene");
        SceneManager.SetActiveScene(scene);
//        m_platform.SetActive(false);
//        Resources.UnloadUnusedAssets();
        m_main_camera.SetActive(false);


        m_player.GetComponent<PlatformPlayer>().health_bar.SetActive(false);
        m_player.SetActive(false);

        new_elevator = Instantiate(m_elevator);
        new_elevator.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = "Character";
        SceneManager.MoveGameObjectToScene(new_elevator, scene);

        new_camera = Instantiate(maze_camera);
        SceneManager.MoveGameObjectToScene(new_camera, scene);
       
    }

    void ClearScripts(GameObject obj)
    {
        MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
        foreach(MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
     
    }

//
//    public void LevelCompleted()
//    {
//        m_level_started = false;
//        MazeUI.gameObject.SetActive(true);
//        LeftButton.gameObject.SetActive(m_current_node.Left_Node != null);
//        UpButton.gameObject.SetActive(m_current_node.Up_Node != null);
//        RightButton.gameObject.SetActive(m_current_node.Right_Node != null);
//    }
}
