using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class Prompter : StateElement
    {
        public event Action<int> OnAnswer;
        
        [SerializeField] private Transform promptHolder = default;
        [SerializeField] private GameObject promptPrefab = default;
        [SerializeField] private float verticalSpacing;

        private readonly List<GameObject> spawned = new List<GameObject>();
        private readonly List<StateElement> promptBehaviours = new List<StateElement>();

        [Serializable]
        public struct Option
        {
            public string text;
            public Color color;
            public Sprite icon;
        }

        public void DisplayPrompt(Option[] options) 
        {
            DestroyPrompts();
            SpawnOptions(options);
        }

        private void GiveAnswer(int answer) => OnAnswer?.Invoke(answer);

        public override void Close()
        {
            base.Close();
            DestroyPrompts();
        }

        private void SpawnOptions(Option[] options) 
        {
            for (int i = 0; i < options.Length; i++)
            {
                SpawnOption(options[i], i);
            }

            promptBehaviours.ForEach(x => x.Open());
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
            GameObject go = Instantiate(promptPrefab, promptHolder);
            RectTransform rect = go.GetComponent<RectTransform>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            rect.anchoredPosition = Vector2.zero + (i * verticalSpacing * Vector2.down);

            go.GetComponent<Image>().color = option.color;
            go.GetComponentInChildren<TMP_Text>().text = option.text;
            go.GetComponentsInChildren<Image>()[1].sprite = option.icon;
            go.GetComponent<Button>().onClick.AddListener(delegate { GiveAnswer(i); });
            go.GetComponent<Lerper>().Send(false);

            promptBehaviours.AddRange(go.GetComponentsInChildren<StateElement>());

            spawned.Add(go);
        }

        /// <summary>
        /// Remove all previous.
        /// </summary>
        private void DestroyPrompts() 
        {
            OnAnswer?.GetInvocationList().ToList().ForEach(x => OnAnswer -= (Action<int>)x);

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
