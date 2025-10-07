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

        public readonly Dictionary<string, DialogueEvent> DialogueDict = new Dictionary<string, DialogueEvent>();
        public readonly Dictionary<string, Actor> ActorsDict = new Dictionary<string, Actor>();

        [SerializeField] private List<DialogueEvent> dialogueEvents = default;
        [SerializeField] private List<Actor> actors = default;

        private void Awake()
        {
            for (int i = 0; i < actors.Count; i++)
            {
                actors[i].index = i;

                for (int j = 0; j < actors[i].dialogue.Length; j++)
                {
                    actors[i].dialogue[j].actor = actors[i];
                }
            }
            
            dialogueEvents.ForEach(x => DialogueDict.Add(x.name, x));
            actors.ForEach(x => ActorsDict.Add(x.name, x));

        }
    }
}
