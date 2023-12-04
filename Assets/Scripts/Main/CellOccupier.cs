using System.Collections;
using UnityEngine;

namespace Main
{
    public abstract class CellOccupier : MonoBehaviour
    {
        [Header("Cell Occupier config")]
        [SerializeField] float tweenTime = 0.2f;


        Cell _cell;
        public Cell RecentCell { get; private set; }
        public Cell Cell
        {
            get => _cell;
            set
            {
                RecentCell = _cell;
                _cell = value;
            }
        }

        public bool IsOnCell => Cell != null;


        Coroutine _tweenToCell = null;
        public void WarpToCell()
        {
            if (_tweenToCell != null)
            {
                StopCoroutine(_tweenToCell);
                _tweenToCell = null;
            }

            transform.position = Cell.transform.position;
        }
        public void AnimateToCell()
        {
            if (_tweenToCell != null)
            {
                StopCoroutine(_tweenToCell);
                _tweenToCell = null;
            }

            _tweenToCell = StartCoroutine(TweenToCell());
        }

        IEnumerator TweenToCell()
        {
            var start = transform.position;
            var end = Cell.transform.position;
            var time = 0.0f;

            while (time < tweenTime)
            {
                transform.position = Vector3.Lerp(start, end, time / tweenTime);
                yield return null;
                time += Time.deltaTime;
            }

            transform.position = end;
        }


        public virtual void OnCellChanged()
        {
            //
        }
    }
}
