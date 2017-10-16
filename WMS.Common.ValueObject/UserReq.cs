namespace WMS.Common
{
    public class KeyAccessModel
    {
        public string Key { get; set; }
        public string Token { get; set; }
    }

    public class FirebaseTokenModel
    {
        public string Token { get; set; }
        public string NewToken { get; set; }
    }
}