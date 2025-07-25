using DingoProjectAppStructure.Core.Model;

namespace LevelBasedInputSystem.InputsHandle.Core
{
    public static class InputSystemExtensions
    {
        public static void SetFullInputModelActive(this AppModelRoot appModelRoot, string playerId, bool value)
        {
            appModelRoot.Get<LowLevelPlayersInputsModel>().SetActive(playerId, value);
            appModelRoot.Get<MiddleLevelSourceInputsModel>().SetActive(playerId, value);
        }
        
        public static void EnableHighLevelInputsOnly(this AppModelRoot appModelRoot, string playerId)
        {
            appModelRoot.Get<LowLevelPlayersInputsModel>().SetActive(playerId, false);
            appModelRoot.Get<MiddleLevelSourceInputsModel>().SetActive(playerId, false);
        }
    }
}