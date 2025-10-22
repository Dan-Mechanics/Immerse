using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Immerse
{
    public class StateHandler : MonoBehaviour
    {
        [SerializeField] private Transform root = default;
        [SerializeField] private GameObject startingParent = default;

        private readonly List<GameObject> parents = new List<GameObject>();
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
            for (int i = 0; i < root.childCount; i++)
            {
                parents.Add(root.GetChild(i).gameObject);
            }

            foreach (GameObject parent in parents)
            {
                parent.SetActive(true);
                states.Add(new State(parent, parent.GetComponentsInChildren<StateElement>().ToList()));
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
            if (current == null)
                return;

            Clean();
            current.elements.ForEach(x => x.DoFrame());
        }

        /// <summary>
        /// This is to avoid a bug where 
        /// the preview is deleted.
        /// </summary>
        private void FixedUpdate()
        {
            if (current == null)
                return;

            Clean();
            for (int i = 0; i < current.elements.Count; i++)
            {
                current.elements[i].DoTick();
            }
        }

        private void Clean()
        {
            for (int i = current.elements.Count - 1; i >= 0; i--)
            {
                if (current.elements[i] == null)
                    current.elements.RemoveAt(i);
            }
        }

        public void Open(GameObject parent)
        {
            if (current != null && current.parent == parent)
                return;
            
            current?.Close();
            
            if (parent == null || !parents.Contains(parent))
                return;

            foreach (State state in states)
            {
                if (state.parent != parent)
                    continue;

                // NOTE: THE ORDER OF THIS IS VERY IMPORTANT.
                current = state;
                state.Open();
                return;
            }
        }
    }
}
