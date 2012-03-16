using System;
using System.IO;
using System.Collections.Generic;

namespace monotifications
{
	public class configuration
	{
		private string filename;
		
		private Dictionary <string,configurationGroup> storage = new Dictionary<string, configurationGroup>();

		public configuration () : this("config.ini")
		{
		}
		
		~configuration() {
			//Console.WriteLine("saving configuration before exit");
			save();
		}
		
		public configuration (string filename)
		{
			this.filename = filename;
			this.load ();
		}
		
		public  configurationGroup this [string key]{
			get {
				//return storage [key];
				
				if (!storage.ContainsKey(key))	{
					Console.Error.WriteLine("Configuring new group {0}...",key);
					storage[key] = new configurationGroup();
				}
				
					
				return storage[key];
			}
			/*set {
				storage [key] = value;
			}*/
		}
		
		public override string ToString ()
		{
			return dump ();
		}
		
		public string dump ()
		{
			string str = "";
			
			foreach (KeyValuePair<string,configurationGroup> kvp in storage) {
				//str += String.Format ("{0}\t= {1}\n", kvp.Key, kvp.Value)				
				if (kvp.Key != "") 
					str += String.Format ("[{0}]\n{1};[/{0}]\n\n", kvp.Key, kvp.Value);
			}
			return str;
		}
		
		public void load() {
			this.load(this.filename);
		}
		
		public void load (string filename)
		{
			FileInfo finfo = new FileInfo (filename);
			if (finfo.Exists) {
				// create reader & open file
				StreamReader streamReader = new StreamReader (filename);

				// read file
				string text = streamReader.ReadToEnd ();
				// close the stream
				streamReader.Close ();
			
				this.parse (text);
			} else
				Console.WriteLine ("File {0} is inaccessible.", filename);
		}
		
		public void save ()
		{
			this.save (this.filename);
		}
		
		public void save (string filename)
		{
			try {
				FileInfo finfo = new FileInfo (filename);
				if (finfo.Exists) {
					Console.WriteLine("File {0} exists. Deleting file...", filename);
					finfo.Delete ();
				}
				
				string text = dump ();		
				StreamWriter streamWriter = new StreamWriter (filename);
				streamWriter.Write (text);
				streamWriter.Close ();
			} catch (Exception e) {
				Console.WriteLine ("Failed: " + e.ToString ());
			}
		}
		
		
		public void parse (string input)
		{
			string container = "";
			foreach (String line in input.Split('\n')) {
				line.Trim ();
				if (!(line.StartsWith (";") || line.StartsWith ("#"))) { // Not a comment
					if (line.Contains ("=")) { // If fully qualified key=value
						
						String line2;
						
						if (line.Contains (";"))
							line2 = line.Split (';') [0];
						else
							line2 = line;
						
						string key = line2.Split ('=') [0];
						string val = line2.Split ('=') [1];
						
						key = key.Trim ();
						val = val.Trim ();
											
						this[container][key] = val;
					} else if (line.Contains ("[") && line.Contains ("]")) {
						container = line.Substring(line.IndexOf("[")+1,line.IndexOf("]")-1);
					}
				}
			}
		}
		
		public static void __Main (String[] args)
		{
			configuration config = new configuration ();
			/*
			//config["grp1"]["key1"] = "foo!";
			
			//configurationGroup g = new configurationGroup ();
			
			//config ["flinstones"] ["barney"] = "Betty";
			
			//g ["foo"] = "bar";
			//g ["fred"] = "wilma";
			//config ["flinstones"] ["fred"] = "Wilma";
			//config ["flinstones"] = g;
			//config ["flinstones"] ["barney"] = "Betty";
			
			Console.WriteLine ("{0}", config);
			Console.WriteLine ("{0}", config ["flinstones"] ["foo"]);
			//config ["flinstones"] ["barney"] = "Betty";
			
			config.parse (@";This is a sample configuration file
key1 = val1
key2 = val2
[group]
key1 = hello
key2 = world
key3=!!!!!
key2=galaxy
[yet another group]
this is a keyword     =     and this is a value ; and just a dummy comment
");
			
			config ["foo"] ["foo1"] = "val1";
			config ["foo"] ["foo3"] = "val3";
			config ["foo"] ["foo2"] = "val2";
			*/
			Console.WriteLine (config);
			config.save ();			
			/*
			
			configuration config2 = new configuration ();
			config2.parse (config.ToString ());
						
			Console.WriteLine (config2);*/
			
		}
	}
}