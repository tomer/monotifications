using System;
using System.Collections.Generic;

namespace monotifications
{
	/*public class configurationGroup{
		private Dictionary<string,string> storage = new Dictionary<string, string>();
		
		public string this[string key]  {
			get {
				return storage[key];
			}
			set {
				storage[key]=value;
			}
		}
	}*/
	public class configurationGroup
	{
		//Dictionary <string,Dictionary<string,string>> storage = new Dictionary<string,Dictionary<string,string>>();
		private Dictionary <string,string> storage = new Dictionary<string,string>();
		
		public configurationGroup ()
		{
		}
		
/*		public override string ToString ()
		{
			string str = "";
			foreach (KeyValuePair<string,Dictionary<string,string>> grp in storage) {
				str += String.Format ("[{0}]\n", grp.Key);
				
				foreach (KeyValuePair<string,string> kvp in grp.Value) {
					str += String.Format ("\t{0}\t= {1}\n", kvp.Key, kvp.Value);
				}
			}
			
			return str;
		}*/
		
		public override string ToString ()
		{
			return dump();
		}
		
		public string dump() {
			string str = "";
			foreach (KeyValuePair<string,string> kvp in storage) {
				str += String.Format ("\t{0}\t= {1}\n", kvp.Key, kvp.Value);
			}
			return str;
		}
	
/*		public string this [string grp, string key] {
			get {
				return storage[grp][key];
			}
			set {
				storage[grp][key] = value;	
			}
		}*/
		
		//public Dictionary<string,string> this [string key] {
		public string this [string key] {
			get {
				if (storage.ContainsKey(key)) return storage [key];
				else return null; // or */ "";
			}
			set {
				storage [key] = value;	
			}
		}
				
		/*		public string this [string grp, string key] {
			get {
				return storage[grp][key];
			}
			set {
				storage[grp][key] = value;
			}
		}*/
		
/*		public this{
			this.parse(Value);
		}*/
		
		public void parse (string input)
		{
			foreach (String line in input.Split('\n')) {
				line.Trim ();
				if (!(line.StartsWith (";") || line.StartsWith ("#"))) { // Not a comment
					if (line.Contains ("=")) { // If fully qualified key=value
						
						String line2;
						
						if (line.Contains(";")) line2 = line.Split (';') [0];
							else line2 = line;
						
						string key = line2.Split ('=') [0];
						string val = line2.Split ('=') [1];
						
						key = key.Trim ();
						val = val.Trim ();
												
						storage[key] = val;
					}
				}
			}
		}
		
		public static void _Main (String[] args)
		{
			configurationGroup config = new configurationGroup ();
			
//			Dictionary<string,string > d = new Dictionary<string, string> ();
			//d ["key1"] = "value";
			//d ["key2"] = "value";
			//d ["key3"] = "value";
			
			//config ["foo"] = d;
			config ["key1"] = "value";
			config ["key2"] = "value2";
			
//			config["key1"]["key2"] = "Value!";
//			config ["group", "key"] = "value!";
			//Console.WriteLine ("{0}", config ["foo"]);
			
		
			config.parse (";comment=true\n ## Another comment goes here \n" +
				"foo=bar\n" +
				"hello   = world;\n" +
				"hello        = universe\n");
			
			Console.WriteLine ("{0}", config);
		}
	
	}
}

