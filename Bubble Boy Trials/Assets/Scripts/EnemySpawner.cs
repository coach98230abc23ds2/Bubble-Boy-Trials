using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject minion;
	private float timer = 0.0f;
	private Vector3 randPos;
	private Vector3 randPos2;
	private float newX; 
	private float newY;
	private int count = 0;
	// public BasicMovement BM;

	// private bool hasActiveObjects (){
	// 	GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
	// 	foreach(GameObject go in allObjects){
	// 		if (go.activeInHierarchy){
	// 			return true;
	// 		}
	// 	}
	// 	return false;
	// }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

			// spawns a normal space invader every 1.1 secs
			if(count < 5){
				timer += Time.deltaTime;
				if(timer > 1.0f){
					newX = (float)Random.Range(5,10)+ 3.0f;
					newY = (float)Random.Range (0,3)+ 1.0f;
					randPos = new Vector3(newX, newY, 0);
					Instantiate(minion, randPos, Quaternion.identity);
					timer = 0.0f;
					count++;
				}
			}
				
		}
}
