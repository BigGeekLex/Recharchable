using System;
using CorD.SparrowInterfaceField;
using MPS.Recharhable.Base;
using Sirenix.OdinInspector;
using SS.Upgrade;
using UnityEngine;
using UniRx;

namespace MPS.Gameplay.Rechargable.Logic
{
    public class RechargableArea : AreaBase, IObservable<RechargableStatus>
    {
        private IDisposable _currentDisposable;
        
        [SerializeField] 
        private InterfaceField<IRechargerProvider> recharcherSource;
        [SerializeField] 
        protected string speedPathBase;

        private IRechargerProvider RechargerProvider => recharcherSource.i;
        private ReactiveProperty<RechargableStatus> _statusProperty = new ReactiveProperty<RechargableStatus>();
        public event Action OnUnitFinished;
        private void Awake()
        {
            if (RechargerProvider != null) RechargerProvider.Finished += FinishProcess;
        }

        private void Start()
        {
            OnStart();
        }
        private void OnEnable()
        {
            _statusProperty.SetValueAndForceNotify(RechargableStatus.None);
        }
        private void OnDisable()
        {
            if(RechargerProvider != null) RechargerProvider.StopRechargeProcess();
        }
        protected void OnDestroy()
        {
            if(_currentDisposable != null) _currentDisposable.Dispose();
            
            if (RechargerProvider != null) RechargerProvider.Finished -= FinishProcess;
            
            OnDest();
        }
        protected void StopProcess()
        {
            RechargerProvider.StopRechargeProcess();
            _statusProperty.SetValueAndForceNotify(RechargableStatus.Paused);
        }
        protected void FinishProcess()
        {
            if (_currentDisposable != null)
            {
                _currentDisposable.Dispose();
            }
            
            RechargerProvider.StopRechargeProcess();
            
            OnUnitFinished?.Invoke();
            //OnDeactivated?.Invoke(this);
            StopProcess();
            
            OnDeactivated(Activatable);
            
            _statusProperty.SetValueAndForceNotify(RechargableStatus.Finished);

            gameObject.SetActive(false);
        }
        protected void StartProcess(GameObject sender)
        {
            if(_currentDisposable != null) _currentDisposable.Dispose();
            
            float duration = 0.0f;
            
            _currentDisposable = sender.GetComponent<MSFD.Data.IStreamProvider>().GetStream(speedPathBase).Subscribe((x)=>
            {
                duration = (float) x;
            });
            
            if (RechargerProvider.TryToStartRechargeProcess(duration))
            {
                _statusProperty.SetValueAndForceNotify(RechargableStatus.Started);
            }
        }
        protected override void OnActivated(GameObject sender)
        {
            StartProcess(sender);
        }
        protected override void OnDeactivated(IActivatable activatable)
        {
            
        }
        public IDisposable Subscribe(IObserver<RechargableStatus> observer)
        {
            observer.OnNext(_statusProperty.Value);
            return _statusProperty.Subscribe(observer);
        }
    }
}