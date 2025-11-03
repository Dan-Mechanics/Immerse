using UnityEngine;
using System.Collections;
using System.IO.Ports;
using Immerse;

public class ArduinoCom : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM5", 9600);
    [SerializeField] Scanner scanner;
    string savedLine;

    void Start()
    {
        sp.DtrEnable = true;
        sp.ReadTimeout = 100;

        sp.Open();
        if (sp.IsOpen)
        {
            print("connected");
        }
    }

    void Update()
    {
        try
        {
            //Debug.Log(sp.BytesToRead);
            string line = sp.ReadLine();
            if (savedLine != line)
            {
                ChangeRoom(line);
                savedLine = line;
            }
        }
        catch (System.Exception)
        {
        }
    }

    private void ChangeRoom(string line)
    {
        switch (line) {
            case "R1":
                scanner.Jeremy();
                Debug.Log("Arduino: " + line);
                break;
            case "R2":
                scanner.Leonardo();
                Debug.Log("Arduino: " + line);
                break;
            case "R3":
                scanner.Marc();
                break;
            case "R4":
                scanner.Vivienne();
                break;
        }
    }

    private void OnApplicationQuit()
    {
        sp.Close();
    }
}