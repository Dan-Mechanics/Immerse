using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class ArduinoCom : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM5", 9600);

    void Start()
    {
        sp.Open();
        sp.ReadTimeout = 100;
        if (sp.IsOpen)
        {
            print("connected");
        }
    }

    void Update()
    {
        try
        {
            Debug.Log(sp.BytesToRead);
        }
        catch (System.Exception)
        {
        }
    }

    private void OnApplicationQuit()
    {
        sp.Close();
    }
}