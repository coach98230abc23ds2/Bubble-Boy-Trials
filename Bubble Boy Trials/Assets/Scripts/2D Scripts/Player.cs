using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{

		public int Health = 30;
		public Sprite IdleSprite;
		public Sprite HurtSprite;
		public Sprite DeadSprite;

		private float animCycle;
		private GameObject healthBar;
		private bool isDead = false;
    
		// Use this for initialization
		void Start ()
		{
				healthBar = GameObject.Instantiate (Resources.Load("HealthBar"), new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
				healthBar.transform.SetParent (GameObject.Find ("Canvas").GetComponent<RectTransform> (), false);
				healthBar.GetComponent<Slider> ().value = Health;
				GetComponent<SpriteRenderer> ().sprite = IdleSprite;
		}
	
		// Update is called once per frame
		void Update ()
		{
				// centers the health bar above the player                    
				RectTransform CanvasRect = GameObject.Find ("Canvas").GetComponent<RectTransform> ();
				Vector2 ViewportPos = Camera.main.WorldToViewportPoint (transform.position);

				Vector2 ScreenPos = new Vector2 (
						(ViewportPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f),
						(ViewportPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)
				);

				healthBar.GetComponent<RectTransform> ().anchoredPosition = ScreenPos + new Vector2 (0, 50);

				// run through animations
				animCycle -= Time.deltaTime;
				if (isDead) {
						GetComponent<SpriteRenderer> ().sprite = DeadSprite;
				} else if (animCycle < 0) {
						animCycle = 0.5f;
						if (GetComponent<SpriteRenderer> ().sprite == HurtSprite)
								GetComponent<SpriteRenderer> ().sprite = IdleSprite;
						else
								GetComponent<SpriteRenderer> ().sprite = HurtSprite;
				}
		}

		public void TakeDamage (int damage)
		{
				Health -= damage;

				if (healthBar != null) {
						healthBar.GetComponent<Slider> ().value = Health;
				}

				if (Health <= 0) {
					isDead = true;
				}
		}
}
