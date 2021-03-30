using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace UnityEngine.UI
{
    /// <summary>
    /// A custom button adds other click functionality
    /// </summary>
    [AddComponentMenu("UI/ExButton", 30)]
    public class ExButton : Button {

        [Serializable]
        /// <summary>
        /// Function definition for a button click event.
        /// </summary>
        public new class ButtonClickedEvent : UnityEvent {}

        // Event delegates triggered on click.
        // [SerializeField]
        // private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
        [SerializeField]
        private ButtonClickedEvent m_OnRightClick = new ButtonClickedEvent();

        public ButtonClickedEvent onRightClick
        {
            get { return m_OnRightClick; }
            set { m_OnRightClick = value; }
        }

        //public ButtonClickedEvent onRightClick;

        protected ExButton()
        {
            //onClick = new ButtonClickedEvent();
            //onRightClick = new ButtonClickedEvent();
        }

        private void LeftClick()
        {
            UISystemProfilerApi.AddMarker("Button.onLeftClick", this);
            onClick.Invoke();
        }

        private void RightClick()
        {
            UISystemProfilerApi.AddMarker("Button.onRightClick", this);
            onRightClick.Invoke();
        }

        override public void OnPointerClick(PointerEventData eventData)
        {
            if (!IsActive() || !IsInteractable())
                return;

            if (eventData.button == PointerEventData.InputButton.Left)
                LeftClick();
            else if(eventData.button == PointerEventData.InputButton.Right)
                RightClick();
        }
    }
}