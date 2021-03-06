﻿using L2dotNET.GameService.network.l2send;
using System;
using L2dotNET.GameService.model.vehicles;

namespace L2dotNET.GameService.network.l2recv
{
    class RequestGetOnVehicle : GameServerNetworkRequest
    {
        private int boatId;
        private int x;
        private int y;
        private int z;
        public RequestGetOnVehicle(GameClient client, byte[] data)
        {
            base.makeme(client, data);
        }

        public override void read()
        {
            boatId = readD();
            x = readD();
            y = readD();
            z = readD();
        }

        public override void run()
        {
            L2Player player = Client.CurrentPlayer;

            if (player.Boat != null)
            {
                player.sendActionFailed();
                return;
            }

            if (player.Summon != null)
            {
                player.sendSystemMessage(1523);//You should release your pet or servitor so that it does not fall off of the boat and drown!
                player.sendActionFailed();
                return;
            }

            player.BoatX = x;
            player.BoatY = y;
            player.BoatZ = z;

            if (player.knownObjects.ContainsKey(boatId))
                player.Boat = (L2Boat)player.knownObjects[boatId];

            player.broadcastPacket(new GetOnVehicle(player));
        }
    }
}
