using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class monedas : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public float velocidadRotacion = 100f;

    // Update se llama una vez por frame
    void Update()
    {
        // Girar el objeto alrededor de su eje Y de forma indefinida
        transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "dron")
        {
            //this.gameObject.IsDestroyed();
            Destroy(this.gameObject);
        }
    }


}
