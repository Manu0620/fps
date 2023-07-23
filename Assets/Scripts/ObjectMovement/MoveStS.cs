using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStS : MonoBehaviour
{
     public float speed = 2.0f;        // Velocidad de movimiento
    public float distance = 2.0f;    // Distancia máxima a la que se moverá de su posición inicial
    private Vector3 startPosition;    // Posición inicial

    void Start()
    {
        startPosition = transform.position;  // Guarda la posición inicial
    }

    void Update()
    {
        // Usa Mathf.PingPong para que el valor oscile entre 0 y 'distance'
        float movement = Mathf.PingPong(Time.time * speed, distance);
        
        // Mueve el objeto de lado a lado basándose en su posición inicial
        transform.position = startPosition + new Vector3(movement, 0, 0);
    }
}
