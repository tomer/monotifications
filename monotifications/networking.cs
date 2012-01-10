using System;
using System.Net;
using System.Net.Sockets;


namespace monotifications
{
	public class networking
	{
		private int _listenPort;
		private delegate void listenerCallback(string text);
		
		public networking ()
		{
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
			//Boolean done = false;
			Boolean exception_thrown = false;
			
#region comments
// Create a socket object. This is the fundamental device used to network
// communications. When creating this object we specify:
// Internetwork: We use the internet communications protocol
// Dgram: We use datagrams or broadcast to everyone rather than send to
// a specific listener
// UDP: the messages are to be formated as user datagram protocal.
// The last two seem to be a bit redundant.
#endregion
			Socket sending_socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram,
			ProtocolType.Udp);

#region comments
// create an address object and populate it with the IP address that we will use
// in sending at data to. This particular address ends in 255 meaning we will send
// to all devices whose address begins with 192.168.2.
// However, objects of class IPAddress have other properties. In particular, the
// property AddressFamily. Does this constructor examine the IP address being
// passed in, determine that this is IPv4 and set the field. If so, the notes
// in the help file should say so.
#endregion

			IPAddress send_to_address = IPAddress.Parse (recipientIP);

#region comments
// IPEndPoint appears (to me) to be a class defining the first or final data
// object in the process of sending or receiving a communications packet. It
// holds the address to send to or receive from and the port to be used. We create
// this one using the address just built in the previous line, and adding in the
// port number. As this will be a broadcase message, I don't know what role the
// port number plays in this.
#endregion

			IPEndPoint sending_end_point = new IPEndPoint (send_to_address, recipientPort);

#region comments
// The below three lines of code will not work. They appear to load
// the variable broadcast_string witha broadcast address. But that
// address causes an exception when performing the send.
//
//string broadcast_string = IPAddress.Broadcast.ToString();
//Console.WriteLine("broadcast_string contains {0}", broadcast_string);
//send_to_address = IPAddress.Parse(broadcast_string);
#endregion

			Console.WriteLine ("Enter text to broadcast via UDP.");
			//string 
			text_to_send = Console.ReadLine ();
// the socket object must have an array of bytes to send.
// this loads the string entered by the user into an array of bytes.
			//byte[] send_buffer = System.Text.Encoding.ASCII.GetBytes (text_to_send);
			byte[] send_buffer = System.Text.Encoding.UTF8.GetBytes (text_to_send);
			
// Remind the user of where this is going.
			Console.WriteLine ("sending to address: {0} port: {1}",
				sending_end_point.Address,
				sending_end_point.Port);
			try {
				sending_socket.SendTo (send_buffer, sending_end_point);
			} catch (Exception send_exception) {
				exception_thrown = true;
				Console.WriteLine (" Exception {0}", send_exception.Message);
			}
			if (exception_thrown == false) {
				Console.WriteLine ("Message has been sent to the broadcast address");
			} else {
				exception_thrown = false;
				Console.WriteLine ("The exception indicates the message was not sent.");
			}
		}
		
		/*
		public void listener ()
		{
			listener (_listenPort);
		}*/
		
		
		/*public Delegate listenerRegisterCallback(Delegate d) {	
			listenerCallback = d;
		}*/
		
		#region Listener thread methods

		/*private System.Threading.Thread listenerThread;
		
		public bool listenerAlive {
			get{ return listenerThread.IsAlive;}
		}
		
		public void listenerStart ()
		{
			//if (!listenerThread.IsAlive) {
			/ *		System.Threading.ThreadStart ts1 = this.listener;
				listenerThread = new System.Threading.Thread (ts1);
				listenerThread.Start();* /
			//}
			
			listen
		}/*

		public void listenerStop ()
		{
			if (listenerThread.IsAlive) {
				try {
				listenerThread.Abort ();
				} catch {
					
				}
			}
		}
		
		#endregion
		
		/*public int listener (int listenPort)
		{
			bool done = false;
			UdpClient listener = new UdpClient (listenPort);
			IPEndPoint groupEP = new IPEndPoint (IPAddress.Any, listenPort);
			string received_data;
			byte[] receive_byte_array;
			try {
				while (!done) {
					Console.WriteLine ("Waiting for broadcast");
// this is the line of code that receives the broadcase message.
// It calls the receive function from the object listener (class UdpClient)
// It passes to listener the end point groupEP.
// It puts the data from the broadcast message into the byte array
// named received_byte_array.
// I don't know why this uses the class UdpClient and IPEndPoint like this.
// Contrast this with the talker code. It does not pass by reference.
// Note that this is a synchronous or blocking call.
					
					receive_byte_array = listener.Receive (ref groupEP);
					Console.WriteLine ("Received a broadcast from {0}", groupEP.ToString ());
					//received_data = System.Text.Encoding.ASCII.GetString (receive_byte_array, 0, receive_byte_array.Length);
					received_data = System.Text.Encoding.UTF8.GetString(receive_byte_array, 0, receive_byte_array.Length);
					Console.WriteLine ("data follows \n{0}\n\n", received_data);
					
					if (received_data == "foo") listenerStop();
				}
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
			}
			listener.Close ();
			return 0;
		}*/
		
		#region AsyncListener with beginReceive
		private bool messageReceived;
		
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
			
			Console.WriteLine ("Received: {0}", receiveString);
			messageReceived = true;
			
			listen(); // Wait for next message...
		}
		
		public void listen ()
		{
			
			messageReceived = false;
			
			IPEndPoint e = new IPEndPoint (IPAddress.Any, _listenPort);
			UdpClient u = new UdpClient (e);
			
			UdpState s = new UdpState ();
			s.e = e;
			s.u = u;
			
			Console.WriteLine ("listening for messages");
			u.BeginReceive (new AsyncCallback (ReceiveCallback), s);
			
			/*
			while (!messageReceived) {
				System.Threading.Thread.Sleep (100); 
				Console.Write ("{0}", messageReceived ? "1" : "");
			}//*/
		}
		
		#endregion	
		
		public static void Main (String[] args)
		{
			networking net = new networking ();
			
			/*System.Threading.ThreadStart ts1 = net.listener(7777);
			//System.Threading.ThreadStart ts2 = net.talker("127.0.0.1", 7777, "Hello World!");
			
			System.Threading.Thread t1 = new System.Threading.Thread(ts1);
			
			t1.Start();//*/
			
			/*System.Threading.Thread thread = new System.Threading.Thread (() => {
				net.listener (7777); });
			thread.Start ();//*/
			
			
			
			//System.Console.ReadLine ();
			net.listenPort = 7777;
			Console.WriteLine ("listening on port {0}", net.listenPort);
			//net.listenerStart ();
			
			/*while (net.listenerAlive) {*/
			
			net.listen ();
			
			/*System.Threading.ThreadStart ts1 = net.listen();
			System.Threading.Thread t1 = new System.Threading.Thread (ts1);
			t1.Start(); //*/
			
			for (int i=0; i<10; i++)
				net.talker ("127.0.0.1", 7777, "Hello World!");
		
			
		}
		
	}
}

#endregion