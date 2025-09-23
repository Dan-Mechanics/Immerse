using System.Collections.Generic;
using UnityEngine;
using System;

namespace Immerse
{
    public class Voicelines
    {
        private readonly Dictionary<string, AudioClip> nameToClip = new Dictionary<string, AudioClip>();
        private readonly Queue<string> pendingVoicelines = new Queue<string>();

        private AudioSource source;
        private float next;
        private readonly char splitToken;

        public Voicelines(char splitToken)
        {
            this.splitToken = splitToken;
        }

        public void Setup(AudioSource source, List<AudioClip> clips)
        {
            this.source = source;
            clips.ForEach(x => nameToClip.Add(x.name.ToLowerInvariant(), x));
        }

        public void Update()
        {
            if (Time.time < next)
                return;

            if (pendingVoicelines.Count <= 0)
                return;

            Play(pendingVoicelines.Dequeue());
        }

        private void Play(string name)
        {
            if (!nameToClip.ContainsKey(name))
                return;

            AudioClip clip = nameToClip[name];
            source.PlayOneShot(clip);
            next = Time.time + clip.length;
        }

        public void Add(string name) 
        {
            pendingVoicelines.Enqueue(name);
        }

        public void Force(string name)
        {
            source.Stop();
            Play(name);
        }

        public void Mix(string name)
        {
            Play(name);
        }

        /// <summary>
        /// Dev Tim: you could use reflection to make
        /// this cleaner but that has a performance cost.
        /// </summary>
        public void Filter(string command)
        {
            command = command.ToLowerInvariant();
            command = command.Replace(" ", string.Empty);

            if (!command.Contains(splitToken))
            {
                Add(command);
                return;
            }

            string[] split = command.Split(splitToken, 3, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length != 2)
            {
                Debug.LogWarning($"'{command}' is invalid.");
                return;
            }

            switch (split[1])
            {
                case "add":
                    Add(split[0]);
                    break;

                case "force":
                    Force(split[0]);
                    break;
                case "mix":
                    Mix(split[0]);
                    break;
                default:
                    Debug.LogWarning($"I don't know '{split[1]}'...");
                    break;
            }
        }
    }
}
