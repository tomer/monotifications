using System;
using System.Xml;

namespace monotifications
{
	public class Message
	{
		private XmlDocument storage = new XmlDocument();
		
		public Message ()
		{
			this.parse("<message></message>");
		}
		
		public void parse (string s)
		{
			if (s.StartsWith("<")) this.storage.LoadXml (s);	
			else return;
				
		}
		
		public string this [string index] {
			get {
				string ret;
				try{
					ret = storage.GetElementsByTagName(index)[0].InnerText;
				}
				catch {
					return null;
				}
				return ret;
			}
			set {
				if (storage.GetElementsByTagName(index) == null)
					storage.GetElementsByTagName(index)[0].InnerText = value;
				else {
					XmlElement element = storage.CreateElement(index);
					element.InnerText = value;
					storage.FirstChild.AppendChild(element);
				}
			}
		}
		
		public override string ToString ()
		{
			//return string.Format ("[Messagethis={0}]", this);
			string ret;
			
			ret = storage.OuterXml;
			
			return ret;
		}
		
		
		public static void _Main (String[] args)
		{
			Message m = new Message ();
			m.parse ("<html><body><p-1>p1</p-1><p-2>p2</p-2></body></html>");
			
			Console.WriteLine (m ["p-1"]);
			m ["hello"] = "world";
			
			m ["p-1"] = "Hello World!";
			Console.WriteLine ("Hello= " + m ["Hello"]); 
			
			Console.WriteLine(m);
		}
	}
}