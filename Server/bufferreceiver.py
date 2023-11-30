from typing import List
import socket
import threading
from message import *

class BufferReceiver:
    inbox: List[Message]
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    def __init__(self, port: int):
        self.inbox = []
        self.s.bind(("0.0.0.0", port))
        threading.Thread(target=self.listen, daemon=True).start()
        print("Server is listening on port " + str(port))

    def listen(self):
        while True:
            data, addr = self.s.recvfrom(16384)
            stringdata = data.decode("utf-8")
            if(stringdata[0] == "C"):
                stringdata = stringdata[1:]
                messagelist = stringdata.split(";")
                for message in messagelist:
                    splitmessage = message.split(",")
                    match int(splitmessage[0]):
                        case 0:
                            self.inbox.append(MovePlayerMessage(splitmessage[1:]))
                        case 2:
                            self.inbox.append(MoveItemMessage(splitmessage[1:]))
                        case 10:
                            self.inbox.append(ConnectPlayerMessage(splitmessage[1:] + [addr]))
                        case 12:
                            self.inbox.append(DisconnectPlayerMessage(splitmessage[1:]))
                        case 101:
                            self.inbox.append(TakeColaMessage(splitmessage[1:]))
                         
    def get(self) -> Message:
        if not self.inbox:
            return None
        return self.inbox.pop(0)
    def isEmpty(self) -> bool:
        return not self.inbox
