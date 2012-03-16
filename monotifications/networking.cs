using System;
using System.Net;
using System.Net.Sockets;


namespace monotifications
{
	public class networking
	{
		private int _listenPort;
		private Boolean _listen;
		
		public delegate void ReceiverDelegate(string text);
		private ReceiverDelegate ReceiverCallback;
		
		public void setReceiveAction (ReceiverDelegate rd)
		{
			ReceiverCallback = rd;
		}
		
		public void clearReceiveAction() {
			setReceiveAction(DefaultReceiveAction);
		}
		
		public networking()
		{
			clearReceiveAction();
		}
		
		public int listenPort {
			get {
				return _listenPort;
			}
			set {
				_listenPort = value;
			}
		}
		
		public void talker (string recipientIP, int recipientPort, string text_to_send)
		{
			Boolean exception_thrown = false;

			Socket sending_socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram,
			ProtocolType.Udp);

			IPAddress send_to_address = IPAddress.Parse (recipientIP);

			IPEndPoint sending_end_point = new IPEndPoint (send_to_address, recipientPort);

			byte[] send_buffer = System.Text.Encoding.UTF8.GetBytes (text_to_send);
			
// Remind the user of where this is going.
			//Console.WriteLine ("sending to address: {0} port: {1}",			sending_end_point.Address,				sending_end_point.Port);
			try {
				sending_socket.SendTo (send_buffer, sending_end_point);
			} catch (Exception send_exception) {
				exception_thrown = true;
				Console.WriteLine (" Exception {0}", send_exception.Message);
			}
			if (exception_thrown == false) {
				//Console.WriteLine ("Message has been sent to the broadcast address");
			} else {
				exception_thrown = false;
				Console.WriteLine ("The exception indicates the message was not sent.");
			}
		}
		
		private class UdpState{
			public UdpClient u;
			public IPEndPoint e;
		}
		
		public void ReceiveCallback (IAsyncResult ar)
		{
			UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).u;
			IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;
			
			Byte[] receiveBytes = u.EndReceive (ar, ref e);
			string receiveString = System.Text.Encoding.UTF8.GetString (receiveBytes);
			
			//Console.WriteLine ("Received: {0}", receiveString);
			//ShowReceivedMessage(receiveString);
			
			if (_listen) {
				ReceiverCallback (receiveString);
				listen (); // Wait for next message...	
			}
		}
		
		public void DefaultReceiveAction(string text) {
			Console.WriteLine ("Received: {0}", text);
		}
		
		public void listen ()
		{	
			_listen = true;
			
			IPEndPoint e = new IPEndPoint (IPAddress.Any, _listenPort);
			UdpClient u = new UdpClient (e);
			
			UdpState s = new UdpState ();
			s.e = e;
			s.u = u;
			
			Console.WriteLine ("listening for messages on port {0}", _listenPort);
			u.BeginReceive (new AsyncCallback (ReceiveCallback), s);
		}
		
		public void stopListener ()
		{
			_listen = false;
			talker("127.0.0.1", listenPort, "shutdown now");
		}
				
		private static void _Main(String[] args)
		{
			networking net = new networking ();
			
			net.listenPort = 7777;
			Console.WriteLine ("listening on port {0}", net.listenPort);
			
			net.listen ();
			
			for (int i=0; i<10; i++)
				net.talker ("127.0.0.1", 7777, "Hello World!");			
		}		
	}
}