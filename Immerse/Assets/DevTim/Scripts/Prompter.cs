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
        [SerializeField] private float verticalSpacing = default;
        [SerializeField] private bool debugMakePromptsInvisible = default; 

        private readonly List<GameObject> spawned = new List<GameObject>();
        private readonly List<StateElement> promptBehaviours = new List<StateElement>();
        private Keyboard keyboardInput;
        private int optionsCount;
        private readonly char[] alpha = { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };

        /// <summary>
        /// OR DO 1234.
        /// </summary>
        private class Keyboard 
        {
            public int DoFrame(char[] alpha)
            {
                for (int i = 0; i < alpha.Length; i++)
                {
                    if (Input.GetKeyDown(alpha[i].ToString()))
                        return i;
                }

                return -1;
            }
        }

        [Serializable]
        public struct Option
        {
            public string text;
            public Color color;
            public Sprite icon;
        }

        public void DisplayPrompt(Option[] options) 
        {
            if (debugMakePromptsInvisible)
                Debug.LogWarning("debugMakePromptsInvisible = true");
            
            DestroyPrompts();
            SpawnOptions(options);

            optionsCount = options.Length;
            string[] phrases = new string[optionsCount];
            for (int i = 0; i < phrases.Length; i++)
            {
                phrases[i] = options[i].text;
            }

            keyboardInput = new Keyboard();
        }

        private void GiveAnswer(int answer) 
        {
            if (answer < 0 || answer >= optionsCount)
                return;

            OnAnswer?.Invoke(answer);
        }

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
            
            if (keyboardInput != null)
                GiveAnswer(keyboardInput.DoFrame(alpha));

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
            rect.anchoredPosition = Vector2.up * verticalSpacing + (i * verticalSpacing * Vector2.down);

            go.GetComponent<Image>().color = option.color;
            go.GetComponentInChildren<TMP_Text>().text = alpha[i].ToString().ToUpperInvariant() + ": " + option.text;
            go.GetComponentsInChildren<Image>()[1].sprite = option.icon;
            go.GetComponent<Button>().onClick.AddListener(delegate { GiveAnswer(i); });
            go.GetComponent<Lerper>().Send(false);

            promptBehaviours.AddRange(go.GetComponentsInChildren<StateElement>());

            spawned.Add(go);

            if (debugMakePromptsInvisible)
                go.SetActive(false);
        }

        /// <summary>
        /// Remove all previous.
        /// </summary>
        private void DestroyPrompts() 
        {
            OnAnswer?.GetInvocationList().ToList().ForEach(x => OnAnswer -= (Action<int>)x);
            keyboardInput = null;

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
