using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class WheelieText : MonoBehaviour
    {
        private Text _text;
        private byte _counter;
        private bool _status = true;

        private void Awake()
        {
            _text = gameObject.GetComponent<Text>();
            Display(false);
        }

        private void FixedUpdate()
        {
            if (_counter > 0)
            {
                _counter--;
                if (_counter == 0)
                {
                    _text.enabled = true;
                }
            }
        }

        public void Display(bool value)
        {
            if (_status == value)
            {
                return;
            }

            _status = value;
            if (value)
            {
                _counter = 20;
            }
            else
            {
                _text.enabled = false;
                _counter = 0;
            }
        }
    }
}
