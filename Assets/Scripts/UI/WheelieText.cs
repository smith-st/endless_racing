using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Text))]
    public class WheelieText : MonoBehaviour
    {
        private Text _text;
        private int _counter;
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
            }else if (_counter < 0)
            {
                _counter++;
                if (_counter == 0)
                {
                    _text.enabled = false;
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
                _counter = -10;
            }
        }
    }
}
