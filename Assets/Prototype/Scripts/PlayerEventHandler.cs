using UnityEngine;
using UnityEngine.Events;

public class PlayerEventHandler : MonoBehaviour, IDamageable
{
    public UnityEvent OnPlayerDeathEvent;



    void IDamageable.OnDamange()
    {
        OnPlayerDeathEvent.Invoke();
    }
}
