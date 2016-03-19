using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{

		public int Health = 30;
		public GameObject HealthPrefab;
		public Sprite IdleSprite;
		public Sprite HurtSprite;
		public Sprite DeadSprite;
		private float animCycle;
		private GameObject healthBar;
    
		// Use this for initialization
		void Start ()
		{
				healthBar = GameObject.Instantiate (HealthPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				healthBar.transform.SetParent (GameObject.Find ("Canvas").GetComponent<RectTransform> (), false);
				healthBar.GetComponent<Slider> ().value = Health;
				GetComponent<SpriteRenderer> ().sprite = IdleSprite;
		}
	
		// Update is called once per frame
		void Update ()
		{
				RectTransform CanvasRect = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
				Vector2 ViewportPos = Camera.main.WorldToViewportPoint (transform.position);

				Vector2 ScreenPos = new Vector2 (
						(ViewportPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f),
						(ViewportPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)
				);

				healthBar.GetComponent<RectTransform> ().anchoredPosition = ScreenPos + new Vector2 (0, 50);

				// run through animations
				animCycle -= Time.deltaTime;
				if (animCycle < 0) {
						animCycle = 0.5f;
						if (GetComponent<SpriteRenderer> ().sprite == HurtSprite)
								GetComponent<SpriteRenderer> ().sprite = IdleSprite;
						else
								GetComponent<SpriteRenderer> ().sprite = HurtSprite;
				}
		}

		void TakeDamage (int damage)
		{
				Health -= damage;
				healthBar.GetComponent<Slider> ().value = Health;
				if (Health <= 0) {
						gameObject.SetActive (false);
				} else if (Health <= 10) {
						GetComponent<SpriteRenderer> ().sprite = DeadSprite;
				}
		}
}
