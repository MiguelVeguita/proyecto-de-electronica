using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panelmanager : MonoBehaviour
{
    public GameObject PanelInstruciones;
    public GameObject PanelCreditos;
    public GameObject InsFalse;

  public void Panel1true()
  {
        PanelInstruciones.SetActive(true);
  }

    public void Panel1False()
    {
        PanelInstruciones.SetActive(false);
    }

    public void Panel2true()
    {
        InsFalse.SetActive(false);
        PanelCreditos.SetActive(true);
    }

    public void Panel2False()
    {
        InsFalse.SetActive(true);
        PanelCreditos.SetActive(false);
    }
}
