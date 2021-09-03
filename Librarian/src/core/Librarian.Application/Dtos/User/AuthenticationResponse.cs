namespace Librarian.Application.Dtos.User
{
    /*
     * Doğrulama başarılı olduğunda dönen response içeriğini temsil eden sınıf.
     * 
     * Aslında Claim sete ait bilgileri taşıdığını ifade edebiliriz.
     * Önemli detaylardan birisi Password alanının olmayışı ama üretilecek JWT token için Token isimli bir özellik kullanışıdır.
     */
    public class AuthenticationResponse
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public AuthenticationResponse(Domain.Entities.User user, string token)
        {
            UserId = user.UserId;
            Name = user.Name;
            LastName = user.LastName;
            Username = user.Username;
            Email = user.Email;
            Token = token;
        }
    }
}
