using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class Prompter : Behaviour
    {
        public event Action<int> OnAnswer;
        
        [SerializeField] private Transform background = default;
        [SerializeField] private GameObject promptPrefab = default;
        [SerializeField] private float verticalSpacing;

        private readonly List<GameObject> spawned = new List<GameObject>();
        private readonly List<Behaviour> promptBehaviours = new List<Behaviour>();

        [Serializable]
        public struct Option
        {
            public string text;
            public Color color;
            public Sprite icon;
        }

        public void DisplayPrompt(Option[] options) 
        {
            /*foreach (var listener in OnAnswer.GetInvocationList())
            {
                OnAnswer -= (Action<int>)listener;
            }*/

            DestroyPrompts();
            SpawnOptions(options);
        }

        private void GiveAnswer(int answer) => OnAnswer?.Invoke(answer);

        public override void ExitState()
        {
            base.ExitState();
            DestroyPrompts();
        }

        private void SpawnOptions(Option[] options) 
        {
            for (int i = 0; i < options.Length; i++)
            {
                SpawnOption(options[i], i);
            }

            promptBehaviours.ForEach(x => x.EnterState());
        }

        public override void DoFrame()
        {
            base.DoFrame();
            promptBehaviours.ForEach(x => x.DoFrame());
        }

        public override void DoTick()
        {
            base.DoTick();
            promptBehaviours.ForEach(x => x.DoTick());
        }

        private void SpawnOption(Option option, int i)
        {
            GameObject go = Instantiate(promptPrefab, background);
            RectTransform rect = go.GetComponent<RectTransform>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            rect.anchoredPosition = Vector2.zero + (i * verticalSpacing * Vector2.down);

            go.GetComponent<Image>().color = option.color;
            go.GetComponentInChildren<TMP_Text>().text = option.text;
            go.GetComponentsInChildren<Image>()[1].sprite = option.icon;
            go.GetComponent<Button>().onClick.AddListener(delegate { GiveAnswer(i); });
            go.GetComponent<Lerper>().Send(false);

            promptBehaviours.AddRange(go.GetComponentsInChildren<Behaviour>());

            spawned.Add(go);
        }

        /// <summary>
        /// Don't have to call ExitState() on the promptBehaviours
        /// because the destroy() handles that.
        /// </summary>
        private void DestroyPrompts() 
        {
            foreach (GameObject go in spawned)
            {
                go.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(go);
            }

            promptBehaviours.Clear();
            spawned.Clear();
        }
    }
}
