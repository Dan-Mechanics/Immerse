using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(Prop), menuName = nameof(Prop))]
    public class Prop : Actor
    {
        public override void OnValidate()
        {
            base.OnValidate();
            if(dialogue.Length > 1)
            {
                Debug.LogWarning("Props should not have more than 1 dialogue.");
                dialogue = new DialogueEvent[1] { dialogue[0] };
            }
        }
    }
}
