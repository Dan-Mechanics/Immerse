using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// Or we use indices but it all depends on what the output is from the scanner basically.
    /// Doesnt really matter.
    /// 
    /// It think this just needs to be asis, 
    /// and then have anotehr one with the dictionary. called like scanner response or something idk.
    /// </summary>
    public class Holder : MonoBehaviour
    {
        public List<DialogueEvent> DialogueEvents => dialogueEvents;
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
