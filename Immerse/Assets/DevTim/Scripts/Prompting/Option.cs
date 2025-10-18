using System;
using UnityEngine;

namespace Immerse
{
    public enum Tag { None = 0, Cancel = 1, Blame = 2 }
    
    [Serializable]
    public struct Option
    {
        public string text;
        public Color color;
        public Tag tag;
        public bool optional;
        public Sprite icon;
    }
}
