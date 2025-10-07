using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Immerse
{
    [Serializable]
    public class Questions
    {
        public List<Replace> replaceCalls;
        
        public Prompter.Option[] options;
        
        
        [Serializable]
        public struct Replace
        {
            public string target;
            public string with;
        }   
    }
}
