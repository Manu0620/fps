using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    InputAction shoot;
    public AudioSource audioSource; 
    public AudioClip gunshotSound;
    public Transform Camera;
    public float range = 20;
    public float impactForce = 150;
    public int fireRate = 10;
    private float nextTimeToFire = 0 ;

    //public ParticleSystem muzzleFlush;
    public GameObject inmpactEffect;

    private int contador;

    // Start is called before the first frame update
    void Start()
    {
        shoot = new InputAction("Shoot", binding: "<mouse>/leftButton");

        shoot.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        bool isShooting = shoot.ReadValue<float>() == 1;

        if(isShooting && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Fire();
            PlayGunshotSound();
        }
    }

    private void Fire()
    {
        RaycastHit hit;
        //muzzleFlush.Play();
        if(Physics.Raycast(Camera.position, Camera.forward, out hit, range))
        {
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject impact =  Instantiate(inmpactEffect, hit.point, impactRotation);
            Destroy(impact, 5);
        }
    }

    void PlayGunshotSound()
    {
        if(audioSource != null && gunshotSound != null)
        {
            audioSource.PlayOneShot(gunshotSound);
            
        }
    }
}
