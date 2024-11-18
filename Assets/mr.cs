using System.IO.Ports;
using UnityEngine;


public class DroneController1 : MonoBehaviour
{
    // Configuraci�n del puerto serial
    SerialPort serialPort = new SerialPort("COM4", 9600); // Cambia "COM3" por el puerto correspondiente

    // Sensibilidad para movimientos del dron
    public float movementSensitivity = 0.01f;
    public float altitudeSensitivity = 0.05f;

    // Componentes y variables del dron
    private Vector3 dronePosition;
    private float altitude = 0;

    void Start()
    {
        serialPort.Open();
        dronePosition = transform.position; // Posici�n inicial del dron
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

        // Actualizar la posici�n del dron
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
        float potValue = float.Parse(values[2]) * altitudeSensitivity;
        bool buttonPressed = values[3] == "1";

        // Movimiento del dron en el eje X y Z (movimiento horizontal)
        dronePosition += new Vector3(xValue, 0, yValue) * Time.deltaTime;

        // Ajustar altura usando el potenci�metro
        altitude = potValue;

        // Efecto al presionar el bot�n del joystick
        if (buttonPressed)
        {
            // Acci�n adicional como parpadeo de luces o vibraci�n
            Debug.Log("Bot�n presionado - efecto especial");
        }
    }

    private void OnApplicationQuit()
    {
        // Cerrar el puerto serial al cerrar la aplicaci�n
        if (serialPort.IsOpen)
            serialPort.Close();
    }
}