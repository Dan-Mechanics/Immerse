using System;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public class History : MonoBehaviour
    {
        public Action<List<Prop>> OnNewProps;
        public Action<Dictionary<Actor, List<int>>> OnNewActorHistory;

        private readonly List<Prop> props = new List<Prop>();
        private readonly Dictionary<Actor, List<int>> actorHistory = new Dictionary<Actor, List<int>>();

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
