using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Microphone.html
    /// </summary>
    public class Microphone : MonoBehaviour
    {
        //Script MicrophoneInput
        /*void Start()
        {
            if (device == null) device = Microphone.devices[0];
            audio.clip = Microphone.Start(MicrophoneInput.device, true, 999, 44100);
            while (!(Microphone.GetPosition(device) > 0))
            { }
            audio.Play();
        }
        void Update()
        {

            if (!active) return;
            float[] data = new float[735];
            audio.GetOutputData(data, 0);
            //take the median of the recorded samples
            ArrayList s = new ArrayList();
            foreach (float f in data)
            {
                s.Add(Mathf.Abs(f));
            }
            s.Sort();
            Volume = (float)s[735 / 2];
        }*/
    }
}
