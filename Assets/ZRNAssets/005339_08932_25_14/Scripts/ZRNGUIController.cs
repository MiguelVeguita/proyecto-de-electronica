using UnityEngine;
using System.Collections;
public class ZRNGUIController : MonoBehaviour
{
    private float hSliderValue = 0.0f;
    private bool menuVisible = false;
    private int operateCameraNumber;
    private bool shadowOn;

    [SerializeField]
    GameObject[] QueryObjects;

    int previousCameraNumber;

    string playModeString;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<CameraController>().ChangeCamera(0);
        operateCameraNumber = 0;
        previousCameraNumber = 0;

        this.GetComponent<AmbientController>().changeShadow(true);
        shadowOn = true;

        changePlayMode(0);
        SetQueryChan(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (menuVisible == true)
        {
            GUI.BeginGroup(new Rect(50, 50, Screen.width - 100, 270));

            GUI.Box(new Rect(0, 0, Screen.width - 100, 270), "Menú de Control");

            if (GUI.Button(new Rect(Screen.width - 100 - 50, 10, 40, 40), "X"))
            {
                menuVisible = false;
            }

            // ---------- Control del Cielo ----------
            GUI.Label(new Rect(20, 40, 100, 30), "Control del Cielo");
            if (GUI.Button(new Rect(20, 60, 80, 40), "Soleado"))
            {
                this.GetComponent<AmbientController>().changeSkybox(AmbientController.AmbientType.AMBIENT_SKYBOX_SUNNY);
            }
            if (GUI.Button(new Rect(110, 60, 80, 40), "Nublado"))
            {
                this.GetComponent<AmbientController>().changeSkybox(AmbientController.AmbientType.AMBIENT_SKYBOX_CLOUD);
            }
            if (GUI.Button(new Rect(200, 60, 80, 40), "Nocturno"))
            {
                this.GetComponent<AmbientController>().changeSkybox(AmbientController.AmbientType.AMBIENT_SKYBOX_NIGHT);
            }

            // ---------- Control de Sombras ----------
            GUI.Label(new Rect(20, 110, 100, 30), "Control de Sombras");
            if (GUI.Button(new Rect(20, 130, 80, 40), "Activar/Desactivar"))
            {
                if (shadowOn == false)
                {
                    this.GetComponent<AmbientController>().changeShadow(true);
                    shadowOn = true;
                }
                else
                {
                    this.GetComponent<AmbientController>().changeShadow(false);
                    shadowOn = false;
                }
            }
            GUI.Label(new Rect(120, 130, 100, 30), "Hora");
            hSliderValue = GUI.HorizontalSlider(new Rect(120, 155, 150, 30), hSliderValue, 0.0f, 100.0f);
            this.GetComponent<AmbientController>().rotateAmbientLight(hSliderValue);

            // ---------- Control de Efectos ----------
            GUI.Label(new Rect(20, 180, 100, 30), "Control de Efectos");
            if (GUI.Button(new Rect(20, 200, 80, 40), "Ninguno"))
            {
                this.GetComponent<AmbientController>().changeParticle(AmbientController.ParticleType.PARTICLE_NONE);
            }
            if (GUI.Button(new Rect(110, 200, 80, 40), "Viento"))
            {
                this.GetComponent<AmbientController>().changeParticle(AmbientController.ParticleType.PARTICLE_WIND);
            }
            if (GUI.Button(new Rect(200, 200, 80, 40), "Lluvia"))
            {
                this.GetComponent<AmbientController>().changeParticle(AmbientController.ParticleType.PARTICLE_RAIN);
            }

            // ---------- Control de Cámara ----------
            if (operateCameraNumber < 100)
            {
                GUI.Label(new Rect(400, 40, 100, 30), "Control de Cámara");
                if (GUI.Button(new Rect(400, 60, 50, 40), "<---"))
                {
                    operateCameraNumber--;
                    if (operateCameraNumber < 0)
                    {
                        operateCameraNumber = this.GetComponent<CameraController>().targetCameraNames.Count - 1;
                        previousCameraNumber = operateCameraNumber;
                    }
                }
                if (GUI.Button(new Rect(600, 60, 50, 40), "--->"))
                {
                    operateCameraNumber++;
                    if (operateCameraNumber > this.GetComponent<CameraController>().targetCameraNames.Count - 1)
                    {
                        operateCameraNumber = 0;
                        previousCameraNumber = operateCameraNumber;
                    }
                }
                GUI.Label(new Rect(460, 60, 140, 20), this.GetComponent<CameraController>().targetCameraNames[operateCameraNumber]);
                if (GUI.Button(new Rect(450, 80, 150, 20), "Cambiar"))
                {
                    this.GetComponent<CameraController>().ChangeCamera(operateCameraNumber);
                    previousCameraNumber = operateCameraNumber;
                    SetQueryChan(0);
                }
            }

            // ---------- Control de Movimiento ----------
            GUI.Label(new Rect(400, 110, 100, 30), "Control de Movimiento");
            if (GUI.Button(new Rect(400, 130, 80, 40), "Normal"))
            {
                operateCameraNumber = previousCameraNumber;
                this.GetComponent<CameraController>().ChangeCamera(operateCameraNumber);
                SetQueryChan(0);
                changePlayMode(0);
            }
            if (GUI.Button(new Rect(490, 130, 80, 40), "Exploración"))
            {
                SetQueryChan(1);
                changePlayMode(1);
            }
            if (GUI.Button(new Rect(580, 130, 80, 40), "Conducción"))
            {
                InitAICars();
                changePlayMode(2);
            }

            GUI.EndGroup();
        }
        else
        {
            // ---------- Botón del Menú ----------
            if (GUI.Button(new Rect(Screen.width - 120, 20, 100, 40), "Menú"))
            {
                menuVisible = true;
            }
        }

        // Mostrar Modo de Juego
        GUI.Box(new Rect(30, Screen.height - 60, 250, 50), "Modo = " + playModeString);
    }

    void SetQueryChan(int QueryNumber)
    {
        foreach (GameObject targetQueryChan in QueryObjects)
        {
            targetQueryChan.SetActive(false);
        }
        QueryObjects[QueryNumber].SetActive(true);
        if (QueryNumber == 1)
        {
            QueryObjects[QueryNumber].GetComponent<FlyThroughController>().InitQuery();
            operateCameraNumber = 100;
            this.GetComponent<CameraController>().ChangeCamera(operateCameraNumber);
        }
    }

    void changePlayMode(int modeNumber)
    {
        switch (modeNumber)
        {
            case 0:
                playModeString = "Normal";
                break;
            case 1:
                playModeString = "Exploración\nTeclas: z = desacelerar, x = acelerar\nFlechas: arriba, abajo, izquierda, derecha";
                break;
            case 2:
                playModeString = "Conducción";
                break;
        }
    }

    void InitAICars()
    {
        GameObject[] targetAICars = GameObject.FindGameObjectsWithTag("AICars");
        foreach (GameObject targetAICar in targetAICars)
        {
            targetAICar.GetComponent<AICarMove>().InitAICar();
            operateCameraNumber = 200;
            this.GetComponent<CameraController>().ChangeCamera(operateCameraNumber);
        }
    }
}