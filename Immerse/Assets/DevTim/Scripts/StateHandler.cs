using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Immerse
{
    public class StateHandler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> parents = default;
        [SerializeField] private GameObject startingParent = default;

        private readonly List<Wrapper> wrappers = new List<Wrapper>();
        private Wrapper current;

        public class Wrapper
        {
            public GameObject go;
            public List<State> states;

            public Wrapper(GameObject go, List<State> states)
            {
                this.go = go;
                this.states = states;
            }

            public void Open()
            {
                go.SetActive(true);
                states.ForEach(x => x.EnterState());
            }

            public void Close()
            {
                states.ForEach(x => x.ExitState());

                if (go != null) 
                    go.SetActive(false);
            }
        }

        private void Awake()
        {
            foreach (GameObject go in parents)
            {
                wrappers.Add(new Wrapper(go, new List<State>()));
                List<State> states = go.GetComponentsInChildren<State>().ToList();
                states.ForEach(x => wrappers[^1].states.Add(x));
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
            wrappers.ForEach(x => x.Close());
            current = null;
        }

        private void Update()
        {
            current?.states.ForEach(x => x.DoFrame());
        }

        private void FixedUpdate()
        {
            current?.states.ForEach(x => x.DoTick());
        }

        public void Open(GameObject go)
        {
            current?.Close();
            
            if (go == null)
                return;

            if (!parents.Contains(go))
                return;

            foreach (Wrapper wrapper in wrappers)
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
