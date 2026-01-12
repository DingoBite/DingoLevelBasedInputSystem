using System;

namespace DingoLevelBasedInputSystem.InputsHandle.Core
{
    public class MiddleLevelSourceInput
    {
        public event Action<MiddleLevelSourceInput> Enable;
        public event Action<MiddleLevelSourceInput> Disable;
        
        public readonly LowLevelPlayerInputsWrapper LowLevelPlayerInputsWrapper;
        public readonly string Id;
        
        private bool _enabled;

        public MiddleLevelSourceInput(LowLevelPlayerInputsWrapper wrapper)
        {
            LowLevelPlayerInputsWrapper = wrapper;
            Id = wrapper.Id;
        }
        
        public MiddleLevelSourceInput(string id)
        {
            Id = id;
        }
        
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value)
                    return;
                if (value)
                    Enable?.Invoke(this);
                else
                    Disable?.Invoke(this);
                _enabled = value;
            }
        }
    }
}