using Bind;
using DingoProjectAppStructure.Core.Model;

namespace DingoLevelBasedInputSystem.InputControllerModels
{
    public class SingleInputControllersModel : HardLinkAppModelBase
    {
        private readonly Bind<InputControllerModel> _inputControllerModel = new();

        public IReadonlyBind<InputControllerModel> InputControllerModel => _inputControllerModel;
        public string SourceId => _inputControllerModel.V?.SourceId;
        
        public void SetupInputControllerModel(InputControllerModel inputControllerModel)
        {
            _inputControllerModel.V = inputControllerModel;
        }
    }
}