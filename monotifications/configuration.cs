using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace monotifications
{
	public class Configuration
	{
		private string filename = "config.ini";
		private bool saveScheduled = false;
		private Dictionary <string,configurationGroup> storage = new Dictionary<string, configurationGroup>();

		public Configuration () : this("config.ini")
		{
		}
		
		~Configuration() {
			//Console.WriteLine("saving configuration before exit");
			Save();
		}
		
		public Configuration (string filename)
		{
			this.filename = filename;
			this.Load ();
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
			return Dump ();
		}
		
		public string Dump ()
		{
			string str = "";
			
			foreach (KeyValuePair<string,configurationGroup> kvp in storage) {
				//str += String.Format ("{0}\t= {1}\n", kvp.Key, kvp.Value)				
				if (kvp.Key != "") 
					str += String.Format ("[{0}]\n{1};[/{0}]\n\n", kvp.Key, kvp.Value);
			}
			return str;
		}
		
		public void Load() {
			this.Load(this.filename);
		}
		
		public void Load (string filename)
		{
			FileInfo finfo = new FileInfo (filename);
			if (finfo.Exists) {
				// create reader & open file
				StreamReader streamReader = new StreamReader (filename);

				// read file
				string text = streamReader.ReadToEnd ();
				// close the stream
				streamReader.Close ();
			
				this.Parse (text);
			} else
				Console.WriteLine ("File {0} is inaccessible.", filename);
		}
		
		private Timer saveScheduler;
		
		public void TriggerSave ()
		{
			if (saveScheduled == false) {
				saveScheduled = true;
				saveScheduler = new Timer (SaveTrigger, null, 1000 * 25, Timeout.Infinite);
			}
		}
		
		private void SaveTrigger (object state)
		{
			saveScheduled = false;
			this.Save();
		}
		
		public void Save ()
		{
			this.Save (this.filename);
		}
		
		public void Save (string filename)
		{
			try {
				FileInfo finfo = new FileInfo (filename);
				if (finfo.Exists) {
					Console.WriteLine("File {0} exists. Deleting file...", filename);
					finfo.Delete ();
				}
				
				string text = Dump ();		
				StreamWriter streamWriter = new StreamWriter (filename);
				streamWriter.Write (text);
				streamWriter.Close ();
			} catch (Exception e) {
				Console.WriteLine ("Failed: " + e.ToString ());
			}
		}
		
		
		public void Parse (string input)
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
			Configuration config = new Configuration ();
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
			
			config.Parse (@";This is a sample configuration file
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
			config.Save ();			
			/*
			
			configuration config2 = new configuration ();
			config2.parse (config.ToString ());
						
			Console.WriteLine (config2);*/
			
		}
	}
}