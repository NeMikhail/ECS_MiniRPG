using UnityEngine.UIElements;

namespace ECSMiniRPG.GUIModule.Elements
{
    public sealed class PauseMenuElement : GUIPanel
    {
        private static readonly bool _isPlayerInputBlocked = true;

        public PauseMenuElement(VisualElement root) : base(root, _isPlayerInputBlocked)
        {
            Close();
        }
    }
}
