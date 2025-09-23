using System.Collections;
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
    public class DialogueEventHolder : MonoBehaviour
    {
        public List<DialogueEvent> DialogueEvents => dialogueEvents;
        [SerializeField] private List<DialogueEvent> dialogueEvents = default;
        private readonly Dictionary<string, DialogueEvent> dict = new Dictionary<string, DialogueEvent>();

        private void Awake()
        {
            dialogueEvents.ForEach(x => dict.Add(x.name, x));
        }

        public bool Has(string name) 
        {
            bool result = dict.ContainsKey(name);

            if (!result)
                Debug.LogWarning($"'{name}' not found, this is a problem !! - Dev Tim".ToUpperInvariant());

            return result;
        }

        public DialogueEvent GetByName(string name) => dict[name];
    }
}
