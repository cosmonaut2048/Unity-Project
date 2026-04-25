using Core.DayLogic.DayEnd;
using Core.DayLogic.DayStart;
using UnityEngine;

namespace Core.DayLogic
{
    public class DayCycleManager : MonoBehaviour
    {
        
        [SerializeField] private DayStartSetup dayStartSetup;
        [SerializeField] private DayEndSetup dayEndSetup;
        
        public static DayCycleManager Instance { get; private set; }
        public void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void OnDayStart()
        {
            dayStartSetup.SetupDayStart();
        }

        public void OnDayEnd()
        {
            dayEndSetup.SetupDayEnd();
        }
    }
}