using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generators
{
    public class QuotaGenerator
    {
        private readonly int _minQuota = 2000;
        private readonly int _maxQuota = 16000;
        private readonly float _baseMult = 1;
        private readonly float _moreDifficultMult = 1.3f;
        private readonly float _moreEasyMult = 0.7f;
        
        public int GenerateQuotaSize(int previousQuotaSize, bool isSuccess)
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
    }
}