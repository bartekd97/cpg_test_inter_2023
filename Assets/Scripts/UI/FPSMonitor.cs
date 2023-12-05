using TMPro;
using UnityEngine;

namespace UI
{
    public class FPSMonitor : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI txtFps;

        float _time = 0.0f;
        int _counter = 0;
        private void Update()
        {
            _time += Time.unscaledDeltaTime;
            _counter++;

            if (_time >= 1.0f)
            {
                txtFps.text = $"FPS: {_counter}";

                _time -= 1.0f;
                _counter = 0;
            }
        }
    }
}
