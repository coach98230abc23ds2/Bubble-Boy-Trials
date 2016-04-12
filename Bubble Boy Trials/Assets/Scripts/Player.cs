using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{

    public int Health = 30;
    private GameObject healthBar;
    public bool isDead = false;
    
    // Use this for initialization
    void Start()
    {
        healthBar = GameObject.Instantiate(Resources.Load("HealthBar"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        healthBar.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
        healthBar.GetComponent<Slider>().value = Health;
    }
	
    // Update is called once per frame
    void Update()
    {
        // canvas is null when in platformer mode
        GameObject canvas = GameObject.Find("Canvas");
        if (healthBar != null && canvas != null)
        {
            // centers the health bar above the player
            healthBar.transform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>(), false);
            RectTransform CanvasRect = canvas.GetComponent<RectTransform>();
            Vector2 ViewportPos = Camera.main.WorldToViewportPoint(transform.position);

            Vector2 ScreenPos = new Vector2(
                                    (ViewportPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f),
                                    (ViewportPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)
                                );

            healthBar.GetComponent<RectTransform>().anchoredPosition = ScreenPos + new Vector2(0, 50);
        }

    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (healthBar != null)
        {
            healthBar.GetComponent<Slider>().value = Health;
        }

        if (Health <= 0)
        {
            Destroy(healthBar);
            isDead = true;
        }
    }
}
