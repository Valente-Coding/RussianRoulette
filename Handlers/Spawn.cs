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
            new Vector3(-1.581f, 991.649f, -39.020f),//
            new Vector3(2.344f, 991.656f, -42.773f),//
            new Vector3(-1.402f, 991.649f, -46.514f),//
            new Vector3(-5.625f, 991.656f, -42.787f),//
            new Vector3(1f, 991.649f, -39.930f),
            new Vector3(1.438f, 991.649f, -45.395f),
            new Vector3(-4.102f, 991.649f, -45.730f),
            new Vector3(-4.316f, 991.649f, -40.023f),
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
