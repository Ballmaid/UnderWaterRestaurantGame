from players import *
class Message:
    def __init__(self, var: list[str]):
        self.var = var
    def run(self, playerlist, itemlist, containerlist, stationlist, counter, distributor):
        pass
    def getString(self) -> str:
        return ""

class MovePlayerMessage(Message):
    def __init__(self, var: list[str]):
        self.playerid = int(var[0])
        self.posX = int(var[1])
    def run(self, playerlist, itemlist, containerlist, stationlist, counter, distributor):
        for player in playerlist:
            if player.id == self.playerid:
                player.pos = self.posX
                distributor.sendExcept(sendPlayerStatus([self.playerid, self.posX]), player)
                break

class MoveItemMessage(Message):
    def __init__(self, var: list[str]):
        self.itemid = int(var[0])
        self.posX = int(var[1])
        self.posY = int(var[2])
    def run(self, playerlist, itemlist, containerlist, stationlist, counter, distributor):  
        for item in itemlist:
            if item.id == self.itemid:
                item.posX = self.posX
                item.posY = self.posY
                distributor.sendAll(MoveItemStatus([self.itemid, self.posX, self.posY]))
                break

class ConnectPlayerMessage(Message):
    def __init__(self, var: list[str]):
        self.username = var[0]
        self.addr = var[1]
    def run(self, playerlist, itemlist, containerlist, stationlist, counter, distributor):
        playerlist.append(Player(self.username, self.addr, playerlist))
        message = serverStatus(["Server", playerlist[-1].id, playerlist[-1].UserName])
        print("Message = " + message.getString())
        for player in playerlist:
            distributor.sendAll(message)

class DisconnectPlayerMessage(Message):
    def __init__(self, var: list[str]):
        self.playerid = int(var[0])        


class TakeColaMessage(Message):
    def __init__(self, var: list[str]):
        # do nothing
        pass

# =========== Server -> Client ===========

class sendPlayerStatus(Message):
    def __init__(self, var: list[str]):
        self.playerid = int(var[0])
        self.posX = int(var[1])
    def getString(self) -> str:
        return "1," + str(self.playerid) + "," + str(self.posX)
    
class MoveItemStatus(Message):
    def __init__(self, var: list[str]):
        self.itemid = int(var[0])
        self.posX = int(var[1])
        self.posY = int(var[2])
    def getString(self) -> str:
        return "3," + str(self.itemid) + "," + str(self.posX) + "," + str(self.posY)
    
class serverStatus(Message):
    def __init__(self, var: list[str]):
        self.serverName = var[0]
        self.playerID = int(var[1])
        self.playerName = var[2]
    def getString(self) -> str:
        return "11," + self.serverName + "," + str(self.playerID) + "," + self.playerName