using UnityEngine;
using UnityEngine.SceneManagement;

namespace Immerse
{
    public class EndScreen : StateElement
    {
        private const char REPLACE_TOKEN = '@';
        
        [SerializeField] private TextWriter text = default;
        [SerializeField] private string writing = default;

        /// <summary>
        /// This needs to be called while inactive.
        /// </summary>
        public void SetWon(bool won)
        {
            writing = writing.Replace(REPLACE_TOKEN.ToString(), won ? "right" : "wrong");
            writing += won ? "!" : "...";
            text.SetColor(won ? Color.green : Color.red);

            writing = writing.ToUpperInvariant();
            text.SetStartupMessage(writing);
        }

        public override void DoFrame()
        {
            base.DoFrame();

            if (!Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
