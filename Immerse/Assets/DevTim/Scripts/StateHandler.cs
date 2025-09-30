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
            public GameObject go;
            public List<Behaviour> behaviours;

            public State(GameObject go, List<Behaviour> behaviours)
            {
                this.go = go;
                this.behaviours = behaviours;
            }

            public void Open()
            {
                go.SetActive(true);
                behaviours.ForEach(x => x.EnterState());
            }

            public void Close()
            {
                behaviours.ForEach(x => x.ExitState());

                if (go != null) 
                    go.SetActive(false);
            }
        }

        private void Awake()
        {
            foreach (GameObject go in parents)
            {
                this.states.Add(new State(go, new List<Behaviour>()));
                List<Behaviour> states = go.GetComponentsInChildren<Behaviour>().ToList();
                states.ForEach(x => this.states[^1].behaviours.Add(x));
                go.SetActive(true);
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
            current?.behaviours.ForEach(x => x.DoFrame());
        }

        private void FixedUpdate()
        {
            current?.behaviours.ForEach(x => x.DoTick());
        }

        public void Open(GameObject go)
        {
            current?.Close();
            
            if (go == null)
                return;

            if (!parents.Contains(go))
                return;

            foreach (State wrapper in states)
            {
                if (wrapper.go != go)
                    continue;

                wrapper.Open();
                current = wrapper;
                return;
            }
        }
    }
}
