﻿using System;

namespace ElevatorConsole_Exercise.Logic
{
    public class CabinDoorOpenedState: CabinDoorState
    {
	    private ElevatorController elevatorController;
	
	    public CabinDoorOpenedState(ElevatorController elevatorController) {
		    this.elevatorController = elevatorController;
	    }

	    
	    public bool isOpened() {
		    return true;
	    }

	    
	    public bool isOpening() {
		    return false;
	    }

	    
	    public bool isClosing() {
		    return false;
	    }

	    
	    public bool isClosed() {
		    return false;
	    }

	    
	    public void cabinDoorClosedWhenWorkingAndCabinStopped() {
		    throw new Exception();
	    }

	    
	    public void closeCabinDoorWhenWorkingAndCabinStopped() {
		    throw new Exception();
	    }

	    
	    public void openCabinDoorWhenWorkingAndCabinStopped() {
		    throw new Exception();
	    }

	    
	    public void accept(CabinDoorStateVisitor visitor) {
		    visitor.visitCabinDoorOpened(this);
	    }
    }
}
