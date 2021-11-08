using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Dtos
{
    public class UserDto: BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}
