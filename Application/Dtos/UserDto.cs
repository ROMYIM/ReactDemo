using System.ComponentModel.DataAnnotations;

namespace ReactDemo.Application.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = "请输入用户名")]
        public string Username { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        public string VerifyCode { get; set; }

        public bool ValidateVerifyCode (string verifyCode)
        {
            if (!string.IsNullOrEmpty(verifyCode) && VerifyCode == verifyCode)
            {
                return true;
            } 
            return false;
        }
    }
}