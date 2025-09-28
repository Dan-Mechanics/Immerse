using System;
using UnityEngine;

namespace Immerse
{
    public class ScriptLoader : MonoBehaviour
    {
        private const string FIRSTS_SPLITTER = "///";
        private const string SECOND_SPLITTER = "=";
        
        [SerializeField] private TextAsset textAsset = default;
        [SerializeField] private Holder holder = default;
        [SerializeField] private bool loadScriptOnStartup = default;

        private void Start()
        {
            if (!loadScriptOnStartup)
                return;
            
            string[] pages = textAsset.text.Split(FIRSTS_SPLITTER, StringSplitOptions.RemoveEmptyEntries);

            foreach (string page in pages)
            {
                if (!CleverSplit(page, SECOND_SPLITTER, out string[] components, 2))
                    continue;

                if (holder.DialogueEvents.ContainsKey(components[0]))
                    holder.DialogueEvents[components[0]].script = components[1];
            }
        }

        /// <summary>
        /// https://www.geeksforgeeks.org/c-sharp/c-sharp-string-trim-method/
        /// </summary>
        private bool CleverSplit(string input, string splitter, out string[] split, int expectedCount = 0)
        {
            split = expectedCount <= 0 ?
                input.Split(splitter, StringSplitOptions.RemoveEmptyEntries) :
                input.Split(splitter, expectedCount + 1, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].Trim();
            }

            return split.Length == expectedCount || expectedCount <= 0;
        }
    }
}
