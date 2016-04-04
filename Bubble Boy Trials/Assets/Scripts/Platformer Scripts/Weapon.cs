using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public float fireRate = 0;
    public float bulletSpeed = 20;
//    public LayerMask whatToHit;
    public GameObject bulletPrefab;
    public Rigidbody2D force;
    public GameObject Clone;
    float timeToFire = 0;
    GameObject firePoint;
    Transform bullet;

    // Use this for initialization
    void Awake () {
        firePoint = GameObject.Find("FirePoint");
        bullet = firePoint.transform;
        if (firePoint == null) {
            Debug.LogError ("No Firepoint Found");

        }

    }

    // Update is called once per frame
    void Update ()
    {
        GameObject playr = GameObject.Find ("Player");
        if (fireRate == 0) {
            if (Input.GetButtonDown ("Fire1")) {
                Debug.Log (Input.GetKey (KeyCode.LeftArrow));
                Debug.Log (Input.GetKey (KeyCode.RightArrow));
                //              PlatformerCharacter2D scr =  playr.GetComponent("PlatformerCharacter2D")
                Debug.Log (playr.transform.localScale [0]);
                if (playr.transform.localScale [0] < 0) {
                    Debug.Log ("SHOOTING LEFT");
                    ShootLeft ();
                } else if (playr.transform.localScale [0] > 0) {
                    Debug.Log ("SHOOTING RIGHT");
                    ShootRight ();
                }
            }
        } else {
            if (Input.GetButton ("Fire1") && Time.time > timeToFire)
                timeToFire = Time.time + 1 / fireRate;
            Debug.Log (Input.GetKey (KeyCode.LeftArrow));
            Debug.Log (Input.GetKey (KeyCode.RightArrow));
            Debug.Log (playr.transform.localScale [0]);
            if (playr.transform.localScale [0] < 0) {
                Debug.Log ("SHOOTING LEFT");
                ShootLeft ();
            } else if (playr.transform.localScale [0] > 0) {
                Debug.Log ("SHOOTING RIGHT");
                ShootRight ();
            }
        }
    }

    void ShootLeft () {
        Debug.Log("Shoot left");
        Vector3 firePointPosition = new Vector3 (bullet.position.x, bullet.position.y, bullet.position.z);
        Clone = (Instantiate(bulletPrefab, firePointPosition, Quaternion.identity)) as GameObject;
        force = Clone.GetComponent<Rigidbody2D>();
        force.isKinematic = true;
        force.velocity = transform.TransformDirection (-transform.right * bulletSpeed);

    }
    void ShootRight () {
        Debug.Log("Shoot right");
        Vector3 firePointPosition = new Vector3 (bullet.position.x, bullet.position.y, bullet.position.z);
        Clone = (Instantiate(bulletPrefab, firePointPosition, Quaternion.identity)) as GameObject;
        force = Clone.GetComponent<Rigidbody2D>();
        force.isKinematic = true;
        force.velocity = transform.TransformDirection (transform.right * bulletSpeed);

    }
}