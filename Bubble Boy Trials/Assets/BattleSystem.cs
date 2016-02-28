using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BattleSystem : MonoBehaviour {

    public enum BattleState {
        player_turn,
        enemy_turn
    };

    private GameObject player;
    public GameObject enemy;
    private BattleState current_state;

    [Range(1, 30)] public float TimePerTurn = 10000;
    [Range(0.25f,2)] public float ReactionWindow = 1000;

    private float timeRemaining;
    private float timeToDefend;

    private int variable_one;
    private int variable_two;
    private int[] answers = new int[4];

    public Button answer1;
    public Button answer2;
    public Button answer3;
    public Button answer4;

    public Text time_left;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerTurn();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        timeRemaining -= Time.fixedDeltaTime;
        time_left.text = timeRemaining.ToString();
	    switch (current_state) {
            case BattleState.player_turn:
                if (timeRemaining < 0)
                {
                    EnemyTurn();
                }
                break;
            case BattleState.enemy_turn:
                if (timeRemaining < 0)
                {
                    PlayerTurn();
                    player.SendMessage("TakeDamage", 10);

                }
                else if (timeRemaining < ReactionWindow && Input.GetKeyDown(KeyCode.Space))
                {
                    PlayerTurn();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    player.SendMessage("TakeDamage", 20);
                }
                if (timeRemaining < ReactionWindow)
                {
                    time_left.text = "NOW!";
                }
                break;
        }
	}

    private void PlayerTurn()
    {
        current_state = BattleState.player_turn;
        answer1.gameObject.SetActive(true);
        answer2.gameObject.SetActive(true);
        answer3.gameObject.SetActive(true);
        answer4.gameObject.SetActive(true);
        timeRemaining = TimePerTurn;
        variable_one = Mathf.CeilToInt(Random.Range(0, 10));
        variable_two = Mathf.CeilToInt(Random.Range(0, 10));
        for (int i = 0; i < 4; ++i)
        {
            answers[i] = Mathf.CeilToInt(Random.Range(0, 20));
        }
        int right_index = Mathf.FloorToInt(Random.Range(0, 3.999f));
        answers[right_index] = variable_one + variable_two;
        answer1.GetComponentInChildren<Text>().text = answers[0].ToString();
        answer1.onClick.RemoveAllListeners();
        answer2.GetComponentInChildren<Text>().text = answers[1].ToString();
        answer2.onClick.RemoveAllListeners();
        answer3.GetComponentInChildren<Text>().text = answers[2].ToString();
        answer3.onClick.RemoveAllListeners();
        answer4.GetComponentInChildren<Text>().text = answers[3].ToString();
        answer4.onClick.RemoveAllListeners();
        // there's definitely a better way to do this... but for now...
        switch (right_index)
        {
            case 0:
                answer1.onClick.AddListener(RightAnswer);
                answer2.onClick.AddListener(WrongAnswer);
                answer3.onClick.AddListener(WrongAnswer);
                answer4.onClick.AddListener(WrongAnswer);
                break;
            case 1:
                answer2.onClick.AddListener(RightAnswer);
                answer1.onClick.AddListener(WrongAnswer);
                answer3.onClick.AddListener(WrongAnswer);
                answer4.onClick.AddListener(WrongAnswer);
                break;
            case 2:
                answer3.onClick.AddListener(RightAnswer);
                answer1.onClick.AddListener(WrongAnswer);
                answer2.onClick.AddListener(WrongAnswer);
                answer4.onClick.AddListener(WrongAnswer);
                break;
            default:
                answer4.onClick.AddListener(RightAnswer);
                answer1.onClick.AddListener(WrongAnswer);
                answer2.onClick.AddListener(WrongAnswer);
                answer3.onClick.AddListener(WrongAnswer);
                break;
        }
    }

    private void RightAnswer()
    {
        enemy.SendMessage("TakeDamage", 10);
        EnemyTurn();
    }

    private void WrongAnswer()
    {
        EnemyTurn();
    }

    private void EnemyTurn()
    {
        answer1.gameObject.SetActive(false);
        answer2.gameObject.SetActive(false);
        answer3.gameObject.SetActive(false);
        answer4.gameObject.SetActive(false);
        current_state = BattleState.enemy_turn;
        timeRemaining = Random.Range(2, 2 + TimePerTurn);
    }
}
