using System;

namespace MPS.Recharhable.Base
{
    public interface IRechargerProvider
    {
        public event Action Finished;
        public event Action Started;
        public event Action<float> Changed;
        public bool TryToStartRechargeProcess(float duration);
        public void StopRechargeProcess();
    }
}