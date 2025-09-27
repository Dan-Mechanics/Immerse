using UnityEngine;
using UnityEngine.SceneManagement;

namespace Immerse
{
    public class SceneUtils : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            if (!Input.GetKey(KeyCode.Mouse4))
                return;

            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
