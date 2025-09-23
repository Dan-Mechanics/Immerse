using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    [CreateAssetMenu(fileName = "Actor", menuName = "Actor")]
    public class Actor : ScriptableObject
    {
        public Sprite icon;
        public string desc;
    }
}
