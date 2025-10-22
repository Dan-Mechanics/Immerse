using System;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public class History : MonoBehaviour
    {
        public Action<List<Prop>> OnNewProps;
        public Action<Dictionary<Actor, List<int>>> OnNewActorHistory;

        [SerializeField] private InteractionHandler interactionHandler = default;
        private readonly List<Prop> props = new List<Prop>();
        private readonly Dictionary<Actor, List<int>> actorHistory = new Dictionary<Actor, List<int>>();

        private void Awake()
        {
            interactionHandler.OnInteractWithDialogue += OnInteractWithDialogue;
        }

        private void OnDestroy()
        {
            interactionHandler.OnInteractWithDialogue -= OnInteractWithDialogue;
        }

        private void OnInteractWithDialogue(DialogueEvent dialogue)
        {
            if (dialogue.owner is Prop prop && !Has(prop))
                Add(prop);

            if (dialogue.owner is Actor actor)
                Add(actor, dialogue.index);
        }

        public bool Has(Prop prop) => props.Contains(prop); 

        public void Add(Prop prop)
        {
            props.Add(prop);
            OnNewProps?.Invoke(props);
        }

        public void Add(Actor actor, int index)
        {
            actorHistory.TryAdd(actor, new List<int>());

            if (!actorHistory[actor].Contains(index))
                actorHistory[actor].Add(index);

            OnNewActorHistory?.Invoke(actorHistory);
        }
    }
}
