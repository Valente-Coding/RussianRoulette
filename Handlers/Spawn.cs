using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RussianRoulette.Handlers
{
    class Spawn
    {
        private List<Vector3> _spawnPoints = new List<Vector3>() { 
            new Vector3(1.199f, 991.649f, -38.363f), //
            new Vector3(5.988f, 991.649f, -42.809f), //
            new Vector3(1.148f, 991.649f, -47.469f), //
            new Vector3(-4.123f, 991.649f, -42.807f),//
            new Vector3(4.383f, 991.649f, -39.570f), ////
            new Vector3(4.523f, 991.649f, -46.275f), ////
            new Vector3(-2.675f, 991.649f, -46.427f),////
            new Vector3(-2.572f, 991.649f, -39.364f),////
            new Vector3(2.910f, 991.649f, -38.660f), //////
            new Vector3(5.652f, 991.649f, -40.930f), //////
            new Vector3(5.406f, 991.649f, -44.699f), //////
            new Vector3(2.836f, 991.649f, -47.141f), //////
            new Vector3(-0.899f, 991.649f, -47.189f),//////
            new Vector3(-3.746f, 991.649f, -44.809f),//////
            new Vector3(-3.858f, 991.649f, -40.862f),//////
            new Vector3(-0.875f, 991.649f, -38.391f),//////
        }; 

        public List<Player> SpawnPlayers()
        {
            Log.Info("Spawning Players for this round.");
            List<Player> list = new List<Player>();

            int i = 0;
            foreach (Player player in Player.List)
            {
                player.Role.Set(RoleTypeId.ClassD);
                player.Position = _spawnPoints[i];

                ApplyEffectsToPlayer(player);

                list.Add(player);
                i++;
            }

            return list;
        }

        private void ApplyEffectsToPlayer(Player player)
        {
            player.EnableEffect(EffectType.Ensnared);
        }
    }
}
