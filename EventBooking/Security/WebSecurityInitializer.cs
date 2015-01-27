using WebMatrix.WebData;

namespace EventBooking.Security
{
    public class WebSecurityInitializer
    {
        private WebSecurityInitializer() { }
        public static readonly WebSecurityInitializer Instance = new WebSecurityInitializer();
        private bool isInit = false;
        private readonly object SyncRoot = new object();
        public void EnsureInitialize()
        {
            if (!isInit)
            {
                lock (this.SyncRoot)
                {
                    if (!isInit)
                    {
                        isInit = true;
                        WebSecurity.InitializeDatabaseConnection("EventBookingContext", "Users", "Id", "Email", autoCreateTables: true);
                    }
                }
            }
        }
    }
}