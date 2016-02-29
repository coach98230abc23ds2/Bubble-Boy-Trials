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
    public Text problem;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerTurn();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        timeRemaining -= Time.fixedDeltaTime;
	    switch (current_state) {
            case BattleState.player_turn:
                time_left.text = Mathf.CeilToInt(timeRemaining).ToString();
                if (timeRemaining < 0)
                {
                    Debug.Log("TRANSITION");
                    EnemyTurn();
                }
                break;
            case BattleState.enemy_turn:
                if (timeRemaining < ReactionWindow)
                {
                    time_left.text = "NOW!";
                } else {
                    time_left.text = "WAITING...";
                }
                if (timeRemaining < 0)
                {
                    PlayerTurn();
                    player.SendMessage("TakeDamage", 10);

                }
                else if (timeRemaining < ReactionWindow && Input.GetKeyDown(KeyCode.X))
                {
                    PlayerTurn();
                }
                else if (Input.GetKeyDown(KeyCode.X))
                {
                    PlayerTurn();
                    player.SendMessage("TakeDamage", 20);
                }
                break;
        }
	}

    private void PlayerTurn()
    {
        timeRemaining = TimePerTurn;
        current_state = BattleState.player_turn;
        answer1.onClick.RemoveAllListeners();
        answer2.onClick.RemoveAllListeners();
        answer3.onClick.RemoveAllListeners();
        answer4.onClick.RemoveAllListeners();
        answer1.gameObject.SetActive(true);
        answer2.gameObject.SetActive(true);
        answer3.gameObject.SetActive(true);
        answer4.gameObject.SetActive(true);
        variable_one = Mathf.CeilToInt(Random.Range(0, 10));
        variable_two = Mathf.CeilToInt(Random.Range(0, 10));
        problem.text = variable_one.ToString() + " + " + variable_two.ToString() + " = ?";
        problem.gameObject.SetActive(true);
        for (int i = 0; i < 4; ++i)
        {
            bool unique = false;
            // we can't have two bubbles with the same answer
            while (!unique) {
                answers[i] = Mathf.CeilToInt(Random.Range(0, 20));
                unique = answers[i] != variable_one + variable_two;
                for (int x = 0; x < i; ++x) {
                    unique = unique && answers[i] != answers[x];
                }
            }
        }
        int right_index = Mathf.FloorToInt(Random.Range(0, 3.999f));
        answers[right_index] = variable_one + variable_two;
        answer1.GetComponentInChildren<Text>().text = answers[0].ToString();
        answer2.GetComponentInChildren<Text>().text = answers[1].ToString();
        answer3.GetComponentInChildren<Text>().text = answers[2].ToString();
        answer4.GetComponentInChildren<Text>().text = answers[3].ToString();
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
        problem.gameObject.SetActive(false);
        current_state = BattleState.enemy_turn;
        timeRemaining = Random.Range(2, 2 + TimePerTurn);
    }
}
