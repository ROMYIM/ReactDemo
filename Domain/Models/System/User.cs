namespace ReactDemo.Domain.Models.System
{
    public class User : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ImageUrl { get; set; }
    }
}