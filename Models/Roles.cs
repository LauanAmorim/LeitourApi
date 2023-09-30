namespace LeitourApi.Models;
public class Roles
{
    public class Deactivated{
        public int Id {get; set;}
        public DateTime DeactivatedDate {get; set;}
        public Deactivated(int id) => Id = id;
    }

    public class Admin{

    }
    
}