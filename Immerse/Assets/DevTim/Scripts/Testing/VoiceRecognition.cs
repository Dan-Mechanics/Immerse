using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Windows.Speech;

namespace Immerse
{
    public class VoiceRecognition : StateElement
    {
        public event Action<int> OnAnswer;
        private readonly Dictionary<string[], KeywordRecognizer> dict = new Dictionary<string[], KeywordRecognizer>();
        private string[] currentPhrases;

        private void OnPhraseRecognized(PhraseRecognizedEventArgs speech)
        {
            print($"{speech.text} @ {speech.confidence}");
            speech.semanticMeanings.ToList().ForEach(x => print(x));

            for (int i = 0; i < currentPhrases.Length; i++)
            {
                if (currentPhrases[i] != speech.text)
                    continue;

                OnAnswer?.Invoke(i);
                return;
            }
        }

        public void Begin(string[] phrases)
        {
            currentPhrases = phrases;

            //  WE ALREADY HAVE IT.
            if (dict.ContainsKey(phrases))
            {
                dict[phrases].OnPhraseRecognized += OnPhraseRecognized;
                dict[phrases].Start();
            }
            else
            {
                KeywordRecognizer recognizer = new KeywordRecognizer(phrases);
                recognizer.OnPhraseRecognized += OnPhraseRecognized;
                recognizer.Start();
                dict.Add(phrases, recognizer);
            }
        }

        public override void Close()
        {
            base.Close();
            if (currentPhrases == null || !dict.ContainsKey(currentPhrases))
                return;

            dict[currentPhrases].Stop();
            dict[currentPhrases].OnPhraseRecognized -= OnPhraseRecognized;
        }
    }
}
