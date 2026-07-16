using UnityEngine.UIElements;

namespace ECSMiniRPG.GUIModule.Elements
{
    public abstract class GUIPanel
    {
        private readonly VisualElement _root;

        protected GUIPanel(VisualElement root, bool isPlayerInputBlocked)
        {
            _root = root;
            IsPlayerInputBlocked = isPlayerInputBlocked;
        }

        public bool IsOpen { get; private set; }
        public bool IsPlayerInputBlocked { get; }

        public void Open()
        {
            if (!IsOpen)
            {
                _root.style.display = DisplayStyle.Flex;
                IsOpen = true;
                OnOpen();
            }
        }

        public void Close()
        {
            if (IsOpen)
            {
                _root.style.display = DisplayStyle.None;
                IsOpen = false;
                OnClose();
            }
        }

        public void Toggle()
        {
            var wasOpen = IsOpen;

            if (wasOpen)
            {
                Close();
            }

            if (!wasOpen)
            {
                Open();
            }
        }

        protected virtual void OnOpen()
        {
        }

        protected virtual void OnClose()
        {
        }
    }
}
