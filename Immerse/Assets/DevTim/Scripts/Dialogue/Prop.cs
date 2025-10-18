using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(Prop), menuName = nameof(Prop))]
    public class Prop : Entity
    {
        public DialogueEvent dialogue;

        public override void Setup()
        {
            base.Setup();
            icon = Resources.Load<Sprite>(name);
        }
    }
}
