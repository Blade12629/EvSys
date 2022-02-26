# EvSys
Simple Event System made for ServUO

# Installation
- Simply drop the code into your server scripts

# Usage
For example scripts check the "Events" folder

# Event flow

Events are executed in the following order:
- Finalize finished events
- Execute Events
    - Queue the successfull executed events up to be finalized in the next update
