using Code.Animators;
using Code.Combats.Onset;
using Code.Core.EventSystems;
using Code.Players;
using UnityEngine;

namespace Code.Entities.FSM
{
    public abstract class PlayerState : EntityState
    {
        protected Player _player;
        protected GameEventChannelSO uiEvt;

        public PlayerState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            uiEvt = _player.uiEvt;
        }
        public override void Enter()
        {
            base.Enter();
            _player.PlayerInput.OnOnsetKeyPressed += HandleOnsetKeyPress;

        }

        public override void Exit()
        {
            _player.PlayerInput.OnOnsetKeyPressed -= HandleOnsetKeyPress;
            base.Exit();

        }

        private float originalFixedDeltaTime = Time.fixedDeltaTime;
        private float originalTimeScale = 1f;
        private void HandleOnsetKeyPress(bool isPressed)
        {
            OnsetUIEvent onsetEvt = UIEvents.OnsetUIEvent;
            onsetEvt.isON = isPressed;
            uiEvt.RaiseEvent(onsetEvt);
            if (isPressed)
            {
                originalFixedDeltaTime = Time.fixedDeltaTime;
                Time.timeScale = 0.5f;
                Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
            }
            else
            {
                Time.timeScale = originalTimeScale;
                Time.fixedDeltaTime = originalFixedDeltaTime;
                Collider2D col = _player.GetCompo<PlayerTargetFinderCompo>().FindProximateTargetsInCicle();
                Debug.Log(col);
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

