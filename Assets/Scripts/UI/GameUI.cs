using Common;
using Messaging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] Button btnClearAll;
        [SerializeField] Button btnClearAdjacent;
        [SerializeField] Button btnSpawn;
        [SerializeField] Slider sliderZoom;

        private void Start()
        {
            btnClearAll.onClick.AddListener(() => SignalBus.Fire(new ClearBallsSignal() { adjacentOnly = false }));
            btnClearAdjacent.onClick.AddListener(() => SignalBus.Fire(new ClearBallsSignal() { adjacentOnly = true }));

            btnSpawn.gameObject.GetEventTrigger().AddCallback(EventTriggerType.PointerDown, () => SignalBus.Fire(new StartSpawnerSignal()));
            btnSpawn.gameObject.GetEventTrigger().AddCallback(EventTriggerType.PointerUp, () => SignalBus.Fire(new StopSpawnerSignal()));

            sliderZoom.onValueChanged.AddListener(v => SignalBus.Fire(new CameraZoomSignal() { targetZoom = v }));
        }
    }
}
