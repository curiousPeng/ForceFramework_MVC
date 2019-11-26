using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Force.Model.ViewModel.SystemUser
{
    public class SystemUserCreate
    {
        [Required(ErrorMessage = "昵称是必填的")]
        public string Name { get; set; }
        [Required(ErrorMessage = "手机是必填的")]
        [Phone]
        public string Phone { get; set; }
        [Required(ErrorMessage="邮箱是必填的")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "账号是必填的")]
        [MinLength(8,ErrorMessage ="账号长度不能小于8位")]
        [MaxLength(16, ErrorMessage = "账号长度不能大于16位")]
        public string Account { get; set; }
        [Required(ErrorMessage = "密码是必填的")]
        [MinLength(6, ErrorMessage = "密码长度不能小于6位")]
        [MaxLength(16, ErrorMessage = "密码长度不能大于16位")]
        public string Pwd { get; set; }
        public short IsUse { get; set; }
    }
}
