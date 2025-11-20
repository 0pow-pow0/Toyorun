using UtilityShit;
using UnityEngine;

namespace ComboUtilities
{
    public class Attacco
    {
        /// <summary>
        /// Conterra' le variabili come durata animazione di attacco
        /// e ne terra' traccia se updatato in modo corretto
        /// </summary>
        public Timer timerAnimazione;
        public int danno;
        public string animationTrigger;
        public GameObject collider; // Prendo il GameObject perche' sono
        // TODO PARTICELLARE

        public Attacco(Timer tmr, int dan, string animTrigg, GameObject coll)
        {
            timerAnimazione = tmr;
            danno = dan;
            animationTrigger = animTrigg;
            collider = coll;
            collider.SetActive(false);
        }
    }

    public class Combo
    {
        public Attacco[] sequenzaAttacchi;
        public uint indexChain=0; // Numero dell'attacco attualmente attivo nella catena
        //public Timer comboCooldown; non usato XD

        public Combo(int numeroAttacchi, float cooldownTimer)
        {
            sequenzaAttacchi = new Attacco[numeroAttacchi];

        }

        // Un semplice for che richiama la funzione reset
        // di ogni istanza Timer in sequenzaAttacchi
        public void ResetAnimationTimers()
        {
            for (int i = 0; i < sequenzaAttacchi.Length; i++)
            {
                sequenzaAttacchi[i].timerAnimazione.Restart();
            }
        }

        public Attacco GetActiveAttacco()
        {
            // Serve per prevenire buggazzi del cazzo
            // Lo fa in automatico unity skrzavo
            //if (indexChain >= sequenzaAttacchi.Length) { Debug.LogError("indexChain outOfBounds"); return; }
            return sequenzaAttacchi[indexChain];
        }
    }
}