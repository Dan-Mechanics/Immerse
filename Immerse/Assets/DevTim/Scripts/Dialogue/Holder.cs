using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// Holder of ScriptableObjects.
    /// </summary>
    public class Holder : MonoBehaviour
    {
        public List<DialogueEvent> Dialogue => dialogueEvents;
        public List<Actor> Actors => actors;
        public List<Prop> Props => props;

        public readonly Dictionary<string, DialogueEvent> DialogueDict = new Dictionary<string, DialogueEvent>();
        public readonly Dictionary<string, Actor> ActorsDict = new Dictionary<string, Actor>();
        public readonly Dictionary<string, Prop> PropsDict = new Dictionary<string, Prop>();

        [SerializeField] private List<DialogueEvent> dialogueEvents = default;
        [SerializeField] private List<Actor> actors = default;
        [SerializeField] private List<Prop> props = default;

        private void Awake()
        {
            for (int i = 0; i < actors.Count; i++)
            {
                Actor actor = actors[i];
                actor.Setup();
                actor.index = i;

                for (int j = 0; j < actor.dialogue.Length; j++)
                {
                    actor.dialogue[j].speaker = actor;
                    actor.dialogue[j].index = j;
                }
            }

            for (int i = 0; i < props.Count; i++)
            {
                Prop prop = props[i];
                prop.Setup();
                prop.index = i;

                prop.dialogue.speaker = prop;
                prop.dialogue.index = 0;
            }

            for (int i = 0; i < dialogueEvents.Count; i++)
            {
                dialogueEvents[i].Setup();
            }

            dialogueEvents.ForEach(x => DialogueDict.Add(x.name, x));
            actors.ForEach(x => ActorsDict.Add(x.name, x));
            props.ForEach(x => PropsDict.Add(x.name, x));
        }
    }
}
