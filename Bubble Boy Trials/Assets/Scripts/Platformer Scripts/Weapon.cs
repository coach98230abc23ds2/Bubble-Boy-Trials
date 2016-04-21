using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Weapon : MonoBehaviour {

    private float fireRate = 0f;
    private float bulletSpeed = .5f;
//    public LayerMask whatToHit;
    public GameObject bulletPrefab;
    public Rigidbody2D force;
    public GameObject Clone;
    float timeToFire = 1f;
    GameObject firePoint;
    Transform bullet;

    private Animator m_anim;
    private bool left_dir;
    private float last_time_shot;

    // Use this for initialization
    void Awake () {
        firePoint = GameObject.Find("FirePoint");
        bullet = firePoint.transform;
        if (firePoint == null) {
            Debug.LogError ("No Firepoint Found");

        }
        m_anim = this.gameObject.GetComponent<Animator>();
        transform.gameObject.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezeRotation);
        PlatformerCharacter2D.can_move = true;
    }

    // Update is called once per frame
    void Update ()
    {
        PlatformerCharacter2D.can_move = true;
        transform.gameObject.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezeRotation) ;
        GameObject playr = GameObject.Find ("Player");
        if (fireRate == 0) {
            if (Input.GetButtonDown ("Fire1") && Time.time > last_time_shot + timeToFire) {
                
                if (playr.transform.localScale [0] < 0) {
//                    Debug.Log ("SHOOTING LEFT");
                   left_dir = true;
                   StartCoroutine(WaitToShoot(left_dir));
                } else if (playr.transform.localScale [0] > 0) {
//                    Debug.Log ("SHOOTING RIGHT");
                   left_dir = false;
                   StartCoroutine(WaitToShoot(left_dir));
                }
                last_time_shot = Time.time;
            }
        } else {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
               
                timeToFire = Time.time + 1 / fireRate;
                StartCoroutine(WaitToShoot(left_dir));
            if (playr.transform.localScale [0] < 0) {
//                Debug.Log ("SHOOTING LEFT");
                left_dir = true;
                StartCoroutine(WaitToShoot(left_dir));
            } else if (playr.transform.localScale [0] > 0) {
//                Debug.Log ("SHOOTING RIGHT");
                left_dir = false;
                StartCoroutine(WaitToShoot(left_dir));
            }
        }
    }

    IEnumerator WaitToShoot(bool left_dir){
        PlatformerCharacter2D.can_move = false;

        m_anim.SetTrigger("Attack");

        yield return new WaitForSeconds(.8f);

        if (left_dir){
            ShootLeft ();
        }else{
            ShootRight();
        }

    }




    void ShootLeft () {
        
//        Debug.Log("Shoot left");
        Vector3 firePointPosition = new Vector3 (bullet.position.x, bullet.position.y, bullet.position.z);
        Clone = (Instantiate(bulletPrefab, firePointPosition, Quaternion.identity)) as GameObject;
        force = Clone.GetComponent<Rigidbody2D>();
        force.isKinematic = true;
        force.velocity = transform.TransformDirection (-transform.right) * bulletSpeed;

    }
    void ShootRight () {
//        Debug.Log("Shoot right");
        Vector3 firePointPosition = new Vector3 (bullet.position.x, bullet.position.y, bullet.position.z);
        Clone = (Instantiate(bulletPrefab, firePointPosition, Quaternion.identity)) as GameObject;
        force = Clone.GetComponent<Rigidbody2D>();
        force.isKinematic = true;
        force.velocity = transform.TransformDirection (transform.right) * bulletSpeed;

    }
}