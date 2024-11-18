using UnityEngine;
using System.IO.Ports;

public class DroneController : MonoBehaviour
{
    // Configuración del puerto serial
    SerialPort serialPort = new SerialPort("COM4", 9600); // Cambia "COM4" por el puerto correspondiente

    // Sensibilidad para movimientos del dron
    public float movementSensitivity = 0.01f;
    public float altitudeSensitivity = 0.05f;

    // Componentes y variables del dron
    private Vector3 dronePosition;
    private float altitude = 0;

    void Start()
    {
        serialPort.Open();
        dronePosition = transform.position; // Posición inicial del dron
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                ProcessData(data);
            }
            catch (System.Exception) { }
        }

        // Actualizar la posición del dron con el joystick y el potenciómetro
        transform.position = new Vector3(dronePosition.x, altitude, dronePosition.z);
    }

    void ProcessData(string data)
    {
        // Separar los datos recibidos (x, y, pot, btn)
        string[] values = data.Split(',');
        if (values.Length < 4) return;

        // Convertir valores recibidos a flotantes
        float xValue = (float.Parse(values[0]) - 512) * movementSensitivity;
        float yValue = (float.Parse(values[1]) - 512) * movementSensitivity;
        float potValue = float.Parse(values[2]) * altitudeSensitivity; // Usamos potenciómetro para la altitud
        bool buttonPressed = values[3] == "1";

        // Invertir el valor de yValue para que el movimiento hacia adelante sea positivo
        yValue = -yValue;

        // Movimiento horizontal del dron (ejes X y Z) con el joystick
        dronePosition += new Vector3(xValue, 0, yValue) * Time.deltaTime;

        // Ajuste de altitud con el potenciómetro
        altitude = potValue;

        // Acción adicional al presionar el botón del joystick
        if (buttonPressed)
        {
            // Acción adicional, como un efecto visual o retroalimentación
            Debug.Log("Botón presionado - efecto especial");
        }
    }

    private void OnApplicationQuit()
    {
        // Cerrar el puerto serial al cerrar la aplicación
        if (serialPort.IsOpen)
            serialPort.Close();
    }
}
