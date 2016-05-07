using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class BossBattleSystem : MonoBehaviour
{

    public enum BattleState
    {
        player_turn,
        enemy_turn}

    ;

    public Button answer1;
    public Button answer2;
    public Button answer3;
    public Button answer4;

    public Slider time_remaining;
    public Text battleMessage;
    public Text problem;
    public GameObject enemy;
    public Door door;
    public AudioClip[] boss_music;
    private AudioSource source;

    private PlatformLevel platform_lvl;
    private GameObject player;
    private BattleState current_state;
    private AudioSource[] platform_music;

    private static float MAX_TIME_PER_TURN = 10;
    private static float TIME_INCREASE_ON_WRONG_ANSWER = 1;
    private static Dictionary<Problem.Operator,RollingAverage> time_per_turn;

    [Range(1, 20)] public static int TimePerTurnAverageLength = 5;
    [Range(0.25f, 2)] public float ReactionWindow = 1000;

    private float timeRemaining;
    private float timeToDefend;

    private List<Button> answers;
    private Problem current_problem;

    private GameObject bubble;
    private bool attackingPlayer;
    private bool bubbleLive;
    private bool createBubble;
    private int comboChain;
    private bool started;
    private int m_score;
    private AudioSource[] sounds;
    private Animator enemy_anim;
    private GameObject main_camera;

    private float y_velocity = 0;
    private float ground_level;

    void Awake()
    {
        sounds = this.gameObject.GetComponents<AudioSource>();
        StartCoroutine(PlayBossMusic());
        enemy_anim = enemy.GetComponent<Animator>();
        main_camera = GameObject.Find("MainCamera");
        platform_music = GameObject.Find("PlatformLevel").GetComponents<AudioSource>();
    }

    IEnumerator PlayBossMusic()
    {
        sounds[1].Play();
        yield return new WaitForSeconds(sounds[1].clip.length + .5f);
        sounds[2].Play();
    }

    // Use this for initialization
    void Start()
    {
        if (time_per_turn == null)
        {
            time_per_turn = new Dictionary<Problem.Operator, RollingAverage>();
            foreach (Problem.Operator op in System.Enum.GetValues(typeof(Problem.Operator)))
            {
                time_per_turn.Add(op, new RollingAverage(TimePerTurnAverageLength, MAX_TIME_PER_TURN));
            }
        }
        player = GameObject.FindGameObjectWithTag("Player");
        platform_lvl = GameObject.Find("PlatformLevel").GetComponent<PlatformLevel>();
        enemy.transform.position = new Vector3 (352.2f, 42.38f, 0);
        answers = new List<Button> { answer1, answer2, answer3, answer4 };
        GameObject.Find("/Canvas/ScoreText").GetComponent<Text>().text = "Score: " + m_score;
        ground_level = player.transform.position.y;
        player.GetComponent<Rigidbody2D>().isKinematic = true;
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        if (y_velocity != 0)
        {
            player.transform.position += new Vector3(0, y_velocity * Time.fixedDeltaTime, 0);
            y_velocity -= 9.81f * Time.fixedDeltaTime;
            if (player.transform.position.y <= ground_level)
            {
                player.transform.position = new Vector3(player.transform.position.x, ground_level, player.transform.position.z);
                y_velocity = 0;
            }
        }

        if (!started)
        {
            if (enemy.transform.position.x - player.transform.position.x < 11f)
            {
                started = true;
                PlayerTurn();
                enemy_anim.SetFloat("Speed", 0f);
            }
            else
            {
                enemy.transform.position -= new Vector3(.8f * Time.fixedDeltaTime, 0, 0);
                enemy_anim.SetFloat("Speed", 1f);
            }
        }

        if (started)
        {   
            timeRemaining -= Time.fixedDeltaTime;

            Animator playerAnimator = player.GetComponent<Animator>();
            Animator enemyAnimator = enemy.GetComponent<Animator>();
            if (createBubble && !attackingPlayer && playerAnimator.IsInTransition(0) && 
                (playerAnimator.GetNextAnimatorStateInfo(0).IsName("Boy Idle") || playerAnimator.GetNextAnimatorStateInfo(0).IsName("Girl Idle")))
            {
                if (bubble != null)
                {
                    GameObject.Destroy(bubble);
                }
                bubble = GameObject.Instantiate(Resources.Load("Bubble"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                bubble.transform.position = player.transform.position + new Vector3(0.5f, 0f, 0);
                bubble.transform.localScale += new Vector3(1f,1f,1f);
                bubbleLive = true;
                createBubble = false;
            }
            else if (createBubble && attackingPlayer && enemyAnimator.IsInTransition(0) && enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Prime Attack"))
            {
                if (bubble != null)
                {
                    GameObject.Destroy(bubble);
                }
                bubble = GameObject.Instantiate(Resources.Load("Bubble"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                bubble.transform.position = enemy.transform.position + new Vector3(-0.5f, 0f, 0);
                bubble.transform.localScale += new Vector3(1f,1f,1f);
                bubbleLive = true;
                createBubble = false;
            }

            if (bubble != null)
            {
                bubble.transform.position += new Vector3(6.0f, 0, 0) * Time.fixedDeltaTime * (attackingPlayer ? -1 : 1);
                if (bubbleLive && attackingPlayer)
                {
                    if (Vector3.Distance(bubble.transform.position, player.transform.position) < 2f)
                    {
                        player.GetComponent<PlatformPlayer>().UpdateHealthBar(25);
                        player.GetComponentInParent<Animator>().SetTrigger("Defend");
                        bubble.GetComponent<Animator>().SetTrigger("Burst");
                        PlayerTurn();
                        bubbleLive = false;
                        bubble.GetComponents<AudioSource>()[1].Play();
                    }
                    if (Input.GetKeyDown(KeyCode.Space) && y_velocity == 0)
                    {
                        y_velocity = 10f;
                    }
                    if (bubble.transform.position.x + 2f < player.transform.position.x)
                    {
                        bubble.GetComponent<Animator>().SetTrigger("Burst");
                        player.GetComponents<AudioSource>()[0].Play();
                        PlayerTurn();
                        bubbleLive = false;
                        bubble.GetComponents<AudioSource>()[1].Play();
                    }
                }
                else if (bubbleLive && !attackingPlayer && bubble.transform.position.x - enemy.transform.position.x > -0.5f)
                {
                    bubble.GetComponent<Animator>().SetTrigger("Burst");
                    enemy.GetComponent<AudioSource>().Play();
                    ApplyHit();
                    bubbleLive = false;
                    bubble.GetComponents<AudioSource>()[1].Play();
                }
            }

            switch (current_state)
            {
                case BattleState.player_turn:
                    time_remaining.value = timeRemaining;
                    if (timeRemaining < 0)
                    {
                        WrongAnswer();
                    }
                    break;
                case BattleState.enemy_turn:
                    if (timeRemaining < ReactionWindow && !createBubble && !bubbleLive)
                    {   
                        if (enemyAnimator !=  null)
                        {
                            enemyAnimator.SetTrigger("Attacking");
                        }
                        createBubble = true;
                        attackingPlayer = true;
                    }
                    break;
            }
        }

        if (player.GetComponent<PlatformPlayer>().m_health <= 0f)
        {
            
            player.GetComponent<Rigidbody2D>().isKinematic = false;
            SceneManager.UnloadScene(5);
//            Destroy(enemy.GetComponent<Boss>().healthBar);
            platform_lvl.ResumeLevel(platform_music[0]);

        }
    }


    private void ResetButton(Button button)
    {
        button.gameObject.SetActive(true);
        SetListener(button, false);
    }


    /* Sets the button's only listener to be Right/WrongAnswer */
    private void SetListener(Button button, bool isRightAnswer)
    {
        button.onClick.RemoveAllListeners();
        if (isRightAnswer)
        {
            button.onClick.AddListener(RightAnswer);
        }
        else
        {
            button.onClick.AddListener(WrongAnswer);
        }
    }

    private void ShowPlayerUI()
    {
        answers.ForEach(b => ResetButton(b));
        problem.gameObject.SetActive(true);
        time_remaining.gameObject.SetActive(true);
        battleMessage.gameObject.SetActive(false);
    }

    private void HidePlayerUI()
    {
        answers.ForEach(b => b.gameObject.SetActive(false));
        time_remaining.gameObject.SetActive(false);
        battleMessage.gameObject.SetActive(true);
        problem.text = "Wait for the bubble to hit!";
    }

    private float CurrentAverage()
    {
        try
        {
            return time_per_turn[current_problem.GetOperator()].GetAverage();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Operator not found!: " + current_problem.GetOperator().ToString());
            throw e;
        }
    }

    private void PlayerTurn()
    {
        // generate new problem
        // this must go above CurrentAverage()
        // since that requires the operation is known
        current_problem = new Problem();

        timeRemaining = CurrentAverage();
        time_remaining.maxValue = timeRemaining;

        current_state = BattleState.player_turn;

        // reset buttons and UI elements
        ShowPlayerUI();
		
        // set question text fields based on new problem
        problem.text = current_problem.ToString();
        problem.gameObject.SetActive(true);
		
        // set solution text fields based on new Problem
        int[] possibleSolutions = current_problem.GetPossibleSolutions();

        // although this is set as a variable in newProblem
        // we need to make UI buttons by hand on the scene, so
        // we should ensure it is equal to 4
        Assert.AreEqual(possibleSolutions.GetLength(0), answers.Count);
        for (int i = 0; i < answers.Count; ++i)
        {
            answers[i].GetComponentInChildren<Text>().text = possibleSolutions[i].ToString();
        }

        SetListener(answers[current_problem.GetSolutionIndex()], true);
    }

    private void RightAnswer()
    {
        createBubble = true;
        attackingPlayer = false;
        player.GetComponent<Animator>().SetTrigger("Attack");
        GetComponent<AudioSource>().Play();
        HidePlayerUI();
    }

    private void ApplyHit()
    {
        int dmg = 10;
        if (comboChain >= 3)
        {
            dmg += 10 * comboChain - 2;
        }
        m_score += dmg;
        GameObject.Find("/Canvas/ScoreText").GetComponent<Text>().text = "Score: " + m_score;
        problem.text = "You did " + dmg + " damage!";

        enemy.GetComponent<Boss>().TakeDamage(dmg);
        if (enemy.GetComponent<Boss>().isDead)
        {
            player.GetComponent<Rigidbody2D>().isKinematic = false;
            SceneManager.UnloadScene(5);
            platform_lvl.ResumeLevel(platform_music[1]);
        }
        else
        {
            time_per_turn[current_problem.GetOperator()].AddValue(
                CurrentAverage() - timeRemaining
            );


            comboChain++;

            EnemyTurn();
        }
    }

    private void WrongAnswer()
    {
        //player.GetComponents<AudioSource>()[1].Play();
        time_per_turn[current_problem.GetOperator()].AddValue(
            CurrentAverage() + TIME_INCREASE_ON_WRONG_ANSWER
        );
        comboChain = 0;
        HidePlayerUI();
        EnemyTurn();
    }

    private void EnemyTurn()
    {
        current_state = BattleState.enemy_turn;
        problem.text = "Now jump over the bubble with SPACEBAR!";
    }
}
