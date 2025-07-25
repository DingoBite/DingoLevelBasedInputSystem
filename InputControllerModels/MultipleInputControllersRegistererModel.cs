using System.Collections.Generic;
using System.Linq;
using Bind;
using DingoProjectAppStructure.Core.Model;
using DingoUnityExtensions.Extensions;

namespace LevelBasedInputSystem.InputControllerModels
{
    public class MultipleInputControllersRegistererModel : HardLinkAppModelBase
    {
        private readonly Dictionary<string, InputControllerModel> _inputControllerModels = new ();
        private readonly Bind<Dictionary<string, IReadonlyInputControllerModel>> _readOnlyControllers = new(new Dictionary<string, IReadonlyInputControllerModel>());

        public IReadonlyBind<IReadOnlyDictionary<string, IReadonlyInputControllerModel>> Controllers => _readOnlyControllers;
        
        public bool TryGetInputControllerModel(string sourceId, out IReadonlyInputControllerModel inputControllerModel)
        {
            inputControllerModel = _inputControllerModels.GetValueOrDefault(sourceId);
            return inputControllerModel != null;
        }

        public void EnableModel<T>(string sourceId) where T : AppModelBase
        {
            if (!_inputControllerModels.TryGetValue(sourceId, out var inputControllerModel))
                return;
            inputControllerModel.EnableModel<T>();
            UpdateControllers();
        }

        public void DisableModel<T>(string sourceId) where T : AppModelBase
        {
            if (!_inputControllerModels.TryGetValue(sourceId, out var inputControllerModel))
                return;
            inputControllerModel.DisableModel<T>();
            UpdateControllers();
        }
        
        public void EnableModels<T>() where T : AppModelBase
        {
            foreach (var (key, inputControllerModel) in _inputControllerModels)
            {
                inputControllerModel.EnableModel<T>();
            }
            UpdateControllers();
        }

        public void DisableModels<T>() where T : AppModelBase
        {
            foreach (var (key, inputControllerModel) in _inputControllerModels)
            {
                inputControllerModel.DisableModel<T>();
            }
            UpdateControllers();
        }
        
        public void RegisterSource(InputControllerModel inputControllerModel)
        {
            _inputControllerModels.Add(inputControllerModel.SourceId, inputControllerModel);
            UpdateControllers();
        }

        public void UnregisterSource(string sourceId)
        {
            _inputControllerModels.Remove(sourceId);
            UpdateControllers();
        }

        private void UpdateControllers()
        {
            var dict = _readOnlyControllers.V;
            dict.Clear();
            dict.AddRange(_inputControllerModels.Select(p => new KeyValuePair<string, IReadonlyInputControllerModel>(p.Key, p.Value)));
            _readOnlyControllers.V = dict;
        }
    }
}