using BookLight.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLight.Models.ViewModels
{
    public class UserRoleVM
    {
        public ApplicationUser applicationUser { get; set; }
        public IEnumerable<SelectListItem> roleList { get; set; }
        public IEnumerable<SelectListItem> companyList { get; set; }
        public string currentRole { get; set; }
    }
}
