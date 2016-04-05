using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlatformPlayer : MonoBehaviour {

    private int m_lives = 3; //player's remaining lives
    private int m_score = 0; //player's current score
    private bool m_invincible = false;
    private EnemySpawner spawner;

    public Text lives_text;
    public Text score_text; 

    private void RespawnPlayer(){
        m_lives = 3;
//        Transform player_transform = this.gameObject.transform;
        Vector2 temp = this.gameObject.transform.position;
        temp.x = 5.3f;
        temp.y = 20.0f;
        this.gameObject.transform.position = temp;

    }

    //increases score by the increment number
    public void GainScore (int increment){
        m_score += increment;
    }

    //decreases lives by 1
    public void LoseLives (){
        if (!m_invincible){
            m_lives--; 
            m_invincible = true;
            StartCoroutine(Invincible());
            m_invincible = false;
        }
    }

    IEnumerator Invincible(){
        yield return new WaitForSeconds(2);
    }

	// Use this for initialization
	void Start () {
        spawner = Camera.main.GetComponent<EnemySpawner>();
	}
	
	// Update is called once per frame
	void Update () {
        lives_text.text = "Lives: " + m_lives;
        score_text.text = "Score: " + m_score;
        if (this.gameObject.transform.position.y <= 5 
            || m_lives <= 0){
            RespawnPlayer();
        }
	}

    void OnTriggerEnter2D(Collider2D other){
        if (other.tag == "Head"){
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * 300);
            GameObject parent_enemy = other.gameObject.transform.parent.gameObject;
            spawner.RemoveFromDict(parent_enemy.name, parent_enemy.transform.position.x);
            Destroy(parent_enemy);
            GainScore(10);

        }else{
            if (!m_invincible){
                LoseLives();
            }
        }
    }

}
