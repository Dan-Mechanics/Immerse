using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Immerse
{
    public class StateHandler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> parents = default;
        [SerializeField] private GameObject startingParent = default;

        private readonly List<State> states = new List<State>();
        private State current;

        public class State
        {
            public GameObject parent;
            public List<StateElement> elements;

            public State(GameObject parent, List<StateElement> elements)
            {
                this.parent = parent;
                this.elements = elements;
            }

            public void Open()
            {
                print($"<b><color=green>Open() --> [{parent.name}].</color></b>");
                parent.SetActive(true);
                elements.ForEach(x => x.Open());
            }

            public void Close()
            {
                print($"<b><color=red>Close() --> [{parent.name}].</color></b>");
                elements.ForEach(x => x.Close());
                parent.SetActive(false);
            }
        }

        private void Awake()
        {
            foreach (GameObject parent in parents)
            {
                this.states.Add(new State(parent, new List<StateElement>()));
                List<StateElement> states = parent.GetComponentsInChildren<StateElement>().ToList();
                states.ForEach(x => this.states[^1].elements.Add(x));
                parent.SetActive(true);
            }
        }

        private void Start() 
        {
            CloseAll();
            Open(startingParent);
        }

        private void CloseAll()
        {
            states.ForEach(x => x.Close());
            current = null;
        }

        private void Update()
        {
            current?.elements.ForEach(x => x.DoFrame());
        }

        private void FixedUpdate()
        {
            current?.elements.ForEach(x => x.DoTick());
        }

        public void Open(GameObject parent)
        {
            if (current.parent == parent)
                return;
            
            current?.Close();
            
            if (parent == null || !parents.Contains(parent))
                return;

            foreach (State wrapper in states)
            {
                if (wrapper.parent != parent)
                    continue;

                wrapper.Open();
                current = wrapper;
                return;
            }
        }
    }
}
