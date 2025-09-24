using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public class SimpleThunder : MonoBehaviour
    {
        [SerializeField] private Camera cam = default;
        [SerializeField] private AudioSource source = default;
        [SerializeField] private float speed = default;
        
        private float value;

        private void Start()
        {
            Thunder();            
        }

        private void FixedUpdate()
        {
            value -= Time.fixedDeltaTime * speed;
            if (value < 0f)
                value = 0f;

            cam.backgroundColor = Color.Lerp(Color.black, Color.white, value);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                value = 1f;
        }

        private void Thunder() 
        {
            value = 1f;
            Invoke(nameof(Thunder), Random.Range(0f, 10f));
        }
    }
}
