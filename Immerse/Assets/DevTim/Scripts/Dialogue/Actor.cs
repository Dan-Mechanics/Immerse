using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = nameof(Actor), menuName = nameof(Actor))]
    public class Actor : Entity
    {
        public Prop prop;
        public DialogueEvent[] dialogue;

        public override void Setup()
        {
            base.Setup();
            icon = Resources.Load<Sprite>(name);
        }
    }
}
