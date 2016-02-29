using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public int Health = 100;

	// Use this for initialization
	void Start () {
        GetComponent<GUIText>().text = Health.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        GUIText text = GetComponent<GUIText>();
        text.pixelOffset = Camera.main.WorldToScreenPoint(transform.position);
    }

    void TakeDamage(int damage)
    {
        Health -= damage;
        GetComponent<GUIText>().text = Health.ToString();
        if (Health < 0)
        {
            Destroy(this);
        }
    }
}
