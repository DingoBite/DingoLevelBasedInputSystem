using System.Threading.Tasks;
using DingoLevelBasedInputSystem.InputControllerModels;
using DingoLevelBasedInputSystem.InputsHandle.Core;
using DingoProjectAppStructure.Core.AppRootCore;
using DingoProjectAppStructure.Core.Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DingoLevelBasedInputSystem.Sample
{
    public class SampleSinglePlayerInputHandler : AppStateStaticElementBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private bool _autoEnableOnInit;
    
        private AppModelRoot _appModelRoot;
        private string _id;

        public override Task BindAsync(AppModelRoot appModel)
        {
            _appModelRoot = appModel;
            return base.BindAsync(_appModelRoot);
        }

        public override Task PostInitializeAsync()
        {
            InitializeInput();
            return base.PostInitializeAsync();
        }
        
private void InitializeInput()
        {
            _id = _appModelRoot.Get<LowLevelPlayersInputsModel>().AddPlayer(_playerInput);
            if (_autoEnableOnInit)
                _appModelRoot.SetFullInputModelActive(_id, true);

            var inputControllerModel = new InputControllerModel(new InputControllerProperties(_id));
            var singleInputControllersModel = _appModelRoot.ExternalDependencies.Get<SingleInputControllers>();
            var multipleInputControllersRegistererModel = _appModelRoot.ExternalDependencies.Get<MultipleInputControllers>();
            singleInputControllersModel.SetupInputControllerModel(inputControllerModel);
            multipleInputControllersRegistererModel?.UnregisterSource(_id);
            multipleInputControllersRegistererModel?.RegisterSource(inputControllerModel);
            inputControllerModel.RegisterModel(new SampleInputProviderModel());
        }

private void OnDestroy()
        {
            if (_appModelRoot == null || string.IsNullOrEmpty(_id))
                return;

            var externalDependencies = _appModelRoot.ExternalDependencies;
            externalDependencies.Get<MultipleInputControllers>()?.UnregisterSource(_id);
            externalDependencies.Get<SingleInputControllers>()?.ClearInputControllerModel(_id);
        }

    }
}