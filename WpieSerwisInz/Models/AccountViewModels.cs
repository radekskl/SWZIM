using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using WpieSerwisInz.Models.BO.Dictionary;

namespace WpieSerwisInz.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
        }

        public EditUserViewModel(ApplicationUser user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;
            this.CreateDate = user.CreateDate;
            this.Phone = user.PhoneNumber;
        }

        public string Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }


        [Display(Name = "First Name")]
        [DisplayFormat(NullDisplayText = "-")]
        public string FirstName { get; set; }


        [Display(Name = "Last Name")]
        [DisplayFormat(NullDisplayText = "-")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Email { get; set; }

        public DateTime CreateDate { get; set; }

        [Display(Name = "Phone")]
        [DisplayFormat(NullDisplayText = "-")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Rola")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Roles { get; set; }

        [Display(Name = "Layer")]
        [DisplayFormat(NullDisplayText = "-")]
        public string Layer { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [DisplayFormat(NullDisplayText = "-")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }

    public class RegisterAdminViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [DisplayFormat(NullDisplayText = "-")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public SelectUserRolesViewModel RolesList { get; set; }

    }

    public class SelectUserRolesViewModel
    {

        public SelectUserRolesViewModel()
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
        }

        public SelectUserRolesViewModel(bool clear)
        {
            this.Roles = new List<SelectRoleEditorViewModel>();
            var Db = new ApplicationDbContext();
            var allRoles = Db.Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                var rvm = new SelectRoleEditorViewModel(role);
                rvm.Selected = false;
                this.Roles.Add(rvm);
            }
        }

        // Enable initialization with an instance of ApplicationUser:

        public SelectUserRolesViewModel(ApplicationUser user)
            : this()
        {
            this.UserName = user.UserName;
            this.Id = user.Id;

            var Db = new ApplicationDbContext();

            // Add all available roles to the list of EditorViewModels:

            var allRoles = Db.Roles;
            foreach (var role in allRoles)
            {
                // An EditorViewModel will be used by Editor Template:
                if (role.Name != RoleType.Unconfirmed)
                {
                    var rvm = new SelectRoleEditorViewModel(role);
                    this.Roles.Add(rvm);
                }
            }

            // Set the Selected property to true for those roles for 
            // which the current user is a member:

            foreach (var userRole in user.Roles)
            {
                var checkUserRole =
                    this.Roles.Find(r => r.RoleName == userRole.Role.Name);
                if (!(checkUserRole == null && userRole.Role.Name == RoleType.Unconfirmed))
                {
                    checkUserRole.Selected = true;
                }
            }
        }
        public string UserName { get; set; }
        public string Id { get; set; }
        public List<SelectRoleEditorViewModel> Roles { get; set; }
    }

    public class SelectRoleEditorViewModel
    {
        public SelectRoleEditorViewModel() { }

        public SelectRoleEditorViewModel(IdentityRole role)
        {
            this.RoleName = role.Name;
        }

        public bool Selected { get; set; }

        [Required]
        public string RoleName { get; set; }

    }
}
