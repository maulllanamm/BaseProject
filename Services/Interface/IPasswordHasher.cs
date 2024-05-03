namespace Services.Interface
{
    public interface IPasswordHasher
    {
        public string ComputeHash(string password, string salt, string pepper, int iteration);
        public string GenerateSalt();
    }
}
