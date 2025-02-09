using Code.Animators;
using Code.Core.EventSystems;
using Code.Entities;
using Code.Entities.FSM;
using DG.Tweening;
using System;
using UnityEngine;

namespace Code.Players.States
{
    public class PlayerOnsetState : EntityState
    {
        private Player _player;
        private EntityMover _mover;
        public PlayerOnsetState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
            _player.PlayerChannel.AddListener<OnsetTargetEvent>(HandleOnSet);
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately(false);
        }

        private void HandleOnSet(OnsetTargetEvent evt)
        {
            _player.transform.DOMove(evt.target.position, 0.5f).OnComplete(() => _player.ChangeState("IDLE"));
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

