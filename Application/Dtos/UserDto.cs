using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ReactDemo.Application.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = "请输入用户名"), BindProperty(Name = "username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "请输入密码"), BindProperty(Name = "password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "请输入验证码"), BindProperty(Name = "verifycode")]
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