using System;
using DiceGame.AppStructure;
using DingoProjectAppStructure.Core.AppRootCore;
using DingoProjectAppStructure.Core.Model;

namespace DingoLevelBasedInputSystem.Sample
{
    public abstract class InputModelAppStateElementBehaviour<T> : AppStateElementBehaviour where T : AppModelBase
    {
        protected abstract void EnableModel(T model);
        protected abstract void DisableModel(T model);

        private void EnableController(Type type, AppModelBase modelBase)
        {
            if (modelBase is T model)
                EnableModel(model);
        }

        private void DisableController(Type type, AppModelBase modelBase)
        {
            if (modelBase is T model)
                DisableModel(model);
        }

        protected override void SubscribeOnly() => Game.InputControllerModel.V.SubscribeAndSet<T>(EnableController, DisableController);
        protected override void UnsubscribeOnly() => Game.InputControllerModel.V.UnSubscribe(EnableController, DisableController);
    }
}