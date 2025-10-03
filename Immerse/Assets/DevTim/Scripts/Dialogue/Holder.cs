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
            dialogueEvents.ForEach(x => DialogueDict.Add(x.name, x));
            actors.ForEach(x => ActorsDict.Add(x.name, x));
        }
    }
}
