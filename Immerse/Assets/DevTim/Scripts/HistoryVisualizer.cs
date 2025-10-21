using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Immerse
{
    public class HistoryVisualizer : MonoBehaviour
    {
        [SerializeField] private History history = default;
        
        [SerializeField] private GameObject prefab = default;
        [SerializeField] private Transform anchor = default;
        [SerializeField] private Sprite conversationSprite = default;
        [SerializeField] private float verticalOffset = default;
        [SerializeField] private float horizontalOffset = default;
        [SerializeField] private int actorHistoryStartingIndex = default;
        [SerializeField] private Color a = Color.white;
        [SerializeField] private Color b = Color.white;
        [SerializeField, Min(2)] private int maxExpectedAnswers = default;

        private readonly List<Image> propImages = new List<Image>();
        private readonly List<Image> actorHistoryImages = new List<Image>();

        private void Awake()
        {
            history.OnNewProps += ShowProps;
            history.OnNewActorHistory += ShowActorHistory;
        }

        private void OnDestroy()
        {
            history.OnNewProps -= ShowProps;
            history.OnNewActorHistory -= ShowActorHistory;
        }

        public void ShowProps(List<Prop> props)
        {
            propImages.ForEach(x => Destroy(x.gameObject));
            propImages.Clear();

            for (int i = 0; i < props.Count; i++)
            {
                Image image = SpawnImage(propImages.Count * verticalOffset * Vector2.up);
                image.sprite = props[i].icon;
                propImages.Add(image);
            }
        }

        public void ShowActorHistory(Dictionary<Actor, List<int>> actorHistory)
        {
            actorHistoryImages.ForEach(x => Destroy(x.gameObject));
            actorHistoryImages.Clear();

            int latIndex = actorHistoryStartingIndex;
            foreach (KeyValuePair<Actor, List<int>> pair in actorHistory)
            {
                // HEADER.
                actorHistoryImages.Add(SpawnImage(latIndex * horizontalOffset * Vector2.right));
                actorHistoryImages[^1].sprite = pair.Key.icon;

                // CONVERSATIONS.
                foreach (int longIndex in pair.Value)
                {
                    Image image = SpawnImage(new Vector2(latIndex * horizontalOffset, (1 + longIndex) * verticalOffset));
                    image.sprite = conversationSprite;
                    image.color = Color.Lerp(a, b, Mathf.Clamp01((float)longIndex / (maxExpectedAnswers - 1)));
                    actorHistoryImages.Add(image);
                }
                
                latIndex++;
            }
        }

        private Image SpawnImage(Vector2 pos)
        {
            GameObject go = Instantiate(prefab, anchor);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.anchoredPosition = pos;

            return go.GetComponent<Image>();
        }
    }
}
