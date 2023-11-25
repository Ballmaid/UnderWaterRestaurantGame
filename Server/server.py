from bufferreceiver import *
from containitems import *
from counter import *
from players import *
import threading
import time


class Controller:
    playerlist = []
    itemlist = []
    containerlist = []
    stationlist = []
    counter = Counter()
    def __init__(self):
        self.playerlist = []
        self.itemlist = []
        self.containerlist = []
        self.stationlist = []
        self.counter = Counter()
        threading.Thread(target=self.work, daemon=True).start()
    
    def work(self):
        while True:
            if not b_r.isEmpty():
                b_r.get().run(self.playerlist, self.itemlist, self.containerlist, self.stationlist, self.counter)
        
            


def qkill():
    global keep_running
    keep_running = True
    while True:
        if input() == "exit":
            keep_running = False
            print("Goodbye")
            exit()

#=========== MAIN ===========
print("the server is starting")
b_r = BufferReceiver(1008)
controller = Controller()

threadkill = threading.Thread(target=qkill, daemon=True).start()
print("Type exit to close the server")
while keep_running:
    time.sleep(1)