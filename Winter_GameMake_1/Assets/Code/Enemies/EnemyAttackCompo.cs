﻿using Code.Combats;
using Code.Core.EventSystems;
using Code.Entities;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Code.Enemies
{
    public class EnemyAttackCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [Header("Atk settings")] 
        public float attackDistance;
        public float detectDistance;
        
        [SerializeField] private float attackCooldown, cooldownRandomness;
        
        [Header("Reference")]
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private string attackRangeName, detectRangeName, attackCooldownName;

        private BTEnemy _enemy;
        private BlackboardVariable<float> _attackCooldownVariable;
        private EntityAnimationTrigger _triggerCompo;

        public void Initialize(Entity entity)
        {
            _enemy = entity as BTEnemy;
            _triggerCompo = entity.GetCompo<EntityAnimationTrigger>();
            damageCaster.InitCaster(entity);
            Debug.Assert(_enemy != null, $"Not corrected entity - enemy attack component [{entity.gameObject.name}]");
            _enemy.GetBlackboardVariable<float>(attackRangeName).Value = attackDistance;
            _enemy.GetBlackboardVariable<float>(detectRangeName).Value = detectDistance;
            _attackCooldownVariable = _enemy.GetBlackboardVariable<float>(attackCooldownName);
        }

        public void AfterInit()
        {

            _triggerCompo.OnAttackTrigger += HandleAttackTrigger;
        }

        private void OnDestroy()
        {
            _triggerCompo.OnAttackTrigger -= HandleAttackTrigger;
        }

        private void HandleAttackTrigger()
        {
            float damage = 5f;
            Vector2 knockBackForce = new Vector2(1f, 1f);
            bool success = damageCaster.CastDamage(damage, knockBackForce, false);

            if (success)
            {
                Debug.Log($"<color=red>Damaged! - {damage}</color>");
            }
        }
    }
}