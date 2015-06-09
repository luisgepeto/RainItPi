using System;
using GPIO.Business.Interfaces;
using Raspberry.IO.GeneralPurpose;

namespace GPIO.Business
{
	public class ImageWriterManager : IImageWriterManager
	{
		public GpioConnection GpioConnection { get; set; }

		public ImageWriterManager(){
			if (GpioConnection == null)
				GpioConnection = new GpioConnection ();
		}
		public void SendAsSerial(bool[,] serialData){

		}

		public void MakePositiveTransition(ConnectorPin connectorPin){
			throw new NotImplementedException ();
		}
		public void MakeNegativeTransition(ConnectorPin connectorPin){
			throw new NotImplementedException ();
		}
	}
}

