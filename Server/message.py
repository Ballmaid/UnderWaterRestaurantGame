from players import *
class Message:
    def __init__(self, var: list[str]):
        self.var = var
    def run(self, playerlist, itemlist, containerlist, stationlist, counter):
        pass
    def getString(self) -> str:
        return ""

class MovePlayerMessage(Message):
    def __init__(self, var: list[str]):
        self.playerid = int(var[0])
        self.posX = int(var[1])
    def run(self, playerlist, itemlist, containerlist, stationlist, counter):
        for player in playerlist:
            if player.id == self.playerid:
                player.pos = self.posX
                break

class MoveItemMessage(Message):
    def __init__(self, var: list[str]):
        self.itemid = int(var[0])
        self.posX = int(var[1])
        self.posY = int(var[2])

class ConnectPlayerMessage(Message):
    def __init__(self, var: list[str]):
        self.username = var[0]
        self.addr = var[1]
    def run(self, playerlist, itemlist, containerlist, stationlist, counter):
        playerlist.append(Player(self.username, self.addr, playerlist))

class DisconnectPlayerMessage(Message):
    def __init__(self, var: list[str]):
        self.playerid = int(var[0])
        


class TakeColaMessage(Message):
    def __init__(self, var: list[str]):
        # do nothing
        pass
