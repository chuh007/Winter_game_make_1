using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;
using DG.Tweening;
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
        }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately(false);
            // 엄청난 가비지 코드임. 수정해야함.
            Transform targetTrm = _player.GetCompo<PlayerTargetFinderCompo>().FindProximateTargetsInCicle().transform;
            _player.transform.DOMove(targetTrm.position, 0.2f).OnComplete(() => _player.ChangeState("IDLE"));
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}

