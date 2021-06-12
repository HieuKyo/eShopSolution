using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    //Chứa các thông tin lưu của User
    public class UserViewModel
    {
        //Muốn view ra cái nào thì khai báo qua rồi qua UserService để list ra
        public Guid Id { get; set; }
        [Display(Name = "Tên")]
        public string FirstName { get; set; }
        [Display(Name = "Họ")]
        public string LastName { get; set; }
        [Display(Name = "Ngày sinh")]
        public DateTime Dob { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }

    }
}
