using UnityEngine;

[RequireComponent(typeof(ActivatableArea))]
public abstract class AreaBase : MonoBehaviour
{ 
    protected IActivatable Activatable => GetComponent<IActivatable>();
    
    protected virtual void OnStart()
    {
        Activatable.OnActivated += OnActivated;
        Activatable.OnDeactivated += OnDeactivated;
    }
    
    protected virtual void OnDest()
    {
        Activatable.OnActivated -= OnActivated;
        Activatable.OnDeactivated -= OnDeactivated;
    }
    protected abstract void OnActivated(GameObject sender);

    protected abstract void OnDeactivated(IActivatable activatable);
}