using Bind;

namespace DingoLevelBasedInputSystem.InputControllerModels
{
    public class SingleInputControllers
    {
        private readonly Bind<InputControllerModel> _inputControllerModel = new();

        public IReadonlyBind<InputControllerModel> InputControllerModel => _inputControllerModel;
        public string SourceId => _inputControllerModel.V?.SourceId;

        public void SetupInputControllerModel(InputControllerModel inputControllerModel)
        {
            _inputControllerModel.V = inputControllerModel;
        }

        public void ClearInputControllerModel(string sourceId = null)
        {
            if (sourceId != null && SourceId != sourceId)
                return;

            _inputControllerModel.V = null;
        }
    }
}
