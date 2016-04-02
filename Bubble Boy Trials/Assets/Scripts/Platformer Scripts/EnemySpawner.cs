using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject player;

	private float m_timer = 0.0f;
	private float m_new_x; 
	private float m_new_y;
	private int m_count = 0;
	private float m_player_pos = 4.9f; //players current global view position
	private Vector2 spawn_position; // enemy spawn position

	public GameObject[] enemy_types; //stores type of enemies
	private GameObject[] active_objects; // active game objects
	private float[] x_positions = new float[]{5.0f, 84.0f, 118.0f, 134.0f}; // all possible x-coordinate spawning points for minions
	private Dictionary<string, List<float>> curr_enemy_positions; /* dictionary that holds all active instantiated enemies 
																																		& their current positions */

	//to be implemented																																
	// private bool IsObjectInRange (){

	// }																													

	private string ListToStr (List<float> list){
		string x = "[";
		foreach (float e in list){
			x += System.Convert.ToString(e) + "," ;
		}
		x += "]";
		return x;
	}

	private string ArrayToStr (GameObject[] arr){
		string x = "[";
		foreach (GameObject e in arr){
			x += e.name + ",";
		}
		x += "]";
		return x;
	}

	private void PrintDict (){
		foreach (KeyValuePair<string,List<float>> pair in curr_enemy_positions){
			Debug.Log(pair.Key + ", " + ListToStr(pair.Value));
		}
	}

	//initializes curr_enemy_positions Dictionary
	private void InitializeEnemyPos (){
		curr_enemy_positions = new Dictionary<string, List<float>>();
		foreach (GameObject enemy in enemy_types){
			curr_enemy_positions.Add(enemy.name + "(Clone)", new List<float>());
		}
	}

	private void AddToDict (string name, float pos){
		try{
			curr_enemy_positions[name].Add(pos); 
		}catch (Exception e){
			Debug.Log("key is not in dictionary yet. Will add.");
			curr_enemy_positions.Add(name, new List<float>(new float[]{pos}));
		}
	}


	//removes a position from the given enemy's list of positons.
	public void RemoveFromDict (string name, float pos){
		try{
			curr_enemy_positions[name].Remove(pos); 
		}catch (Exception e){
			Debug.Log("key is not in dictionary yet. Cannot remove.");
		}
	}

	private bool ShouldInstantiate (float enemy_pos){
		if (active_objects.Length != 0){
			foreach (GameObject enemy in active_objects){
				if (!curr_enemy_positions[enemy.name].Contains(enemy_pos)){
					return true;
				}
			}
			return false;
		}
		return true;

	}

	//spawns minions in designated positions if the player is in viewable range
	private void SpawnMinions (){
		for (int i = 0; i< x_positions.Length; i++){
			int r = UnityEngine.Random.Range (0, enemy_types.Length);
			GameObject rand_enemy = enemy_types[r];		

			if (Mathf.Abs(m_player_pos - x_positions[i]) <= 30.0f)
			{
				if (ShouldInstantiate(x_positions[i])){
					spawn_position = new Vector2 (x_positions[i], 25);
					GameObject new_enemy = (GameObject) Instantiate(rand_enemy, spawn_position, rand_enemy.transform.rotation);
					AddToDict(new_enemy.name, x_positions[i]);
				}
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		InitializeEnemyPos();
	}
	
	// Update is called once per frame
	void Update () {
		active_objects = UnityEngine.GameObject.FindGameObjectsWithTag("Enemy"); 
		active_objects.Distinct();
		m_timer += Time.deltaTime;
		if(m_timer > 6.0f){
			// Debug.Log(ArrayToStr(active_objects));
			PrintDict();
			m_player_pos = player.transform.position.x;
			SpawnMinions();
			m_timer = 0.0f;
		}
	}

}
