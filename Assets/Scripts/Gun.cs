using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

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
    public GameObject Panel;
    public GameObject ganastePanel;

    private int puntos = 0;
    public int municionesActuales = 30;
    public int municionesMaximas = 30;
    public TextMeshProUGUI textoPuntos, textoMunicion;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;        // Oculta el puntero del ratón
        Cursor.lockState = CursorLockMode.Locked;
        textoMunicion.text = "30/30";
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

        if (municionesActuales <= 0)  
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GameOver();
        }

        if(puntos == 80)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Ganaste();
        }
    }

    private void Fire()
    {
        if(municionesActuales > 0)
        {
            municionesActuales--;
            setTextMun();
            
            RaycastHit hit;
            //muzzleFlush.Play();
            if(Physics.Raycast(Camera.position, Camera.forward, out hit, range))
            {
                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                // Verifica si el objeto impactado tiene la etiqueta "Objetivo1"
                if (hit.collider.gameObject.CompareTag("Objetivo1"))
                {
                    // Desactiva el objeto impactado
                    hit.collider.gameObject.SetActive(false);
                    // Incrementa el contador de puntos
                    puntos = puntos + 5;
                    setPuntos();  // Actualiza el texto de puntos
                }

                // Verifica si el objeto impactado tiene la etiqueta "Objetivo1"
                if (hit.collider.gameObject.CompareTag("Objetivo2"))
                {
                    // Desactiva el objeto impactado
                    hit.collider.gameObject.SetActive(false);
                    // Incrementa el contador de puntos
                    puntos = puntos + 10;
                    setPuntos();  // Actualiza el texto de puntos
                }

                Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
                GameObject impact =  Instantiate(inmpactEffect, hit.point, impactRotation);
                Destroy(impact, 5);
            }
        }
    }

    void PlayGunshotSound()
    {
        if(audioSource != null && gunshotSound != null)
        {
            audioSource.PlayOneShot(gunshotSound);
            
        }
    }

    void setPuntos(){
        textoPuntos.text = "Puntos: " + puntos.ToString();
    }

    void setTextMun()
    {
        textoMunicion.text = municionesActuales.ToString() + "/" + municionesMaximas.ToString();
    }

    void GameOver()
    {
        // Desactiva cualquier funcionalidad del jugador
        // Por ejemplo, puedes desactivar el script de movimiento o el script del arma
        this.enabled = false; // Desactiva el script de Gun

        // Muestra el panel de "Game Over"
        Panel.SetActive(true);
    }

    void Ganaste()
    {
        // Desactiva cualquier funcionalidad del jugador
        // Por ejemplo, puedes desactivar el script de movimiento o el script del arma
        this.enabled = false; // Desactiva el script de Gun

        // Muestra el panel de "Game Over"
        ganastePanel.SetActive(true);
    }

    public void RestartGame()
    {
        // Aquí usaremos la función de reinicio de Unity para reiniciar el nivel
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
