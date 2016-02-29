using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public int Health = 100;
    public GameObject HealthPrefab;
    private GameObject healthtxt;
    
	// Use this for initialization
	void Start () {
            healthtxt = GameObject.Instantiate(HealthPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
            healthtxt.GetComponent<GUIText>().text = Health.ToString();
    }
	
	// Update is called once per frame
	void Update () {
            healthtxt.GetComponent<GUIText>().pixelOffset = Camera.main.WorldToScreenPoint(transform.position);
    }

    void TakeDamage(int damage)
    {
        Health -= damage;
        healthtxt.GetComponent<GUIText>().text = Health.ToString();
        if (Health < 0)
        {
            Destroy(this);
        }
    }
}
