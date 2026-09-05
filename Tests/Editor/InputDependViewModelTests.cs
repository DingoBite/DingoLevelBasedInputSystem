#if UNITY_EDITOR

using System.Collections.Generic;
using DingoLevelBasedInputSystem.Elements;
using DingoLevelBasedInputSystem.InputControllerModels;
using DingoProjectAppStructure.Core.Model;
using DingoProjectAppStructure.Core.ViewModel;
using NUnit.Framework;

namespace DingoLevelBasedInputSystem.Tests.Editor
{
    public class InputDependViewModelTestInput : AppModelBase
    {
    }

    public class InputDependViewModelProbe
        : InputDependViewModel<InputDependViewModelTestInput>
    {
        public readonly List<InputDependViewModelTestInput> Enabled = new();
        public readonly List<InputDependViewModelTestInput> Disabled = new();

        public InputDependViewModelProbe(
            AppViewModelRoot appViewModelRoot,
            AppModelRoot appModelRoot)
            : base(appViewModelRoot, appModelRoot)
        {
        }

        protected override void EnableModel(
            InputDependViewModelTestInput model)
        {
            Enabled.Add(model);
        }

        protected override void DisableModel(
            InputDependViewModelTestInput model)
        {
            Disabled.Add(model);
        }
    }

    [Category("PoliticsPhase4")]
    public class InputDependViewModelTests
    {
        [Test]
        public void ControllerReplacement_DisablesOldAndTracksOnlyNew()
        {
            var inputs = new SingleInputControllers();
            var viewModel = CreateViewModel(inputs);
            var first = CreateEnabledController("first", out var firstModel);
            var second = CreateEnabledController("second", out var secondModel);

            inputs.SetupInputControllerModel(first);
            inputs.SetupInputControllerModel(second);

            Assert.That(viewModel.Enabled, Is.EqualTo(new[]
            {
                firstModel,
                secondModel,
            }));
            Assert.That(viewModel.Disabled, Is.EqualTo(new[]
            {
                firstModel,
            }));

            second.EnableModel<InputDependViewModelTestInput>();
            Assert.That(viewModel.Enabled.Count, Is.EqualTo(2));
            Assert.That(viewModel.Disabled.Count, Is.EqualTo(1));

            first.DisableModel<InputDependViewModelTestInput>();
            first.EnableModel<InputDependViewModelTestInput>();
            Assert.That(viewModel.Enabled.Count, Is.EqualTo(2));
            Assert.That(viewModel.Disabled.Count, Is.EqualTo(1));

            second.DisableModel<InputDependViewModelTestInput>();
            second.EnableModel<InputDependViewModelTestInput>();
            Assert.That(viewModel.Enabled.Count, Is.EqualTo(3));
            Assert.That(viewModel.Enabled[2], Is.SameAs(secondModel));
            Assert.That(viewModel.Disabled.Count, Is.EqualTo(2));
            Assert.That(viewModel.Disabled[1], Is.SameAs(secondModel));

            viewModel.Dispose();
        }

        [Test]
        public void Dispose_DetachesExternalAndCurrentControllerSubscriptions()
        {
            var inputs = new SingleInputControllers();
            var viewModel = CreateViewModel(inputs);
            var first = CreateEnabledController("first", out var firstModel);
            inputs.SetupInputControllerModel(first);

            viewModel.Dispose();
            viewModel.Dispose();

            Assert.That(viewModel.Enabled, Is.EqualTo(new[] { firstModel }));
            Assert.That(viewModel.Disabled, Is.EqualTo(new[] { firstModel }));

            first.DisableModel<InputDependViewModelTestInput>();
            first.EnableModel<InputDependViewModelTestInput>();
            var second = CreateEnabledController("second", out _);
            inputs.SetupInputControllerModel(second);

            Assert.That(viewModel.Enabled.Count, Is.EqualTo(1));
            Assert.That(viewModel.Disabled.Count, Is.EqualTo(1));
        }

        private static InputDependViewModelProbe CreateViewModel(
            SingleInputControllers inputs)
        {
            var dependencies = new ExternalDependencies();
            dependencies.Register(inputs);
            return new InputDependViewModelProbe(
                new AppViewModelRoot(),
                new AppModelRoot(dependencies));
        }

        private static InputControllerModel CreateEnabledController(
            string sourceId,
            out InputDependViewModelTestInput model)
        {
            var controller = new InputControllerModel(
                new InputControllerProperties(sourceId));
            model = new InputDependViewModelTestInput();
            controller.RegisterModel(model);
            controller.EnableModel<InputDependViewModelTestInput>();
            return controller;
        }
    }
}

#endif
