using System;
using Bind;
using DingoLevelBasedInputSystem.InputControllerModels;
using DingoProjectAppStructure.Core.Model;
using DingoProjectAppStructure.Core.ViewModel;

namespace DingoLevelBasedInputSystem.Elements
{
    public abstract class InputDependViewModel<T> : AppViewModelBase, IDisposable
        where T : AppModelBase
    {
        private IReadonlyBind<InputControllerModel> _inputControllerBind;
        private InputControllerModel _inputControllerModel;
        private T _enabledModel;
        private bool _disposed;

        protected abstract void EnableModel(T model);
        protected abstract void DisableModel(T model);

        private void EnableController(Type type, AppModelBase modelBase)
        {
            if (_disposed || modelBase is not T model
                || ReferenceEquals(_enabledModel, model))
            {
                return;
            }

            DisableEnabledModel();
            _enabledModel = model;
            EnableModel(model);
        }

        private void DisableController(Type type, AppModelBase modelBase)
        {
            if (_disposed || modelBase is not T model
                || !ReferenceEquals(_enabledModel, model))
            {
                return;
            }

            DisableEnabledModel();
        }

        protected InputDependViewModel(
            AppViewModelRoot appViewModelRoot,
            AppModelRoot appModelRoot)
            : base(appViewModelRoot, appModelRoot)
        {
            _inputControllerBind = appModelRoot.ExternalDependencies
                .Get<SingleInputControllers>()?.InputControllerModel;
            _inputControllerBind?.SafeSubscribeAndSet(
                InputControllerModelInitialized);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _inputControllerBind?.UnSubscribe(
                InputControllerModelInitialized);
            _inputControllerBind = null;
            DetachInputController();
        }

        private void InputControllerModelInitialized(
            InputControllerModel inputControllerModel)
        {
            if (_disposed
                || ReferenceEquals(_inputControllerModel, inputControllerModel))
            {
                return;
            }

            DetachInputController();
            _inputControllerModel = inputControllerModel;
            _inputControllerModel?.SubscribeAndSet<T>(
                EnableController,
                DisableController);
        }

        private void DetachInputController()
        {
            _inputControllerModel?.UnSubscribe(
                EnableController,
                DisableController);
            _inputControllerModel = null;
            DisableEnabledModel();
        }

        private void DisableEnabledModel()
        {
            var model = _enabledModel;
            _enabledModel = null;
            if (model != null)
            {
                DisableModel(model);
            }
        }
    }
}
