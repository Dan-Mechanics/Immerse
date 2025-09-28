using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class Prompter : MonoBehaviour
    {
        [SerializeField] private Transform background = default;
        [SerializeField] private GameObject promptPrefab = default;
        [SerializeField] private float verticalSpacing;

        private bool isPrompting;
        private int? chosen;

        private readonly List<GameObject> spawned = new List<GameObject>();
        private Option[] options;

        [System.Serializable]
        public class Option
        {
            public string text = "wdwd";
            public Color color = Color.white;
            public Sprite icon;
        }

        private void FixedUpdate()
        {
            background.gameObject.SetActive(isPrompting);
        }

        public int? GetAnswer(Option[] options) 
        {
            if (options != this.options)
                DestroyPrompts();
            
            if (isPrompting)
            {
                if (chosen != null)
                {
                    isPrompting = false;
                    DestroyPrompts();
                    int? temp = chosen;
                    chosen = null;

                    return temp;
                }

                return null;
            }
            else
            {
                this.options = options;
                isPrompting = true;
                SpawnOptions(options);

                return null;
            }
        }

        public void SetChosen(int chosen) => this.chosen = chosen;

        private void SpawnOptions(Option[] options) 
        {
            for (int i = 0; i < options.Length; i++)
            {
                SpawnOption(options[i], i);
            }
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
            go.GetComponent<Button>().onClick.AddListener(delegate { SetChosen(i); });
            go.GetComponent<LerperBase>().Send(false);
            spawned.Add(go);
        }

        private void DestroyPrompts() 
        {
            spawned.ForEach(x => x.GetComponent<Button>().onClick.RemoveAllListeners());
            spawned.ForEach(x => Destroy(x));
            spawned.Clear();
        }
    }
}
