using System;
using CorD.SparrowInterfaceField;
using MSFD;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace MPS.Recharhable.Base
{
    public class RechargerProviderController : MonoBehaviour, IRechargerProvider
    {
        [SerializeField] private UnityEvent onStarted;
        [SerializeField] private UnityEvent onFinished;
        [SerializeField]
        private InterfaceField<IRechargable<float>> rechargableSource;
        
        public event Action Finished;
        public event Action Started;
        public event Action<float> Changed;
        private IRechargable<float> _rechargable => rechargableSource.i;
        private bool _rechargeStatus;
        private void Awake()
        {
            if (_rechargable != null)
            {
                _rechargable.GetObsIsRechargeStarted().Subscribe((x) => _rechargeStatus = x).AddTo(this);
                
                _rechargable.GetObsOnMinBorder().Subscribe((x) =>
                {
                    _rechargable.GetRechargeSpeed().SetValue(0); _rechargable.StopRecharge(); _rechargable.Fill(); _rechargeStatus = false; Finished?.Invoke(); onFinished?.Invoke();
                }).AddTo(this);
                
                _rechargable.GetObsOnChange().Subscribe((x) => Changed?.Invoke(x)).AddTo(this);
            }
        }
        public bool TryToStartRechargeProcess(float speed)
        {
            if (!_rechargeStatus)
            {
                _rechargeStatus = true;
            }
            
            _rechargable.GetRechargeSpeed().SetValue(speed);
            _rechargable.StartRecharge();
                
            Started?.Invoke();
            onStarted?.Invoke();
            
            return true;
        }
        public void StopRechargeProcess()
        {
            if (_rechargeStatus)
            {
                _rechargable.GetRechargeSpeed().SetValue(0);
                _rechargable.StopRecharge();
            }
        }
    }
}