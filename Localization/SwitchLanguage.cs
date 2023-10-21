using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RussianRoulette.Localization
{
    public class SwitchLanguage
    {
        public static SwitchLanguage Instance = new SwitchLanguage();

        public string WAIT_TO_START_1 = "Next round is starting in ";
        public string WAIT_TO_START_2 = " seconds";
        public string START_COMMAND_MESSAGE = "Russian Roulette Event Activated. Starts Next Round.";
        public string USE_ADRENALINE = "You are protected.";
        public string USE_COIN = "Someone spinned the barrel! Everyone lost the bullet.";
        public string USE_KEYCARD = "Not today! Turn skipped.";
        public string USE_FLASHLIGHT_1 = "The bullet is ";
        public string USE_FLASHLIGHT_2 = " shots away.";
        public string BULLET_WASTED = "The bullet was wasted. Reloading a new one.";
        public string NO_BULLET = "There was no bullet.";
        public string PLAYER_DEAD_1 = "The player ";
        public string PLAYER_DEAD_2 = " died.";
        public string PLAYER_SHIELD_1 = "The player ";
        public string PLAYER_SHIELD_2 = " had shild activated. Reloading a new bullet.";
        public string PLAYER_WON = " won this round! What a lucky guy.";
        public string EVERYBODY_DIED = "Everybody died!";


        public void To(string lang)
        {
            Log.Info(lang);
            switch (lang.ToLower())
            {
                case "en":
                    SwitchToEnglish();
                    break;
                case "es":
                    SwitchToSpanish();
                    break;
                case "pt":
                    SwitchToPortuguese();
                    break;
            }
        }

        private void SwitchToEnglish()
        {
            WAIT_TO_START_1 = "Next round is starting in ";
            WAIT_TO_START_2 = " seconds";
            START_COMMAND_MESSAGE = "Russian Roulette Event Activated. Starts Next Round.";
            USE_ADRENALINE = "You are protected.";
            USE_COIN = "Someone spinned the barrel! Everyone lost the bullet.";
            USE_KEYCARD = "Not today! Turn skipped.";
            USE_FLASHLIGHT_1 = "The bullet is ";
            USE_FLASHLIGHT_2 = " shots away.";
            BULLET_WASTED = "The bullet was wasted. Reloading a new one.";
            NO_BULLET = "There was no bullet.";
            PLAYER_DEAD_1 = "The player ";
            PLAYER_DEAD_2 = " died.";
            PLAYER_SHIELD_1 = "The player ";
            PLAYER_SHIELD_2 = " had shild activated. Reloading a new bullet.";
            PLAYER_WON = " won this round! What a lucky guy.";
            EVERYBODY_DIED = "Everybody died!";
        }

        private void SwitchToSpanish()
        {
            WAIT_TO_START_1 = "El siguiente ronda comenzará en ";
            WAIT_TO_START_2 = " segundos";
            START_COMMAND_MESSAGE = "Evento de Ruleta Rusa activado. Comienza la siguiente ronda.";
            USE_ADRENALINE = "Estás protegido.";
            USE_COIN = "¡Alguien giró el tambor! Todos perdieron la bala.";
            USE_KEYCARD = "¡Hoy no! Se saltó el turno.";
            USE_FLASHLIGHT_1 = "La bala está a ";
            USE_FLASHLIGHT_2 = " disparos de distancia.";
            BULLET_WASTED = "La bala se perdió. Recargando una nueva.";
            NO_BULLET = "No había bala.";
            PLAYER_DEAD_1 = "El jugador ";
            PLAYER_DEAD_2 = " murió.";
            PLAYER_SHIELD_1 = "El jugador ";
            PLAYER_SHIELD_2 = " activó un escudo. Recargando una nueva bala.";
            PLAYER_WON = " ganó esta ronda. ¡Qué suertudo!";
            EVERYBODY_DIED = "¡Todos murieron!";
        }

        private void SwitchToPortuguese()
        {
            WAIT_TO_START_1 = "A próxima rodada começa em ";
            WAIT_TO_START_2 = " segundos";
            START_COMMAND_MESSAGE = "Evento de Roleta Russa ativado. Começa a próxima rodada.";
            USE_ADRENALINE = "Estás protegido.";
            USE_COIN = "Alguém girou o tambor! Todos perderam a bala.";
            USE_KEYCARD = "Hoje não! O turno foi pulado.";
            USE_FLASHLIGHT_1 = "A bala está a ";
            USE_FLASHLIGHT_2 = " tiros de distância.";
            BULLET_WASTED = "A bala foi desperdiçada. Recarregando uma nova.";
            NO_BULLET = "Não havia bala.";
            PLAYER_DEAD_1 = "O jogador ";
            PLAYER_DEAD_2 = " morreu.";
            PLAYER_SHIELD_1 = "O jogador ";
            PLAYER_SHIELD_2 = " ativou um escudo. Recarregando uma nova bala.";
            PLAYER_WON = " ganhou esta rodada! Que sortudo!";
            EVERYBODY_DIED = "Todos morreram!";
        }
    }
}
