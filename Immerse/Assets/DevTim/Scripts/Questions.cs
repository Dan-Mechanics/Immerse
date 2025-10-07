using System;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    [Serializable]
    public struct Questions
    {
        public int Length => options.Length;
        
        public int processingAppliesTo;
        public Color baseColor;
        public float saturation;
        public Color colorA;
        public Color colorB;
        public Actor actor;
        public List<Replace> replaceCalls;
        public Prompter.Option[] options;

        public void Process() 
        {
            if (processingAppliesTo > options.Length)
                processingAppliesTo = options.Length;
            
            for (int i = 0; i < processingAppliesTo; i++)
            {
                options[i].color = Color.Lerp(baseColor, Color.Lerp(colorA, colorB, (float)i / options.Length), saturation);
                options[i].icon = actor.icon;
                for (int j = 0; j < replaceCalls.Count; j++)
                {
                    options[i].text = options[i].text.Replace(replaceCalls[j].target, replaceCalls[j].with);
                }
            }
        }
        
        [Serializable]
        public struct Replace
        {
            public string target;
            public string with;
        }   
    }
}
