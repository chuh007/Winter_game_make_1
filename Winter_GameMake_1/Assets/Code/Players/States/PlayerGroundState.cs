using Code.Animators;
using Code.Combats.Onset;
using Code.Core.EventSystems;
using Code.Entities;
using Code.Entities.FSM;
using Code.SkillSystem;
using System;
using Unity.Behavior;
using UnityEngine;

namespace Code.Players.States
{
    public abstract class PlayerGroundState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;
        protected GameEventChannelSO uiEvt;

        public PlayerGroundState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
            uiEvt = _player.uiEvt;
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnJumpKeyPressed += HandleJumpKeyPress;
            _player.PlayerInput.OnAttackKeyPressed += HandleAttackKeyPress;
            _player.PlayerInput.OnCounterKeyPressed += HandleCounterKeyPress;
            _player.PlayerInput.OnSkillKeyPressed += HandleSkillKeyPress;
            _player.PlayerInput.OnOnsetKeyPressed += HandleOnsetKeyPress;
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
            _player.PlayerInput.OnOnsetKeyPressed -= HandleOnsetKeyPress;

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
            if(_mover.IsGroundDetected())
                _player.ChangeState("ATTACK");
        }

        private void HandleJumpKeyPress()
        {
            if(_mover.IsGroundDetected())
                _player.ChangeState("JUMP");
        }


        private float originalFixedDeltaTime = Time.fixedDeltaTime;
        private float originalTimeScale = 1f;

        private void HandleOnsetKeyPress(bool isPressed)
        {
            OnsetUIEvent onsetEvt = UIEvents.OnsetUIEvent;
            if (isPressed)
            {
                originalFixedDeltaTime = Time.fixedDeltaTime;
                Time.timeScale = 0.5f;
                Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
                uiEvt.RaiseEvent(onsetEvt);
            }
            else
            {
                Time.timeScale = originalTimeScale;
                Time.fixedDeltaTime = originalFixedDeltaTime;
                uiEvt.RaiseEvent(onsetEvt);
                Collider2D col = _player.GetCompo<PlayerTargetFinderCompo>().FindProximateTargetsInCicle();
                if (col != null && col.transform.GetComponent<IOnsetable>().IsFindPlayer == false)
                {
                    OnsetTargetEvent onSetTargetEvt = PlayerEvents.OnSetTargetEvent;
                    onSetTargetEvt.target = col.transform;
                    _player.PlayerChannel.RaiseEvent(onSetTargetEvt);
                    _player.ChangeState("ONSET");
                }
            }
        }
    }
}
