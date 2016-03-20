using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class BattleSystem : MonoBehaviour
{

		public enum BattleState
		{
				player_turn,
				enemy_turn
		};

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

		[Range (1, 30)] public float TimePerTurn = 10000;
		[Range (0.25f, 2)] public float ReactionWindow = 1000;

		private float timeRemaining;
		private float timeToDefend;

		private List<Button> answers;


		private int comboChain;

		// Use this for initialization
		void Start ()
		{
				player = GameObject.FindGameObjectWithTag ("Player");
				answers = new List<Button> { answer1, answer2, answer3, answer4 };
				PlayerTurn ();
		}
	
		// Update is called once per frame
		void FixedUpdate ()
		{
				timeRemaining -= Time.fixedDeltaTime;
				switch (current_state) {
				case BattleState.player_turn:
						time_remaining.value = timeRemaining;
						if (timeRemaining < 0) {
								comboChain = 0;
								EnemyTurn ();
						}
						break;
				case BattleState.enemy_turn:
						if (timeRemaining < ReactionWindow) {
								battleMessage.text = "NOW!";
						} else {
								battleMessage.text = "WAITING...";
						}
						if (timeRemaining < 0) {
								PlayerTurn ();
								player.GetComponent<Player>().TakeDamage(10);

						} else if (timeRemaining < ReactionWindow && Input.GetKeyDown (KeyCode.X)) {
								PlayerTurn ();
						} else if (Input.GetKeyDown (KeyCode.X)) {
								PlayerTurn ();
								player.GetComponent<Player> ().TakeDamage (20);
						}
						break;
				}
		}


		private void ResetButton (Button button)
		{
				button.gameObject.SetActive (true);
				SetListener (button, false);
		}


		/* Sets the button's only listener to be Right/WrongAnswer */
		private void SetListener (Button button, bool isRightAnswer)
		{
				button.onClick.RemoveAllListeners ();
				if (isRightAnswer) {
						button.onClick.AddListener (RightAnswer);
				} else {
						button.onClick.AddListener (WrongAnswer);
				}
		}

		private void ShowPlayerUI ()
		{
				answers.ForEach (b => ResetButton (b));
				problem.gameObject.SetActive (true);
				time_remaining.gameObject.SetActive (true);
				battleMessage.gameObject.SetActive (false);
		}

		private void HidePlayerUI ()
		{
				answers.ForEach (b => b.gameObject.SetActive (false));
				problem.gameObject.SetActive (false);
				time_remaining.gameObject.SetActive (false);
				battleMessage.gameObject.SetActive (true);
		}


		private void PlayerTurn ()
		{
				timeRemaining = TimePerTurn;
				current_state = BattleState.player_turn;

				// reset buttons and UI elements
				ShowPlayerUI ();

				// generate new problem
				Problem newProblem = new Problem ();
		
				// set question text fields based on new problem
				problem.text = newProblem.ToString();
				problem.gameObject.SetActive (true);
		
				// set solution text fields based on new Problem
				int[] possibleSolutions = newProblem.GetPossibleSolutions ();

				// although this is set as a variable in newProblem
				// we need to make UI buttons by hand on the scene, so
				// we should ensure it is equal to 4
				Assert.AreEqual (possibleSolutions.GetLength (0), answers.Count);
				for (int i = 0; i < answers.Count; ++i) {
						answers [i].GetComponentInChildren<Text> ().text = possibleSolutions [i].ToString ();
				}

				SetListener (answers [newProblem.GetSolutionIndex ()], true);
		}

		private void RightAnswer ()
		{
				int dmg = 10;
				if (comboChain >= 3) {
						dmg += 10 * comboChain - 2;
				}

				enemy.GetComponent<Player> ().TakeDamage (dmg);

				comboChain++;

				HidePlayerUI ();
				EnemyTurn ();
		}

		private void WrongAnswer ()
		{
				comboChain = 0;
				HidePlayerUI ();
				EnemyTurn ();
		}

		private void EnemyTurn ()
		{
				current_state = BattleState.enemy_turn;
				timeRemaining = Random.Range (2, 2 + TimePerTurn);
		}
}
