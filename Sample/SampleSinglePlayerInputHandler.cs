using System.Threading.Tasks;
using DingoLevelBasedInputSystem.InputControllerModels;
using DingoProjectAppStructure.Core.AppRootCore;
using DingoProjectAppStructure.Core.Model;
using LevelBasedInputSystem.InputControllerModels;
using LevelBasedInputSystem.InputsHandle.Core;
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
            var singleInputControllersModel = _appModelRoot.Get<SingleInputControllersModel>();
            singleInputControllersModel.SetupInputControllerModel(inputControllerModel);
            inputControllerModel.RegisterModel(new SampleInputProviderModel());
        }
    }
}