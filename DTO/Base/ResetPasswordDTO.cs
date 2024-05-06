namespace DTO.Base
{
    public class ResetPasswordDTO
    {
        public string PasswordResetToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
