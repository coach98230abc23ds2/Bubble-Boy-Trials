using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public float Damage = 10;
	public LayerMask notToHit;

	float timeToFire = 0;
	Transform firePoint;

	// Use this for initialization
	void Awake () {
		firePoint = transform.FindChild ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("No FirePoint = WHAT?!");

				}
	
	}
	
	// Update is called once per frame
	void Update () {
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
								Shoot ();
			}
		} 
		else {
			if (Input.GetButton ("Fire1") && Time.time > timeToFire)
				timeToFire = Time.time + 1 / fireRate;
					Shoot ();
		}
	}
	
		void Shoot () {
			Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
			RaycastHit2D hit = Physics2D.Raycast (firePointPosition, Vector2.right, 100, notToHit);
			Debug.Log ("Test Shot");
	}

}
