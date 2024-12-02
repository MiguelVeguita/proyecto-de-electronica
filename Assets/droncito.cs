using System;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de tener la librería de UI
using TMPro;
public class JoystickControl : MonoBehaviour
{
    private SerialPort serialPort;
    private string serialPortName = "COM4"; // Cambia este nombre según el puerto correcto
    private int baudRate = 9600; // Debe coincidir con la velocidad del Arduino
    private bool portOpened = false;

    public GameObject objeto, panel, te;//eto a mover y rotar con los joysticks
    public int vida,Puntos;

    // Variables para los controles del Joystick
    [Header("Joystick 1 (Movimiento)")]
    public float moveSpeed = 5f; // Velocidad de movimiento en X y Z
    public float moveXSensitivity = 1f; // Sensibilidad de movimiento en X
    public float moveZSensitivity = 1f; // Sensibilidad de movimiento en Z

    [Header("Joystick 2 (Rotación y Movimiento Y)")]
    public float rotationSpeed = 100f; // Velocidad de rotación en el eje Y
    public float rotationSensitivity = 1f; // Sensibilidad de rotación en Y
    public float moveYSensitivity = 1f; // Sensibilidad de movimiento en Y (subir/abajo)

    //  public TMP_Text coordinatesText; // Variable para el texto de las coordenadas (UI)
    public TMP_Text puntaje,tiempo;

    private float xPos, yPos, zPos,tien; // Coordenadas actuales

    void Start()
    {
        // Intentar abrir el puerto serial
        serialPort = new SerialPort(serialPortName, baudRate);
        serialPort.ReadTimeout = 50; // Tiempo de espera para lectura

        try
        {
            serialPort.Open();
            portOpened = true;
            Debug.Log("Puerto serial abierto con éxito.");
        }
        catch (UnauthorizedAccessException)
        {
            Debug.LogError("Acceso denegado al puerto serial. Asegúrate de que el puerto no esté siendo usado por otro programa.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error al abrir el puerto serial: " + ex.Message);
        }
    }

    void Update()
    {
        if (portOpened)
        {
            try
            {
                // Si hay datos disponibles, leerlos
                if (serialPort.BytesToRead > 0)
                {
                    string data = serialPort.ReadLine().Trim();
                    string[] values = data.Split(',');

                    if (values.Length == 5)
                    {
                        // Asignar los valores leídos del puerto a las variables correspondientes
                        float moveX = Map(int.Parse(values[0]), 0, 1023, -1f, 1f) * moveXSensitivity;
                        float moveZ = Map(int.Parse(values[1]), 0, 1023, 1f, -1f) * moveZSensitivity;
                        float moveY = Map(int.Parse(values[3]), 0, 1023, -1f, 1f) * moveYSensitivity;
                        float rotation = Map(int.Parse(values[2]), 0, 1023, -1f, 1f) * rotationSensitivity;

                        // Invertir el movimiento en X (Joystick 1: derecha/izquierda)
                        moveX = -moveX;

                        // Invertir el movimiento en Z (Joystick 1: adelante/atrás)
                        moveZ = -moveZ;

                        // Invertir la rotación en Y (Joystick 2: giro)
                        rotation = -rotation;

                        // Movimiento en el eje X y Z (Joystick 1)
                        objeto.transform.Translate(moveX * moveSpeed * Time.deltaTime, 0f, moveZ * moveSpeed * Time.deltaTime);

                        // Movimiento en el eje Y (Joystick 2) - Subir o bajar
                        if (Mathf.Abs(moveY) > 0.1f)
                        {
                            objeto.transform.Translate(0f, moveY * moveSpeed * Time.deltaTime, 0f);
                        }

                        // Rotación en el eje Y (Joystick 2)
                        if (Mathf.Abs(rotation) > 0.1f)
                        {
                            objeto.transform.Rotate(0f, rotation * rotationSpeed * Time.deltaTime, 0f);
                        }

                        // Actualizar las coordenadas en las variables
                        xPos = objeto.transform.position.x;
                        yPos = objeto.transform.position.y;
                        zPos = objeto.transform.position.z;

                        // Mostrar las coordenadas en la UI
                        //coordinatesText.text = $"X: {xPos:F2}\nY: {yPos:F2}\nZ: {zPos:F2}";
                    }
                    else
                    {
                        Debug.LogWarning("Datos incorrectos recibidos desde el Arduino.");
                    }
                }
            }
            catch (TimeoutException)
            {
                Debug.LogWarning("Tiempo de espera agotado al leer desde el puerto serial.");
            }
            catch (Exception ex)
            {
                Debug.LogError("Error al leer desde el puerto serial: " + ex.Message);
            }
        }
        if (Puntos<= 90)
        {
            tien = tien + Time.deltaTime;
         
            
            
        }
        else
        {
            panel.SetActive(true);
            tiempo.text = tien.ToString("F2") + " segundos";
            te.SetActive(false);
        }
    }

    void OnApplicationQuit()
    {
        // Cerrar el puerto serial al finalizar la aplicación
        if (portOpened && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Puerto serial cerrado.");
        }
    }

    private float Map(int value, int inMin, int inMax, float outMin, float outMax)
    {
        return (float)(value - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "mon")
        {
            Puntos = Puntos + 10;
            puntaje.text = Puntos.ToString();
        }
        if (collision.gameObject.tag == "pared")
        {
            vida--;
        }

    }
}
