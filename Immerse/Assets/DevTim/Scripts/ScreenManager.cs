using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> screens = default;
        [SerializeField] private GameObject startingScreen = default;

        private GameObject current;

        private void Start() => Open(startingScreen);

        public void Open(GameObject go)
        {
            screens.ForEach(x => x.SetActive(false));

            if (go != null && !screens.Contains(go))
                return;

            current = go;
            current.SetActive(true);
        }
    }
}
