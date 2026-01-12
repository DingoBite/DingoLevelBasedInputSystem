using System;
using System.Collections.Generic;
using Bind;
using DingoProjectAppStructure.Core.Model;
using UnityEngine.InputSystem;

namespace DingoLevelBasedInputSystem.InputsHandle.Core
{
    public class LowLevelPlayersInputsModel : HardLinkAppModelBase
    {
        public event Action<LowLevelPlayerInputsWrapper> Added;
        public event Action<LowLevelPlayerInputsWrapper> Removed;
        
        private readonly Bind<Dictionary<string, LowLevelPlayerInputsWrapper>> _lowLevelPlayers = new(new Dictionary<string, LowLevelPlayerInputsWrapper>());
        private readonly Dictionary<PlayerInput, string> _playerToId = new();
        
        public IReadonlyBind<IReadOnlyDictionary<string, LowLevelPlayerInputsWrapper>> LowLevelPlayers => _lowLevelPlayers;
        
        public string AddPlayer(PlayerInput playerInput)
        {
            var id = GetId(playerInput);
            _playerToId[playerInput] = id;
            var wrapper = new LowLevelPlayerInputsWrapper(id, playerInput);
            _lowLevelPlayers.V[id] = wrapper;
            _lowLevelPlayers.V = _lowLevelPlayers.V;
            Added?.Invoke(wrapper);
            return wrapper.Id;
        }

        public string RemovePlayer(PlayerInput playerInput)
        {
            var id = _playerToId.GetValueOrDefault(playerInput, "");
            RemovePlayer(id);
            return id;
        }
        
        public void RemovePlayer(string id)
        {
            if (!_lowLevelPlayers.V.Remove(id, out var wrapper))
                return;
            wrapper.Dispose();
            _lowLevelPlayers.V = _lowLevelPlayers.V;
            Removed?.Invoke(wrapper);
        }

        public void SetActive(string id, bool value)
        {
            if (!_lowLevelPlayers.V.TryGetValue(id, out var wrapper))
                return;
            wrapper.Enabled = value;
        }

        public string GetId(PlayerInput playerInput)
        {
            return playerInput.user.id.ToString();
        }
    }
}