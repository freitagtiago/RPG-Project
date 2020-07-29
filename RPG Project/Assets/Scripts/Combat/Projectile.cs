﻿using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Resources;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 30f;
        [SerializeField] Health _target = null;
        [SerializeField] GameObject _hitEffect = null;
        [SerializeField] float _lifeAfterImpact = 2f;
        float _damage = 0;
        [SerializeField] float _damagePlus = 0;
        [SerializeField] bool _canHoming = false;
        GameObject instigator = null;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        // Update is called once per frame
        void Update()
        {
            if (_target == null) return;
            Move();
        }

        private void Move()
        {
            if(_canHoming == true && !_target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * _speed * Time.deltaTime );
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this._target = target;
            this._damage = damage + _damagePlus;
            this.instigator = instigator;
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return _target.transform.position;
            return _target.transform.position + Vector3.up * targetCapsule.height/2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;
            if (_target.IsDead() == true) return;
            if(_hitEffect != null)
            {
                GameObject effect = Instantiate(_hitEffect, GetAimLocation(), other.transform.rotation);
                Destroy(effect.gameObject, _lifeAfterImpact);
            }
            _target.TakeDamage(instigator, _damage);
            Destroy(this.gameObject);
        }
    }
}
