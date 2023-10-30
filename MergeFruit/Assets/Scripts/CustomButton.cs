using System;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    /// <summary>
    /// A standard button that sends an event when clicked.
    /// </summary>
    [AddComponentMenu("UI/CustomButton", 30)]
    public class CustomButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [Serializable]
        /// <summary>
        /// Function definition for a button click event.
        /// </summary>
        public class ButtonClickedEvent : UnityEvent { }

        // Event delegates triggered on click.
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent _onClick = new ButtonClickedEvent();

        private bool _buttonClickAnimIsRunning = false;

        protected CustomButton()
        { }

        public ButtonClickedEvent onClick
        {
            get { return _onClick; }
            set { _onClick = value; }
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable() || _buttonClickAnimIsRunning)
                return;

            UISystemProfilerApi.AddMarker("CustomButton.onClick", this);
            StartCoroutine(ButtonClickAnim());

            if (GameManager.Instance != null)
                GameManager.Instance.PlaySFX("Button");
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }

        IEnumerator ButtonClickAnim()
        {
            _buttonClickAnimIsRunning = true;

            Vector3 originScale = transform.localScale;

            float time = 0.25f;
            float count = 0f;
            float t;
            float targetScale = 1.1f;

            while(count < time)
            {
                t = count / time;

                if(count < time / 2)
                    transform.localScale = Vector3.Lerp(transform.localScale, originScale * targetScale, t);
                else
                    transform.localScale = Vector3.Lerp(transform.localScale, originScale, t);

                count += Time.deltaTime;

                yield return null;
            }

            transform.localScale = originScale;

            _buttonClickAnimIsRunning = false;

            _onClick.Invoke();
        }

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();

            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable())
                return;

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
    }
}
