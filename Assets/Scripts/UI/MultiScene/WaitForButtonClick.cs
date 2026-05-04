using UnityEngine;
using UnityEngine.UIElements;

namespace UI.MultiScene
{
    public class WaitForButtonClick : CustomYieldInstruction
    {
        private bool _clicked;

        public WaitForButtonClick(Button button)
        {
            button.clicked += () => _clicked = true;
        }
        
        public override bool keepWaiting => !_clicked;
    }
}