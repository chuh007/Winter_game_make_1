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
            Vector2 dir = evt.target.position - _player.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _renderer.transform.rotation = Quaternion.Euler(0, 0, angle);
            Debug.Log(angle);
            int movePlus = evt.target.position.x - _player.transform.position.x > 0 ? 2 : -2;
            _player.transform.DOMove(new Vector2(evt.target.localPosition.x + movePlus,
                evt.target.position.y), 0.25f).OnComplete(() =>
            {
                _attackCompo.OnsetAttack(evt.target);
                _renderer.transform.rotation = Quaternion.Euler(0, 0, 0);
                _player.ChangeState("IDLE");
            });
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

