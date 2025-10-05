using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Test : MonoBehaviour
{
    [SerializeField] private string[] m_Keywords;
    [SerializeField] private AudioClip clip;

    private KeywordRecognizer recognizer;

    private void Start()
    {
        recognizer = new KeywordRecognizer(m_Keywords);
        recognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizer.Start();
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        if (args.text == "computer")
            GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
