using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WebcamUnity
{
    public class RectMovement : MonoBehaviour
    {
     //   [SerializeField] private RectTransform rect = default;
        [SerializeField, Min(0.1f)] private float pixelsPerSecond = default;

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A)) 
                transform.Translate(pixelsPerSecond * Time.fixedDeltaTime * Vector3.left);

            if (Input.GetKey(KeyCode.D))
                transform.Translate(pixelsPerSecond * Time.fixedDeltaTime * Vector3.right);

            if (Input.GetKey(KeyCode.W))
                transform.Translate(pixelsPerSecond * Time.fixedDeltaTime * Vector3.up);

            if (Input.GetKey(KeyCode.S))
                transform.Translate(pixelsPerSecond * Time.fixedDeltaTime * Vector3.down);

            if (Input.GetKey(KeyCode.Space))
                transform.localScale *= 1.005f * Vector2.one;

            if (Input.GetKey(KeyCode.LeftShift))
                transform.localScale *= 0.995f * Vector2.one;

            // ===

            if (Input.GetKey(KeyCode.LeftArrow)) 
            {
                Vector3 scale = transform.localScale;
                scale.x *= 0.995f;
                transform.localScale = scale;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 scale = transform.localScale;
                scale.x *= 1.005f;
                transform.localScale = scale;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Vector3 scale = transform.localScale;
                scale.y *= 1.005f;
                transform.localScale = scale;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                Vector3 scale = transform.localScale;
                scale.y *= 0.995f;
                transform.localScale = scale;
            }
        }

        /*private Vector3 pass(Vector3 wdwd)
        { return wdwd; }*/
    }
}