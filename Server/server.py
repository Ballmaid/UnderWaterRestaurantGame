import configparser
import socket
import threading
import time


bufferlist = []
playerlist = []

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
                case "10":                          #player connected
                    connectPlayer(messagelist[1], addr)
                case "12":                          #player disconnected
                    disconnectPlayer(messagelist[1])
            
            for buffer in bufferlist:
                if buffer.addr == addr:
                    buffer.flush()
                    break
            time.sleep(0.1)




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
    for player in playerlist:
        if player.addr == addr:
            player.announceMyself()
            break
    
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
                print("My id is " + str(self.id))
                break
        
        for clientplayer in playerlist:
            
            print("sending " + "11," + ServerName + "," + str(self.id) + "," + self.UserName + " to " + str(clientplayer.addr))
            sendCommand("11," + ServerName + "," + str(self.id) + "," + self.UserName, clientplayer.addr)
    
    def announceMyself():
        for clientplayer in playerlist:
            print("0th Player has ID " + playerlist[0].id)
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




print("the server is starting")
loadConfig()
setupNetwork()
threadkill = threading.Thread(target=qkill, daemon=True).start()
print("Type exit to close the server")

while keep_running:
    listenCommand()