using UnityEngine;

namespace Immerse
{
    public class SceneSetup : MonoBehaviour
    {
        [SerializeField] [Min(1)] private int fps = default;
        [SerializeField] private float fixedUpdateRate = default;
        [SerializeField] private bool mouseLocked = default;

        private void Start()
        {
            Application.targetFrameRate = fps;
            Time.fixedDeltaTime = 1f / fixedUpdateRate;
            Cursor.visible = !mouseLocked;
            Cursor.lockState = mouseLocked ? CursorLockMode.Locked : CursorLockMode.None;

            Destroy(gameObject);
        }
    }
}