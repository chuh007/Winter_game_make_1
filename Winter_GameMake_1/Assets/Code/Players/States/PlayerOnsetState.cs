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
        private PlayerAttackCompo _attackCompo;
        public PlayerOnsetState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
            _player.PlayerChannel.AddListener<OnsetTargetEvent>(HandleOnSet);
        }

        public override void Enter()
        {
            base.Enter();
        }

        private void HandleOnSet(OnsetTargetEvent evt)
        {
            _renderer.FlipController(evt.target.transform.position.x - _player.transform.position.x);
            _player.transform.DOMove(evt.target.position, 0.25f).OnComplete(() =>
            {
                _attackCompo.OnsetAttack();
                _player.ChangeState("IDLE");// юс╫ц
            });
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

