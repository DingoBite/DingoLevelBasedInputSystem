using UnityEngine;

namespace DingoLevelBasedInputSystem.Elements
{
    public abstract class InputModelDependBehaviour<TInputModel> : MonoBehaviour
    {
        [SerializeField] private bool _manageActiveness;
        
        protected TInputModel Model { get; private set; }
        public bool Active { get; private set; }
        public string SourceId { get; private set; }
        
        public void Enable(string sourceId, TInputModel model)
        {
            if (_manageActiveness)
                gameObject.SetActive(true);
            Model = model;
            Active = true;
            SourceId = sourceId;
            OnEnableBehaviour();
        }

        public void Disable()
        {
            if (_manageActiveness)
                gameObject.SetActive(false);
            Active = false;
            SourceId = null;
            if (Model != null)
                OnDisableBehaviour();
        }

        protected abstract void OnEnableBehaviour();
        protected abstract void OnDisableBehaviour();
    }
}