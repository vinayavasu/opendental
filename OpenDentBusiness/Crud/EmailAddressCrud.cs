//This file is automatically generated.
//Do not attempt to make changes to this file because the changes will be erased and overwritten.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

namespace OpenDentBusiness.Crud{
	public class EmailAddressCrud {
		///<summary>Gets one EmailAddress object from the database using the primary key.  Returns null if not found.</summary>
		public static EmailAddress SelectOne(long emailAddressNum) {
			string command="SELECT * FROM emailaddress "
				+"WHERE EmailAddressNum = "+POut.Long(emailAddressNum);
			List<EmailAddress> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets one EmailAddress object from the database using a query.</summary>
		public static EmailAddress SelectOne(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EmailAddress> list=TableToList(Db.GetTable(command));
			if(list.Count==0) {
				return null;
			}
			return list[0];
		}

		///<summary>Gets a list of EmailAddress objects from the database using a query.</summary>
		public static List<EmailAddress> SelectMany(string command) {
			if(RemotingClient.RemotingRole==RemotingRole.ClientWeb) {
				throw new ApplicationException("Not allowed to send sql directly.  Rewrite the calling class to not use this query:\r\n"+command);
			}
			List<EmailAddress> list=TableToList(Db.GetTable(command));
			return list;
		}

		///<summary>Converts a DataTable to a list of objects.</summary>
		public static List<EmailAddress> TableToList(DataTable table) {
			List<EmailAddress> retVal=new List<EmailAddress>();
			EmailAddress emailAddress;
			foreach(DataRow row in table.Rows) {
				emailAddress=new EmailAddress();
				emailAddress.EmailAddressNum   = PIn.Long  (row["EmailAddressNum"].ToString());
				emailAddress.SMTPserver        = PIn.String(row["SMTPserver"].ToString());
				emailAddress.EmailUsername     = PIn.String(row["EmailUsername"].ToString());
				emailAddress.EmailPassword     = PIn.String(row["EmailPassword"].ToString());
				emailAddress.ServerPort        = PIn.Int   (row["ServerPort"].ToString());
				emailAddress.UseSSL            = PIn.Bool  (row["UseSSL"].ToString());
				emailAddress.SenderAddress     = PIn.String(row["SenderAddress"].ToString());
				emailAddress.Pop3ServerIncoming= PIn.String(row["Pop3ServerIncoming"].ToString());
				emailAddress.ServerPortIncoming= PIn.Int   (row["ServerPortIncoming"].ToString());
				emailAddress.UserNum           = PIn.Long  (row["UserNum"].ToString());
				emailAddress.AccessToken       = PIn.String(row["AccessToken"].ToString());
				emailAddress.RefreshToken      = PIn.String(row["RefreshToken"].ToString());
				retVal.Add(emailAddress);
			}
			return retVal;
		}

		///<summary>Converts a list of EmailAddress into a DataTable.</summary>
		public static DataTable ListToTable(List<EmailAddress> listEmailAddresss,string tableName="") {
			if(string.IsNullOrEmpty(tableName)) {
				tableName="EmailAddress";
			}
			DataTable table=new DataTable(tableName);
			table.Columns.Add("EmailAddressNum");
			table.Columns.Add("SMTPserver");
			table.Columns.Add("EmailUsername");
			table.Columns.Add("EmailPassword");
			table.Columns.Add("ServerPort");
			table.Columns.Add("UseSSL");
			table.Columns.Add("SenderAddress");
			table.Columns.Add("Pop3ServerIncoming");
			table.Columns.Add("ServerPortIncoming");
			table.Columns.Add("UserNum");
			table.Columns.Add("AccessToken");
			table.Columns.Add("RefreshToken");
			foreach(EmailAddress emailAddress in listEmailAddresss) {
				table.Rows.Add(new object[] {
					POut.Long  (emailAddress.EmailAddressNum),
					            emailAddress.SMTPserver,
					            emailAddress.EmailUsername,
					            emailAddress.EmailPassword,
					POut.Int   (emailAddress.ServerPort),
					POut.Bool  (emailAddress.UseSSL),
					            emailAddress.SenderAddress,
					            emailAddress.Pop3ServerIncoming,
					POut.Int   (emailAddress.ServerPortIncoming),
					POut.Long  (emailAddress.UserNum),
					            emailAddress.AccessToken,
					            emailAddress.RefreshToken,
				});
			}
			return table;
		}

		///<summary>Inserts one EmailAddress into the database.  Returns the new priKey.</summary>
		public static long Insert(EmailAddress emailAddress) {
			return Insert(emailAddress,false);
		}

		///<summary>Inserts one EmailAddress into the database.  Provides option to use the existing priKey.</summary>
		public static long Insert(EmailAddress emailAddress,bool useExistingPK) {
			if(!useExistingPK && PrefC.RandomKeys) {
				emailAddress.EmailAddressNum=ReplicationServers.GetKey("emailaddress","EmailAddressNum");
			}
			string command="INSERT INTO emailaddress (";
			if(useExistingPK || PrefC.RandomKeys) {
				command+="EmailAddressNum,";
			}
			command+="SMTPserver,EmailUsername,EmailPassword,ServerPort,UseSSL,SenderAddress,Pop3ServerIncoming,ServerPortIncoming,UserNum,AccessToken,RefreshToken) VALUES(";
			if(useExistingPK || PrefC.RandomKeys) {
				command+=POut.Long(emailAddress.EmailAddressNum)+",";
			}
			command+=
				 "'"+POut.String(emailAddress.SMTPserver)+"',"
				+"'"+POut.String(emailAddress.EmailUsername)+"',"
				+"'"+POut.String(emailAddress.EmailPassword)+"',"
				+    POut.Int   (emailAddress.ServerPort)+","
				+    POut.Bool  (emailAddress.UseSSL)+","
				+"'"+POut.String(emailAddress.SenderAddress)+"',"
				+"'"+POut.String(emailAddress.Pop3ServerIncoming)+"',"
				+    POut.Int   (emailAddress.ServerPortIncoming)+","
				+    POut.Long  (emailAddress.UserNum)+","
				+"'"+POut.String(emailAddress.AccessToken)+"',"
				+"'"+POut.String(emailAddress.RefreshToken)+"')";
			if(useExistingPK || PrefC.RandomKeys) {
				Db.NonQ(command);
			}
			else {
				emailAddress.EmailAddressNum=Db.NonQ(command,true,"EmailAddressNum","emailAddress");
			}
			return emailAddress.EmailAddressNum;
		}

		///<summary>Inserts one EmailAddress into the database.  Returns the new priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EmailAddress emailAddress) {
			return InsertNoCache(emailAddress,false);
		}

		///<summary>Inserts one EmailAddress into the database.  Provides option to use the existing priKey.  Doesn't use the cache.</summary>
		public static long InsertNoCache(EmailAddress emailAddress,bool useExistingPK) {
			bool isRandomKeys=Prefs.GetBoolNoCache(PrefName.RandomPrimaryKeys);
			string command="INSERT INTO emailaddress (";
			if(!useExistingPK && isRandomKeys) {
				emailAddress.EmailAddressNum=ReplicationServers.GetKeyNoCache("emailaddress","EmailAddressNum");
			}
			if(isRandomKeys || useExistingPK) {
				command+="EmailAddressNum,";
			}
			command+="SMTPserver,EmailUsername,EmailPassword,ServerPort,UseSSL,SenderAddress,Pop3ServerIncoming,ServerPortIncoming,UserNum,AccessToken,RefreshToken) VALUES(";
			if(isRandomKeys || useExistingPK) {
				command+=POut.Long(emailAddress.EmailAddressNum)+",";
			}
			command+=
				 "'"+POut.String(emailAddress.SMTPserver)+"',"
				+"'"+POut.String(emailAddress.EmailUsername)+"',"
				+"'"+POut.String(emailAddress.EmailPassword)+"',"
				+    POut.Int   (emailAddress.ServerPort)+","
				+    POut.Bool  (emailAddress.UseSSL)+","
				+"'"+POut.String(emailAddress.SenderAddress)+"',"
				+"'"+POut.String(emailAddress.Pop3ServerIncoming)+"',"
				+    POut.Int   (emailAddress.ServerPortIncoming)+","
				+    POut.Long  (emailAddress.UserNum)+","
				+"'"+POut.String(emailAddress.AccessToken)+"',"
				+"'"+POut.String(emailAddress.RefreshToken)+"')";
			if(useExistingPK || isRandomKeys) {
				Db.NonQ(command);
			}
			else {
				emailAddress.EmailAddressNum=Db.NonQ(command,true,"EmailAddressNum","emailAddress");
			}
			return emailAddress.EmailAddressNum;
		}

		///<summary>Updates one EmailAddress in the database.</summary>
		public static void Update(EmailAddress emailAddress) {
			string command="UPDATE emailaddress SET "
				+"SMTPserver        = '"+POut.String(emailAddress.SMTPserver)+"', "
				+"EmailUsername     = '"+POut.String(emailAddress.EmailUsername)+"', "
				+"EmailPassword     = '"+POut.String(emailAddress.EmailPassword)+"', "
				+"ServerPort        =  "+POut.Int   (emailAddress.ServerPort)+", "
				+"UseSSL            =  "+POut.Bool  (emailAddress.UseSSL)+", "
				+"SenderAddress     = '"+POut.String(emailAddress.SenderAddress)+"', "
				+"Pop3ServerIncoming= '"+POut.String(emailAddress.Pop3ServerIncoming)+"', "
				+"ServerPortIncoming=  "+POut.Int   (emailAddress.ServerPortIncoming)+", "
				+"UserNum           =  "+POut.Long  (emailAddress.UserNum)+", "
				+"AccessToken       = '"+POut.String(emailAddress.AccessToken)+"', "
				+"RefreshToken      = '"+POut.String(emailAddress.RefreshToken)+"' "
				+"WHERE EmailAddressNum = "+POut.Long(emailAddress.EmailAddressNum);
			Db.NonQ(command);
		}

		///<summary>Updates one EmailAddress in the database.  Uses an old object to compare to, and only alters changed fields.  This prevents collisions and concurrency problems in heavily used tables.  Returns true if an update occurred.</summary>
		public static bool Update(EmailAddress emailAddress,EmailAddress oldEmailAddress) {
			string command="";
			if(emailAddress.SMTPserver != oldEmailAddress.SMTPserver) {
				if(command!="") { command+=",";}
				command+="SMTPserver = '"+POut.String(emailAddress.SMTPserver)+"'";
			}
			if(emailAddress.EmailUsername != oldEmailAddress.EmailUsername) {
				if(command!="") { command+=",";}
				command+="EmailUsername = '"+POut.String(emailAddress.EmailUsername)+"'";
			}
			if(emailAddress.EmailPassword != oldEmailAddress.EmailPassword) {
				if(command!="") { command+=",";}
				command+="EmailPassword = '"+POut.String(emailAddress.EmailPassword)+"'";
			}
			if(emailAddress.ServerPort != oldEmailAddress.ServerPort) {
				if(command!="") { command+=",";}
				command+="ServerPort = "+POut.Int(emailAddress.ServerPort)+"";
			}
			if(emailAddress.UseSSL != oldEmailAddress.UseSSL) {
				if(command!="") { command+=",";}
				command+="UseSSL = "+POut.Bool(emailAddress.UseSSL)+"";
			}
			if(emailAddress.SenderAddress != oldEmailAddress.SenderAddress) {
				if(command!="") { command+=",";}
				command+="SenderAddress = '"+POut.String(emailAddress.SenderAddress)+"'";
			}
			if(emailAddress.Pop3ServerIncoming != oldEmailAddress.Pop3ServerIncoming) {
				if(command!="") { command+=",";}
				command+="Pop3ServerIncoming = '"+POut.String(emailAddress.Pop3ServerIncoming)+"'";
			}
			if(emailAddress.ServerPortIncoming != oldEmailAddress.ServerPortIncoming) {
				if(command!="") { command+=",";}
				command+="ServerPortIncoming = "+POut.Int(emailAddress.ServerPortIncoming)+"";
			}
			if(emailAddress.UserNum != oldEmailAddress.UserNum) {
				if(command!="") { command+=",";}
				command+="UserNum = "+POut.Long(emailAddress.UserNum)+"";
			}
			if(emailAddress.AccessToken != oldEmailAddress.AccessToken) {
				if(command!="") { command+=",";}
				command+="AccessToken = '"+POut.String(emailAddress.AccessToken)+"'";
			}
			if(emailAddress.RefreshToken != oldEmailAddress.RefreshToken) {
				if(command!="") { command+=",";}
				command+="RefreshToken = '"+POut.String(emailAddress.RefreshToken)+"'";
			}
			if(command=="") {
				return false;
			}
			command="UPDATE emailaddress SET "+command
				+" WHERE EmailAddressNum = "+POut.Long(emailAddress.EmailAddressNum);
			Db.NonQ(command);
			return true;
		}

		///<summary>Returns true if Update(EmailAddress,EmailAddress) would make changes to the database.
		///Does not make any changes to the database and can be called before remoting role is checked.</summary>
		public static bool UpdateComparison(EmailAddress emailAddress,EmailAddress oldEmailAddress) {
			if(emailAddress.SMTPserver != oldEmailAddress.SMTPserver) {
				return true;
			}
			if(emailAddress.EmailUsername != oldEmailAddress.EmailUsername) {
				return true;
			}
			if(emailAddress.EmailPassword != oldEmailAddress.EmailPassword) {
				return true;
			}
			if(emailAddress.ServerPort != oldEmailAddress.ServerPort) {
				return true;
			}
			if(emailAddress.UseSSL != oldEmailAddress.UseSSL) {
				return true;
			}
			if(emailAddress.SenderAddress != oldEmailAddress.SenderAddress) {
				return true;
			}
			if(emailAddress.Pop3ServerIncoming != oldEmailAddress.Pop3ServerIncoming) {
				return true;
			}
			if(emailAddress.ServerPortIncoming != oldEmailAddress.ServerPortIncoming) {
				return true;
			}
			if(emailAddress.UserNum != oldEmailAddress.UserNum) {
				return true;
			}
			if(emailAddress.AccessToken != oldEmailAddress.AccessToken) {
				return true;
			}
			if(emailAddress.RefreshToken != oldEmailAddress.RefreshToken) {
				return true;
			}
			return false;
		}

		///<summary>Deletes one EmailAddress from the database.</summary>
		public static void Delete(long emailAddressNum) {
			string command="DELETE FROM emailaddress "
				+"WHERE EmailAddressNum = "+POut.Long(emailAddressNum);
			Db.NonQ(command);
		}

	}
}