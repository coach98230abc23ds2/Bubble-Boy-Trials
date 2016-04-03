using UnityEngine;
using System.Collections;

public class PlatformPlayer : MonoBehaviour {

    private int m_lives = 3; //player's remaining lives
    private int m_score = 0; //player's current score

    //increases score by the increment number
    public void GainScore (int increment){
        m_score += increment;
    }

    //decreases lives by 1
    public void LoseLives (){
        m_lives--; 
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
