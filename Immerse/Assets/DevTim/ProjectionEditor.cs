using UnityEngine;

namespace Immerse
{
    public class ProjectionEditor : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float speed = 1f;
        [SerializeField] private float growthFactor = 1.005f;
        [SerializeField] private float shrinkageFactor = 0.995f;

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.A)) 
                transform.Translate(speed * Time.fixedDeltaTime * Vector3.left);

            if (Input.GetKey(KeyCode.D))
                transform.Translate(speed * Time.fixedDeltaTime * Vector3.right);

            if (Input.GetKey(KeyCode.W))
                transform.Translate(speed * Time.fixedDeltaTime * Vector3.up);

            if (Input.GetKey(KeyCode.S))
                transform.Translate(speed * Time.fixedDeltaTime * Vector3.down);

            if (Input.GetKey(KeyCode.Space))
                transform.localScale *= growthFactor;

            if (Input.GetKey(KeyCode.LeftShift))
                transform.localScale *= shrinkageFactor;

            // ===

            if (Input.GetKey(KeyCode.LeftArrow)) 
            {
                Vector3 scale = transform.localScale;
                scale.x *= shrinkageFactor;
                transform.localScale = scale;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 scale = transform.localScale;
                scale.x *= growthFactor;
                transform.localScale = scale;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                Vector3 scale = transform.localScale;
                scale.y *= growthFactor;
                transform.localScale = scale;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                Vector3 scale = transform.localScale;
                scale.y *= shrinkageFactor;
                transform.localScale = scale;
            }
        }
    }
}