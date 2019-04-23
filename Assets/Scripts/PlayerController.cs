using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public GameObject shot2;
    public Transform shotSpawn2;
    public float fireRate;

    private Rigidbody rb;
    private AudioSource audioSource;

    private float nextFire;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if((Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space)) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            Instantiate(shot2, shotSpawn2.position, shotSpawn2.rotation);
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
        );

        rb.rotation = Quaternion.Euler(rb.velocity.x * tilt, 90f, 0.0f);
        

	}
}
