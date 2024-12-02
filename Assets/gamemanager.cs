using UnityEngine;
using TMPro;  // Necesario para usar TextMeshPro

public class RotarMoneda : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public float velocidadRotacion = 100f;

    // Referencia al objeto TextMeshPro en la UI donde se mostrará el tiempo
    public TMP_Text tiempoText;

    // Update se llama una vez por frame
    void Update()
    {
        // Girar el objeto alrededor de su eje Y de forma indefinida
        transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0);

        // Mostrar el tiempo transcurrido en el texto de TextMeshPro
        MostrarTiempo();
    }

    // Método para actualizar el texto con el tiempo transcurrido
    void MostrarTiempo()
    {
        // Actualizar el texto con el tiempo transcurrido desde el inicio del juego
        tiempoText.text = "Tiempo: " + Time.time.ToString("F2") + " segundos";
    }
}
