﻿using Code.Animators;
using Code.Entities;
using Code.Entities.FSM;
using Code.SkillSystem;

namespace Code.Players.States
{
    public abstract class PlayerGroundState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;
        
        public PlayerGroundState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnJumpKeyPressed += HandleJumpKeyPress;
            _player.PlayerInput.OnAttackKeyPressed += HandleAttackKeyPress;
            _player.PlayerInput.OnCounterKeyPressed += HandleCounterKeyPress;
            _player.PlayerInput.OnSkillKeyPressed += HandleSkillKeyPress;
        }

        public override void Update()
        {
            base.Update();
            if (_mover.IsGroundDetected() == false && _mover.CanManualMove)
            {
                _player.ChangeState("FALL");
            }
        }

        public override void Exit()
        {
            _player.PlayerInput.OnJumpKeyPressed -= HandleJumpKeyPress;
            _player.PlayerInput.OnAttackKeyPressed -= HandleAttackKeyPress;
            _player.PlayerInput.OnCounterKeyPressed -= HandleCounterKeyPress;
            _player.PlayerInput.OnSkillKeyPressed -= HandleSkillKeyPress;
            base.Exit();
        }

        private void HandleSkillKeyPress(bool isPressed)
        {
            Skill activeSkill = _player.GetCompo<SkillCompo>().activeSkill;
            if (activeSkill == null) return;
            
            if (isPressed && activeSkill.AttemptUseSkill())
            {
                if(activeSkill is IReleasable)
                    _player.ChangeState("SKILL_CHARGE");
                else 
                    _player.ChangeState("SKILL_USE");
            }
        }

        private void HandleCounterKeyPress()
        {
            //나중에 쿨타임도 체크해야한다.
            _player.ChangeState("COUNTER_ATTACK");
        }

        protected virtual void HandleAttackKeyPress()
        {
            if (_player.GetCompo<PlayerTargetFinderCompo>().FindProximateTargetsInCicle())
                _player.ChangeState("ONSET");
            else if(_mover.IsGroundDetected())
                _player.ChangeState("ATTACK");
        }

        private void HandleJumpKeyPress()
        {
            if(_mover.IsGroundDetected())
                _player.ChangeState("JUMP");
        }
    }
}
