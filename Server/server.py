import configparser
import socket
import threading
import time


bufferlist = []
playerlist = []
itemslist = []
def loadConfig():
    global Maxplayers, ServerName, Password
    config = configparser.ConfigParser()
    config.read('config.ini')
    Maxplayers = config['DEFAULT']['MaxPlayers']
    print("Max Players = " + Maxplayers)
    ServerName = config['DEFAULT']['ServerName']
    print("Server Name = " + ServerName)
    Password = config['DEFAULT']['Password']
    print("Password = " + Password)

def setupNetwork():
    global s
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.bind(("0.0.0.0", 1008))
    print("Server is listening on port 1008")

def listenCommand():
    data, addr = s.recvfrom(1024)
    stringdata = data.decode("utf-8")
    #stringdata is in format "msgtype,var1,var2,var3..." 
    #msgtype 
    #1X is for player
    #2X is for items  
    #If stringdata begins with C
    if stringdata[0] == "C":
        globalmessagelist = stringdata.split(";")
        #remove the C
        globalmessagelist[0] = globalmessagelist[0][1:]
        for globalmessage in globalmessagelist:
            messagelist = globalmessage.split(",")
            messagetype = messagelist[0]
            match messagetype:
                case "0":                           #player is moving
                    movePlayer(messagelist[1], messagelist[2], addr)
                case "2":                           #player is moving item
                    moveItem(messagelist[1], messagelist[2], messagelist[3])
                case "10":                          #player connected
                    connectPlayer(messagelist[1], addr)
                case "12":                          #player disconnected
                    disconnectPlayer(messagelist[1])
                case "101":                        #takeCola
                    Station1.takeCola()
            
            for buffer in bufferlist:
                if buffer.addr == addr:
                    buffer.flush()
                    break




def qkill():
    global keep_running
    keep_running = True
    while True:
        if input() == "exit":
            keep_running = False
            s.close()
            print("server is closing")

def connectPlayer(UserName, addr):
    print("Player " + UserName + " connected")
    playerlist.append(Player(UserName, addr))
    bufferlist.append(Buffer(addr))
    for clientplayer in playerlist:
        clientplayer.announceMyself()
    
def disconnectPlayer(UserName):
    print("Player " + UserName + " disconnected")
    for player in playerlist:
        if player.UserName == UserName:
            playerlist.remove(player)
            break

def movePlayer(ID, pos, addr):
    for player in playerlist:
        if player.id == int(ID):
            player.pos = pos
            sendPlayerStatus(addr)
            break

def sendPlayerStatus(addr):
    message = "1,"
    for player in playerlist:
        message += str(player.id) + "," + str(player.pos) + ","
    message = message[:-1]
    sendCommand(message, addr)

def sendCommand(command, addr):
    for buffer in bufferlist:
        if buffer.addr == addr:
            buffer.add(command)
            break

def sendUDP(command, addr):
    s.sendto(command.encode("utf-8"), addr)

def createItem(ItemType, State):
    ItemID = 0
    for i in range(0, 10000):
        if not any(item.id == i for item in itemslist):
            ItemID = i
            break
    itemslist.append(Item(ItemID, ItemType, State))
    for clientplayer in playerlist:
        sendCommand("20," + ItemID + "," + ItemType + "," + State, clientplayer.addr)

def destroyItem(ItemID):
    for item in itemslist:
        if item.id == int(ItemID):
            itemslist.remove(item)
            break
    for clientplayer in playerlist:
        sendCommand("21," + ItemID, clientplayer.addr)

def changeItemState(ItemID, NewState):
    for item in itemslist:
        if item.id == int(ItemID):
            item.state = NewState
            break
    for clientplayer in playerlist:
        sendCommand("22," + ItemID + "," + NewState, clientplayer.addr)

def moveItem(ItemID, NewPosX, NewPosY):
    for item in itemslist:
        if item.id == int(ItemID):
            item.posX = NewPosX
            item.posY = NewPosY
            break

def moveItemStatus(ItemID, NewPosX, NewPosY):
    for clientplayer in playerlist:
        sendCommand("1," + ItemID + "," + NewPosX + "," + NewPosY, clientplayer.addr)

class Player:
    UserName = ""
    addr = ""
    id = 0
    pos = 0
    def __init__(self, UserName, addr):
        print("Init player " + UserName)
        self.UserName = UserName
        self.addr = addr
        for i in range(0, 1000):
            if not any(player.id == i for player in playerlist):
                self.id = i
                break
    
    def announceMyself(self):
        for clientplayer in playerlist:
            print("Sending 11," + ServerName + "," + str(self.id) + "," + self.UserName + " to " + clientplayer.UserName + " at " + str(clientplayer.addr))
            sendCommand("11," + ServerName + "," + str(self.id) + "," + self.UserName, clientplayer.addr)


class Buffer:
    addr = ""
    buffer = "S"
    def __init__(self, addr):
        self.addr = addr
    def add(self, data):
        if self.buffer == "S":
            self.buffer += data
        else:
            self.buffer += ";" + data

    def flush(self):
        if self.buffer != "S":
            sendUDP(self.buffer, self.addr)
            self.buffer = "S"



#======Items======
#State is a string legth 16 used to store infos about the item
class Item:
    posX = 0
    posY = 0
    id = 0
    state = "0000000000000000"
    def __init__(self, posX, posY, state):
        self.posX = posX
        self.posY = posY
        self.state = state
        for i in range(0, 10000):
            if not any(item.id == i for item in itemslist):
                self.id = i
                break


class Cola(Item): #ID 0
    opened = False
    def __init__(self, pos):
        super().__init__(pos)
    def open(self):
        self.opened = True
        self.state = "0000000000000001"


#======Stations======

class SnackStation:
    posX = 0
    def __init__(self, posX):
        self.posX = posX
    def takeCola(self):
        createItem(0, "0000000000000000")
        moveItem(itemslist[-1].id, self.posX+10, 50)
        




print("the server is starting")
loadConfig()
setupNetwork()
threadkill = threading.Thread(target=qkill, daemon=True).start()
print("Type exit to close the server")
tick_time = time.time()
Station1 = SnackStation(100)
while keep_running:
    tick_time = time.time()
    listenCommand()
    #Calculate tickrate in ms with accuracy 0.1ms and divide by amount of players

    tickrate = round(((time.time() - tick_time) * 1000) * len(playerlist), 1)
    print("Tickrate: " + str(tickrate) + "ms")