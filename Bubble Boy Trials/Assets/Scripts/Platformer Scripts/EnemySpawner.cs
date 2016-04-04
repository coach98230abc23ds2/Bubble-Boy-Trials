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
	private Vector2 spawn_position; //enemy spawn position

	public GameObject[] enemy_types; //stores type of enemies
	private GameObject[] active_objects; //active game objects
	private float[] enemy_1_positions = new float[]{24.59f, 84.0f, 118.0f}; //all possible x-coordinate spawning points for grounded minions
    private float[] enemy_2_positions = new float[]{48.0f, 136.7f}; //all possible x-coordinate spawning points for flying minions
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
			Debug.Log("DICT:" + pair.Key + ", " + ListToStr(pair.Value));
		}
	}

	//initializes curr_enemy_positions Dictionary
	private void InitializeEnemyPos (){
		curr_enemy_positions = new Dictionary<string, List<float>>();
		foreach (GameObject enemy in enemy_types){
//            curr_enemy_positions.Add(enemy.name, new List<float>());
			curr_enemy_positions.Add(enemy.name + "(Clone)", new List<float>());
		}
	}


    // adds given position to the dictionary
	private void AddToDict (string name, float pos){
		try {
			curr_enemy_positions[name].Add(pos); 
		} catch (Exception e){
            Debug.Log("key is not in dictionary yet. Will add:" + e.Message);
			curr_enemy_positions.Add(name, new List<float>(new float[]{pos}));
		}
	}


	//removes a position from the given enemy's list of positons.
	public void RemoveFromDict (string name, float pos){
		try{
			curr_enemy_positions[name].Remove(pos); 
		}catch (Exception e){
            Debug.Log("key is not in dictionary yet. Cannot remove:" + e.Message);
		}
	}

    private bool ShouldInstantiate (string enemy_key, float enemy_pos){
		if (active_objects.Length != 0){
            
            Debug.Log ("ENEMY_NAME: " + enemy_key);
            Debug.Log(ListToStr(curr_enemy_positions[enemy_key]));
            Debug.Log (System.Convert.ToString(enemy_pos));
            if (!(curr_enemy_positions[enemy_key].Contains(enemy_pos))){
                Debug.Log("can instantiate enemy");
    			return true;
            }else{
                return false;
			}
			
        }else{
            return true;
        }
   
	}

	//spawns minions in designated positions if the player is in viewable range
	private void SpawnMinions (){
        float[] curr_positions = new float[Mathf.Max(enemy_1_positions.Length, 
                                                   enemy_2_positions.Length)];

        foreach (GameObject enemy in enemy_types){        
            switch (enemy.name)
            {
                case "enemy1":
                case "enemy1(Clone)":
                    curr_positions = enemy_1_positions;
                    break;
                case "enemy2":
                case "enemy2(Clone)":
                    curr_positions = enemy_2_positions;
                    break;
            }
            for (int i = 0; i< curr_positions.Length; i++){
                if (Mathf.Abs(m_player_pos - curr_positions[i]) <= 15.0f)
    			{  
                    if (ShouldInstantiate(enemy.name + "(Clone)", curr_positions[i])){
    					spawn_position = new Vector2 (curr_positions[i], 25);
    					GameObject new_enemy = (GameObject) Instantiate(enemy, 
                                    spawn_position, enemy.transform.rotation);
                        AddToDict(new_enemy.name, curr_positions[i]);
    				}
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
		if(m_timer > 1.0f){
			 Debug.Log("ACTIVE OBJECTS:"+ ArrayToStr(active_objects));
			PrintDict();
			m_player_pos = player.transform.position.x;
			SpawnMinions();
			m_timer = 0.0f;
		}
	}

}
