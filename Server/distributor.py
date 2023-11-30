from players import *
from message import *

class Distributor:
    controller = None
    def __init__ (self, controller):
        self.controller = controller
    def sendAll(self, message):
        playerlist = self.controller.playerlist
        for player in playerlist:
            player.getBufferSender().put(message)
    def sendOne(self, message, player):
        playerlist = self.controller.playerlist
        for player in playerlist:
            if player.id == player.id:
                player.getBufferSender().put(message)
                break
    def sendExcept(self, message, player):
        playerlist = self.controller.playerlist
        for player in playerlist:
            if player.id != player.id:
                player.getBufferSender().put(message)

    