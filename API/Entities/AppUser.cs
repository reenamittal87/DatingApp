namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        // private int myVar;
        // public int MyProperty
        // {
            // get { return myVar; }
            // set { myVar = value; }
        // }
        
    }
}