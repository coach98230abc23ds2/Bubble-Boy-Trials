using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour
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

    private GameObject player;
    private BattleState current_state;

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
        enemy.transform.position = player.transform.position + new Vector3(10, 0, 0);
        answers = new List<Button> { answer1, answer2, answer3, answer4 };
        m_score = GameObject.FindGameObjectWithTag("MazeSystem").GetComponent<MazeSystem>().GetScore();
        GameObject.Find("/Canvas/ScoreText").GetComponent<Text>().text = "Score: " + m_score;
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!started)
        {
            if (Vector3.Distance(enemy.transform.position, player.transform.position) < 5.2f)
            {
                started = true;
                PlayerTurn();
            }
            else
            {
                enemy.transform.position -= new Vector3(2f * Time.fixedDeltaTime, 0, 0);
            }
        }
        if (started)
        {
            timeRemaining -= Time.fixedDeltaTime;

            if (player.GetComponent<Player>().isDead)
            {
                GameObject.FindGameObjectWithTag("MazeSystem").GetComponent<MazeSystem>().LevelCompleted(false);
            }

            Animator playerAnimator = player.GetComponent<Animator>();
            Animator enemyAnimator = enemy.GetComponent<Animator>();
            if (createBubble && !attackingPlayer && playerAnimator.IsInTransition(0) && playerAnimator.GetNextAnimatorStateInfo(0).IsName("Idle"))
            {
                if (bubble != null)
                {
                    GameObject.Destroy(bubble);
                }
                bubble = GameObject.Instantiate(Resources.Load("Bubble"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                bubble.transform.position = player.transform.position + new Vector3(0.5f, 0f, 0);
                bubbleLive = true;
                createBubble = false;
            }
            else if (createBubble && attackingPlayer && enemyAnimator.IsInTransition(0) && enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Green Minion Attack"))
            {
                if (bubble != null)
                {
                    GameObject.Destroy(bubble);
                }
                bubble = GameObject.Instantiate(Resources.Load("SlimeBubble"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                bubble.transform.position = enemy.transform.position + new Vector3(-0.5f, 0f, 0);
                bubbleLive = true;
                createBubble = false;
            }

            if (bubble != null)
            {
                bubble.transform.position += new Vector3(1.5f, 0, 0) * Time.fixedDeltaTime * (attackingPlayer ? -1 : 1);
                if (bubbleLive && attackingPlayer)
                {
                    if (Vector3.Distance(bubble.transform.position, player.transform.position) < 0.5f)
                    {
                        player.GetComponent<Player>().TakeDamage(10);
                        player.GetComponentInParent<Animator>().SetTrigger("Defend");
                        bubble.GetComponent<Animator>().SetTrigger("Burst");
                        PlayerTurn();
                        bubbleLive = false;
                        bubble.GetComponents<AudioSource>()[1].Play();
                    }
                    else if (Vector3.Distance(bubble.transform.position, player.transform.position) > 1f && Vector3.Distance(bubble.transform.position, player.transform.position) < 3.5f && Input.GetKeyDown(KeyCode.Space))
                    {
                        GameObject.FindGameObjectWithTag("PlayerElevator").GetComponentInParent<Animator>().SetTrigger("Defend");
                        bubble.GetComponent<Animator>().SetTrigger("Burst");
                        player.GetComponents<AudioSource>()[0].Play();
                        PlayerTurn();
                        bubbleLive = false;
                        bubble.GetComponents<AudioSource>()[1].Play();
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        player.GetComponent<Player>().TakeDamage(20);
                        player.GetComponentInParent<Animator>().SetTrigger("Defend");
                        GameObject.FindGameObjectWithTag("PlayerElevator").GetComponentInParent<Animator>().SetTrigger("Defend");
                        bubble.GetComponent<Animator>().SetTrigger("Burst");
                        PlayerTurn();
                        bubbleLive = false;
                        player.GetComponents<AudioSource>()[1].Play();
                        bubble.GetComponents<AudioSource>()[1].Play();
                    }
                }
                else if (bubbleLive && !attackingPlayer && Vector3.Distance(bubble.transform.position, enemy.transform.position) < 0.5)
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
                        enemyAnimator.SetTrigger("Attacking");
                        createBubble = true;
                        attackingPlayer = true;
                    }
                    break;
            }
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
        problem.text = "You are attacking!";
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
        enemy.GetComponent<Player>().TakeDamage(dmg);
        if (enemy.GetComponent<Player>().isDead)
        {
            SceneManager.UnloadScene(2);
            GameObject.FindGameObjectWithTag("MazeSystem").GetComponent<MazeSystem>().LevelCompleted(true, m_score);
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
        player.GetComponents<AudioSource>()[1].Play();
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
        problem.text = "Now defend by pressing SPACEBAR!";
    }
}
