using System;
using System.Collections.Generic;
using Core.QuotaLogic;
using Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generators
{
    public class QuotaGenerator
    {
        private readonly List<String> _quotaNames =  new List<String>
        {
            "Quota Name",
            "Small Quota",
            "Medium Quota",
            "Large Quota"
        };

        private readonly List<String> _quotaDescriptions = new List<string>
        {
            "Quota Description"
        };
        
        private readonly int _minQuota = 2000;
        private readonly int _maxQuota = 16000;
        private readonly float _baseMult = 1;
        private readonly float _moreDifficultMult = 1.3f;
        private readonly float _moreEasyMult = 0.7f;
        
        private int GenerateQuotaSize(int previousQuotaSize, bool isSuccess)
        {
            if (isSuccess)
            {
                float multiplier = Random.Range(_baseMult, _moreDifficultMult);
                return Mathf.Clamp((int)Math.Ceiling(previousQuotaSize * multiplier), _minQuota, _maxQuota);
            }
            else
            {
                float multiplier = Random.Range(_moreEasyMult, _baseMult);
                return Mathf.Clamp((int)Math.Floor(previousQuotaSize * multiplier), _minQuota, _maxQuota);
            }
        }

        public QuotaRuntime GenerateQuotaRuntime(int previousQuotaSize, bool isSuccess)
        {
            int size = GenerateQuotaSize(previousQuotaSize, isSuccess);
            Quota quota = ScriptableObject.CreateInstance<Quota>();
            
            quota.InitializeQuota(
                _quotaNames[0],
                _quotaDescriptions[0],
                size,
                5);
            
            QuotaRuntime quotaRuntime = ScriptableObject.CreateInstance<QuotaRuntime>();
            
            quotaRuntime.SetQuotaRuntime(
                quota,
                0,
                0);
            
            return quotaRuntime;
        }

        public QuotaRuntime GenerateFirstQuotaRuntime()
        {
            Quota quota = ScriptableObject.CreateInstance<Quota>();
            
            quota.InitializeQuota(
                _quotaNames[0],
                _quotaDescriptions[0],
                _minQuota,
                quota.MaxDuration);
            
            QuotaRuntime quotaRuntime = ScriptableObject.CreateInstance<QuotaRuntime>();
            
            quotaRuntime.SetQuotaRuntime(
                quota,
                0,
                0);
            
            return quotaRuntime;
        }
    }
}