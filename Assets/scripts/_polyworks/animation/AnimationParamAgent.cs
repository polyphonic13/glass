namespace Polyworks
{
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class AnimationParamAgent : AnimationAgent
    {
        private Animator animator;
        private bool isPlaying;

        public void TogglePlayback(string param)
        {
            isPlaying = !isPlaying;
            animator.SetBool(param, isPlaying);
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }
    }
}
