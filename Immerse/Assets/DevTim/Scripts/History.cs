using System.Collections.Generic;
using UnityEngine;

namespace Immerse
{
    public class History : MonoBehaviour
    {
        [SerializeField] private Sprite[] dialogueSprites = default;


        public void AddDialogue(DialogueEvent dialogue)
        {

        }

        public void AddActor(Actor actor)
        {

        }

        public class Some
        {
            public List<int> dialogues;
            public bool hasObject;

            public void Add(int index)
            {
                for (int i = dialogues.Count - 1; i >= 0; i--)
                {
                    // IDK IF THIS IS GOOD.
                    if (index > dialogues[i])
                        dialogues.Insert(i, index);
                }
            }
        }
    }
}
