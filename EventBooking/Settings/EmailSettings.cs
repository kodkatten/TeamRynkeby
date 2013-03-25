namespace EventBooking.Settings
{
	public class EmailSettings
	{
		public string Server { get; private set; }
		public string Port { get; private set; }
		public string User { get; private set; }
		public string Password { get; private set; }
		public string From { get; private set; }

		public EmailSettings()
		{
			
		}

		public EmailSettings(string server, string port, string user, string password, string from)
		{
			Server = server;
			Port = port;
			User = user;
			Password = password;
			From = @from;
		}
	}
}