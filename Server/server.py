import configparser
import socket


def loadConfig():
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
    print("received message: " + stringdata)




print("the server is starting")
#load config.ini
loadConfig()
setupNetwork()
print("the server is ready")
while True:
    listenCommand()
    

