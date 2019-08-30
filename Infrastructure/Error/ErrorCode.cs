using System.ComponentModel;

namespace ReactDemo.Infrastructure.Error
{
    public enum ErrorCode
    {
        [Description("系统异常")]
        SystemError = -1,

        [Description("用户不存在")]
        UserNotExist,

        [Description("验证失败")]
        AuthenticationFail,

        [Description("登录失败")]
        LoginFail,

        [Description("验证码错误")]
        VerifyCodeError
    }
}