using System;
using System.Xml;

namespace monotifications
{
	public class Message
	{
		private XmlDocument storage = new XmlDocument();
		
		public Message ()
		{
			this.parse ("<message></message>");
			this.generateRandomID();
			
		}
		
		private void generateRandomID ()
		{
			System.Random rnd = new System.Random ();
			this ["id"] = rnd.NextDouble().ToString();
			
		}
		
		public void parse (string s)
		{
			if (s.StartsWith ("<"))
				this.storage.LoadXml (s);
			else
				return;
				
		}
		
/*		public string[] this[string index] {
			get {
				String[] r;
				
				try {
					XmlNodeList nodes = storage.GetElementsByTagName(index);
					
					//string[] r;
					int i = 0;
					foreach (XmlNode node in nodes) {
						 r[i++] = node.InnerText;
					}
				}
				
				catch {
						return null;
				}
				finally {
					return r;
				}
			}
				
		set {
			if (storage.GetElementsByTagName (index) == null)
				storage.GetElementsByTagName (index) [0].InnerText = value;
			else {
					foreach (string[] s in value) {
						XmlElement element = storage.CreateElement (index);
						element.InnerText = s;
						storage.FirstChild.AppendChild (element);
					}
				}

				
			}	
		}*/
		
		public int countMatches (string TagName)
		{
			int matches = 0;
			try {
				matches = storage.GetElementsByTagName (TagName).Count;
			} catch {
				return 0;
			}
			return matches;
		}
		
		public string[] getArray (string TagName)
		{
			if (countMatches (TagName) > 0) {
				System.Collections.Generic.List<string > list = new System.Collections.Generic.List<string> ();
				for (int i = 0; i< countMatches(TagName); i++) {
					list.Add (storage.GetElementsByTagName (TagName) [i].InnerText);
				}
				string[] s = list.ToArray ();
				return s;
			} else
				return null;
		}
		
		public void append (string TagName, string content)
		{
			XmlElement element = storage.CreateElement (TagName);
			element.InnerText = content;
			storage.FirstChild.AppendChild (element);
			
		}
		
		public void delete (string TagName, int index)
		{
			XmlNode element = storage.GetElementsByTagName (TagName) [index];
			storage.RemoveChild (element);
		}			
		
		public string this [string index] {
			get {
				string ret;
				try{
					ret = storage.GetElementsByTagName(index)[0].InnerText;
				}
				catch {
					return "";//null;
				}
				return ret;
			}
			set {
				if (storage.GetElementsByTagName(index) == null)
//				if (countMatches(index) > -1)
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
		
		
		public static void Main (String[] args)
		{
			Message m = new Message ();
			m.parse ("<html><body><p-1>p1</p-1><p-2>p2</p-2></body></html>");
			
			Console.WriteLine ("p-1="+ m ["p-1"] +";");
			m ["hello"] = "world";
			
			m ["p-1"] = "Hello World!";
			Console.WriteLine ("Hello=" + m ["hello"] +";"); 
			
			Console.WriteLine(m);
		}
	}
}