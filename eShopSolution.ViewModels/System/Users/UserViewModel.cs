using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    //Chứa các thông tin lưu của User
    public class UserViewModel
    {
        //Muốn view ra cái nào thì khai báo qua rồi qua UserService để list ra
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

    }
}
