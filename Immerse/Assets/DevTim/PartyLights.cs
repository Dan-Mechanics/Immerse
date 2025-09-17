using UnityEngine;

namespace Immerse
{
    [RequireComponent(typeof(Light))]
    public class PartyLights : MonoBehaviour
    {
        [SerializeField, Min(0.001f)] private float interval = default;
        private Light partyLight;

        private void Awake() => partyLight = GetComponent<Light>();
        private void Start() => InvokeRepeating(nameof(ChangeColor), 0f, interval);

        private void ChangeColor() => partyLight.color = Random.ColorHSV();
    }
}
