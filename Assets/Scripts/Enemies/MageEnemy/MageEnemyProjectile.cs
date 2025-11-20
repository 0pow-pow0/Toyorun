using UnityEngine;

/// <summary>
/// Tutti gli attacchi seguono questo pattern di variabli.
/// Per ordine li tengo qui
/// </summary>
/// <typeparam name="T">Collider che utilizza l'attacco</typeparam>
///<summary>
/// Ogni proiettile consta di un collider, 
/// e di componenti per dare feedback al giocatore
/// </summary>
/// 

// Informazioni condivise da ogni attacco del mago.
// Servono alla FSM per sapere come comportarsi
public class MageEnemyAttackInformations
{
    public readonly float CAST_TIME; // Tempo necessario per lanciare incantesimo
    public readonly float DURATION_TIME; // Una volta lanciato, quanto durera' sul campo?
    public readonly float DAMAGE;
    public readonly string ANIMATION_TRIGGER;

    public MageEnemyAttackInformations(
        float castTime, 
        float durationTime,
        float damage,
        string animationTrigger)
    {
        CAST_TIME = castTime;
        DURATION_TIME = durationTime;
        DAMAGE = damage;
        ANIMATION_TRIGGER = animationTrigger;
    }
}

public class MageEnemyProjectile<T>
{
    public GameObject parent;
    public T collider;
    public GameObject background;
    public GameObject hintSlider;

    public MageEnemyProjectile()
    {
    }
}