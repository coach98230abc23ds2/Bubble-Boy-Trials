using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

    private float target;
    public static float Speed = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target > GetComponent<Slider>().value + 0.5f)
        {
            GetComponent<Slider>().value += Speed * Time.deltaTime;
        }
        else if (target < GetComponent<Slider>().value - 0.5f)
        {
            GetComponent<Slider>().value += Speed * Time.deltaTime;
        }
	}

    public void SetTarget(float newVal)
    {
        this.target = newVal;
    }
}
