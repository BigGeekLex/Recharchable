using System;
using UnityEngine;

public interface IActivatable
{
    public ActivatableZone GetActivatableZoneType();
    public bool TryActivate(GameObject sender);
    public void Deactivate();
    
    public event Action<GameObject> OnActivated;

    public event Action<IActivatable> OnDeactivated;
}