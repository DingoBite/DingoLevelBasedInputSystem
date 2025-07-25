using System;
using System.Collections.Generic;
using Bind;
using DingoProjectAppStructure.Core.Model;

namespace LevelBasedInputSystem.InputsHandle.Core
{
    public class MiddleLevelSourceInputsModel : HardLinkAppModelBase
    {
        public event Action<MiddleLevelSourceInput> Added;
        public event Action<MiddleLevelSourceInput> Removed;
        
        private readonly Bind<Dictionary<string, MiddleLevelSourceInput>> _middleLevelSourceInputContainers = new(new Dictionary<string, MiddleLevelSourceInput>());
        
        private readonly LowLevelPlayersInputsModel _lowLevelPlayersInputsModel;
        private readonly Func<LowLevelPlayerInputsWrapper, MiddleLevelSourceInput> _linker;

        public MiddleLevelSourceInputsModel(LowLevelPlayersInputsModel lowLevelPlayersInputsModel, Func<LowLevelPlayerInputsWrapper, MiddleLevelSourceInput> linker)
        {
            _linker = linker;
            _lowLevelPlayersInputsModel = lowLevelPlayersInputsModel;
            _lowLevelPlayersInputsModel.Added += LowLevelInputsAdded;
            _lowLevelPlayersInputsModel.Removed += LowLevelInputsRemoved;
        }

        public void SetActive(string id, bool value)
        {
            if (!_middleLevelSourceInputContainers.V.TryGetValue(id, out var wrapper))
                return;
            wrapper.Enabled = value;
        }
        
        public void AddMiddleLevelSourceInput(MiddleLevelSourceInput input)
        {
            _middleLevelSourceInputContainers.V[input.Id] = input;
            _middleLevelSourceInputContainers.V = _middleLevelSourceInputContainers.V;
            Added?.Invoke(input);
        }
        
        public void RemoveMiddleLevelSourceInput(MiddleLevelSourceInput input)
        {
            _middleLevelSourceInputContainers.V.Remove(input.Id);
            _middleLevelSourceInputContainers.V = _middleLevelSourceInputContainers.V;
            input.Enabled = false;
            Removed?.Invoke(input);
        }
        
        private void LowLevelInputsAdded(LowLevelPlayerInputsWrapper wrapper)
        {
            var middleLevelSourceInput = _linker(wrapper);
            _middleLevelSourceInputContainers.V[wrapper.Id] = middleLevelSourceInput;
            middleLevelSourceInput.Enabled = wrapper.Enabled;
            _middleLevelSourceInputContainers.V = _middleLevelSourceInputContainers.V;
            Added?.Invoke(middleLevelSourceInput);
        }

        private void LowLevelInputsRemoved(LowLevelPlayerInputsWrapper wrapper)
        {
            _middleLevelSourceInputContainers.V.Remove(wrapper.Id, out var middleLevelSourceInput);
            middleLevelSourceInput.Enabled = false;
            _middleLevelSourceInputContainers.V = _middleLevelSourceInputContainers.V;
            Removed?.Invoke(middleLevelSourceInput);
        }
    }
}