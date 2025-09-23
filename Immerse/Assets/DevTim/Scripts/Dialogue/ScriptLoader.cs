using System;
using UnityEngine;

namespace Immerse
{
    /// <summary>
    /// TODO: DEAD SPACE FIX.
    /// </summary>
    public class ScriptLoader : MonoBehaviour
    {
        private const string FIRSTS_SPLITTER = "///";
        private const string SECOND_SPLITTER = "=";
        
        [SerializeField] private TextAsset textAsset = default;
        [SerializeField] private DialogueEventHolder holder = default;
        [SerializeField] private bool loadScriptOnStartup = default;

        /// <summary>
        /// Start because this can be done whenever.
        /// </summary>
        private void Start()
        {
            if (!loadScriptOnStartup)
                return;
            
            string[] pages = textAsset.text.Split(FIRSTS_SPLITTER, StringSplitOptions.RemoveEmptyEntries);

            foreach (var page in pages)
            {
                if (!CleverSplit(page, SECOND_SPLITTER, out string[] components, 2))
                    continue;

                if (holder.Has(components[0]))
                    holder.GetByName(components[0]).script = components[1];
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
