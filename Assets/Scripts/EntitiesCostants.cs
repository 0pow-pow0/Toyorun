///<summary>
/// Semplice file che contiene tutte le costanti utilizzate dal codice dei nemici.
/// Alcune potrebbero essere serializzate, ma preferisco averle tutte ordinate qui xD.
/// Tutti singleton
/// </summary>
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.Rendering.DebugUI;

namespace EntitiesCostants
{   
    public class LayerMaskCostants
    {
        private static LayerMaskCostants _values;

        public static LayerMaskCostants instance()
        {
            if(_values == null) { _values = new LayerMaskCostants(); }
            return _values;
        }

        //--------------- Player
        public readonly int playerBody;
        public readonly int playerAttack;
        public readonly int playerGrab;


        //--------------- Enemies
        public readonly int allEnemiesBody;
        public readonly int allEnemiesAttack;

        public readonly int simpleEnemiesBody;
        public readonly int simpleEnemiesAttack;
                                   
        public readonly int mageEnemiesBody;
        public readonly int mageEnemiesSphereAttack;
        public readonly int mageEnemiesWaveAttack;
        public readonly int mageEnemiesStraightAttack;

        public readonly int barrelsCollider;
        public readonly int barrelsActivationRange;

        private LayerMaskCostants()
        {
            playerBody       = LayerMask.GetMask("PlayerBodyCollider");
            playerAttack     = LayerMask.GetMask("PlayerAttackCollider");
            playerGrab = LayerMask.GetMask("PlayerGrabCollider");

            allEnemiesBody = LayerMask.GetMask("SimpleEnemyBodyCollider", "MageEnemyBodyCollider");
            allEnemiesAttack = LayerMask.GetMask("SimpleEnemyAttackCollider", "MageEnemyAttackCollider");

            simpleEnemiesBody = LayerMask.GetMask("SimpleEnemyBodyCollider");
            simpleEnemiesAttack = LayerMask.GetMask("SimpleEnemyAttackCollider");

            mageEnemiesBody = LayerMask.GetMask("MageEnemyBodyCollider");
            mageEnemiesSphereAttack = LayerMask.GetMask("MageEnemySphereAttackCollider");
            mageEnemiesWaveAttack = LayerMask.GetMask("MageEnemyWaveAttackCollider");
            mageEnemiesStraightAttack = LayerMask.GetMask("MageEnemyStraightAttackCollider");

            barrelsCollider = LayerMask.GetMask("BarrelCollider");
            barrelsActivationRange = LayerMask.GetMask("BarrelActivationCollider");
        } 
    }
            
    //------------------------------------ PLAYER ----------------------------------
    public class PlayerCostants
    {
        public static PlayerCostants _values;

        public static PlayerCostants instance()
        {
            if (_values == null) { _values = new PlayerCostants(); }
            return _values;
        }

        /// La velocita' e' settata nell'inspector per comodita'

        public readonly int START_HP = 2;
        public readonly int START_MAX_HP = 2;
        //public readonly int = 3;

        // ------------------------ COLORS
        // Colore di base dell'outline del player
        public readonly Color outlineColorBase = new Color(0.9749f, 1f, 0f);
        // Colore dell'outline quando diventa invincibile    
        public readonly Color outlineColorIinvincible = new Color(1f, 1f, 1f);

        // ------------------------ HURT STATE
        public readonly float HURT_DURATION_TIME = 1f;
        public readonly float HURT_INVINCIBILITY_DURATION_TIME = 3f;

        // ------------------------ GRAB STATE
        public readonly float GRAB_ANIMATION_DURATION = 0.5f;
        public readonly float GRAB_MAX_GRAB_TIME_DURATION = 3f;

        // ------------------------ THROW STATE
        // tempo in cui sta fermo dopo aver lanciato
        public readonly float THROW_STILL_DURATION = 0.8f;

        // ------------------------ STUNNED STATE
        public readonly float STUNNED_DURATION_TIME = 1.2f; // Tempo durata stun
        public readonly float STUNNED_KNOCKBACK_DURATION_TIME; // durata knowback mentre stunnato
        public readonly Vector3 STUNNED_KNOCKBACK_FORCE = new Vector3(0, 0, -10);

        // ------------------------ MOVE STATE
        public readonly float MOVE_STATE_SPEED = 25;

        private PlayerCostants() 
        {
        }
    }




    //----------------------------------- ENEMIES ----------------------------------
    public class BaseEnemyCostants
    {
        // Gli hp quando il nemico spawna :)
        public int START_HP { get; protected set; }
        public int ATTACK { get; protected set; }

        // ----------------- COLORI -----------------
        // Colore di base 
        public Color COLOR_OUTLINE_BASE { get; protected set; }

        // Quando viene hittato diventa di questo colore...
        public readonly Color COLOR_OUTLINE_JUST_HITTED = new Color(1, 1, 1);
        // ...per poi sfumarsi a questo
        public readonly Color COLOR_OUTLINE_HURT = new Color(0, 0, 0);

        // Instanziabile solo durante dai figli che ereditano
        protected BaseEnemyCostants() {  }
    }

    public class SimpleEnemyCostants : BaseEnemyCostants
    {
        public static SimpleEnemyCostants _values;
        public static SimpleEnemyCostants instance()
        {
            if (_values == null) { _values = new SimpleEnemyCostants(); }
            return _values;
        }


        // Velocita' nel girarsi verso la direzione del player
        public readonly int ROTATION_SPEED = 3;



        // ----------------- STATI -----------------
        // La convenzione che utilizzo nei nomi e' la seguente:
        // [NOMESTATO]_[INFORMAZIONI]
        // ----------------- WALKINGTO STATE
        public readonly float WALKINGTO_MIN_TIME_TO_SWITCH_APPROACH_TO = 1f;
        public readonly float WALKINGTO_MAX_TIME_TO_SWITCH_APPROACH_TO = 5f;

        public readonly float WALKINGTO_MIN_TIME_TO_SWITCH_IDLE = 0.5f;
        public readonly float WALKINGTO_MAX_TIME_TO_SWITCH_IDLE = 7f;

        // ----------------- APPROACH TO STATE
        public readonly int APPROACHTO_MIN_DISTANCE_TO_ATTACK = 20;
                               

        // ----------------- THROW STATE
        public readonly Vector3 THROWN_THROW_FORCE = new Vector3(0, 0, 150);

        // La massima distanza che puo' percorrere volando xd
        public readonly int THROWN_MAX_TRAVEL_DISTANCE = 20;

        // Tempo di volo quando viene lanciato
        public readonly float THROWN_MAX_FLIGHT_TIME = 0.65f;

        private SimpleEnemyCostants() 
        {
            // ----------------- COLORI -----------------
            COLOR_OUTLINE_BASE = new Color(1, 0, 0);

                
            START_HP = 5;
            ATTACK = 1; 
        }
    }
    public class MageEnemyCostants : BaseEnemyCostants
    {
        public static MageEnemyCostants _values;
        public static MageEnemyCostants instance()
        {
            if (_values == null) { _values = new MageEnemyCostants(); }
            return _values;
        }

        // ---------------------------- 
        public readonly float SHIELD_MIN_TIME_TO_DESTROY = 7f;
        public readonly float SHIELD_MAX_TIME_TO_DESTROY = 10f;
        public readonly float SHIELD_DESTROYED_DURATION = 4f;


        // ---------------------------- IDLE STATE
        public readonly float IDLE_MIN_TIME_TO_SWITCH = 6f;
        public readonly float IDLE_MAX_TIME_TO_SWITCH = 10f;

        // ---------------------------- CAST STATE
        // timer settati dall'attacco

        // ---------------------------- LAUNCH STATE
        public readonly float LAUNCH_TIME_TO_SWITCH = 1.5f;

        // ---------------------------- HURT STATE
        public readonly float HURT_TIME_TO_SWITCH = 3f;

        // ---------------------------- MAGIE
        // Per la spiegazione delle magie andare a vedere il codice corrispondente
        public readonly int SPHERE_ATTACK_PROJECTILES = 3;
        public readonly int SPHERE_ATTACK_DAMAGE = 1;
        public readonly string SPHERE_ATTACK_ANIM_TRIGGER = "sphereAttackTrigger";
        public readonly float SPHERE_ATTACK_CAST_DURATION = 5f;
        public readonly float SPHERE_ATTACK_MAGIC_DURATION = 2f;
        public readonly Vector3 SPHERE_ATTACK_HINT_SLIDER_MAX_DIMENSIONS = new Vector3(1, 0.1f, 1); 
        public readonly Vector3[] SPHERE_ATTACK_POSITIONS = { // Posizione locale delle sfere nell'attacco
        new Vector3(-50f, 0, -40f),
        new Vector3(0, 0, 0),
        new Vector3(50f, 0, -40f)};
        



        private MageEnemyCostants()
        {            
            START_HP = 3;
            ATTACK = 1;
        }
    }
}