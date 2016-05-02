using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Weapon : MonoBehaviour {

    private float fireRate = 0f;
    private float bulletSpeed = 9.6f;

//    public LayerMask whatToHit;
    public GameObject bulletPrefab;
    public Rigidbody2D force;
    public GameObject Clone;
    public AnimationClip attack_clip;
    public bool can_attack = true;
    float timeToFire = 1f;
    GameObject firePoint;
    Transform bullet;

    private Animator m_anim;
    private bool left_dir;
    private float last_time_shot;

    // Use this for initialization
    void Awake () 
    {
        firePoint = GameObject.Find("FirePoint");
        bullet = firePoint.transform;
        if (firePoint == null) 
        {
            Debug.LogError ("No Firepoint Found");
        }
        m_anim = this.gameObject.GetComponent<Animator>();
        transform.gameObject.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezeRotation);
    }

    // Update is called once per frame
    void Update ()
    {
        transform.gameObject.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezeRotation) ;
        GameObject playr = GameObject.Find ("Player");
        if (can_attack)
        {
            if (fireRate == 0) 
            {
                if (Input.GetButtonDown ("Fire1") && Time.time > last_time_shot + timeToFire) 
                {
                    
                    if (playr.transform.localScale [0] < 0) 
                    {
                       left_dir = true;
                       StartCoroutine(WaitToShoot(left_dir));
                    } 
                    else if (playr.transform.localScale [0] > 0) 
                    {
                       left_dir = false;
                       StartCoroutine(WaitToShoot(left_dir));
                    }
                    last_time_shot = Time.time;
                }
            } 
            else 
            {
                // need to fix this
                if (Input.GetButton("Fire1") && Time.time > timeToFire){
                    timeToFire = Time.time + 1 / fireRate;
                    StartCoroutine(WaitToShoot(left_dir));
                }
                if (playr.transform.localScale [0] < 0) 
                {
                    left_dir = true;
                    StartCoroutine(WaitToShoot(left_dir));
                } 
                else if (playr.transform.localScale [0] > 0) 
                {
                    left_dir = false;
                    StartCoroutine(WaitToShoot(left_dir));
                }
            }
        }
    }

    IEnumerator WaitToShoot(bool left_dir)
    {

        m_anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attack_clip.length - 1f);

        if (left_dir)
        {
            ShootLeft ();
        }
        else
        {
            ShootRight();
        }

    }

    void ShootLeft () 
    {
        Vector3 firePointPosition = new Vector3 (bullet.position.x, bullet.position.y, bullet.position.z);
        Clone = (Instantiate(bulletPrefab, firePointPosition, Quaternion.identity)) as GameObject;
        Clone.transform.localScale += new Vector3(.5f,.5f,.5f);
        force = Clone.GetComponent<Rigidbody2D>();
        force.isKinematic = true;
        force.velocity = transform.TransformDirection (-transform.right) * bulletSpeed;

    }
    void ShootRight () 
    {
        Vector3 firePointPosition = new Vector3 (bullet.position.x, bullet.position.y, bullet.position.z);
        Clone = (Instantiate(bulletPrefab, firePointPosition, Quaternion.identity)) as GameObject;
        Clone.transform.localScale += new Vector3(.5f,.5f,.5f);
        force = Clone.GetComponent<Rigidbody2D>();
        force.isKinematic = true;
        force.velocity = transform.TransformDirection (transform.right) * bulletSpeed;

    }
}