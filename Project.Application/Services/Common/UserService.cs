using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Project.Common.Support;
using Project.Core.Entities.Common.Role;
using Project.Core.Entities.Common.User;
using Project.Core.Entities.Common.User.Dtos;
using Project.Core.Entities.Helper;
using Project.Core.Interfaces.Services.Common;
using Serilog;
using System.Data;
using X.PagedList;
using X.PagedList.Extensions;

namespace Project.Application.Services.Common
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _user;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper,IServiceProvider serviceProvider)
        {
            _user = serviceProvider.GetRequiredService<UserManager<User>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<UserRole>>();
            _mapper = mapper;
        }

        public IPagedList<UserDto> PagedList(int page, int pageSize, List<int> roleList)
        {
            var data = _user.Users.OrderByDescending(m => m.Id);
            IEnumerable<UserDto> iEnumerableDataSet = _mapper.Map<IQueryable<User>, IEnumerable<UserDto>>(data);

            return iEnumerableDataSet.Where(x => roleList.Any(y => y.Equals(x.RoleId))).ToPagedList(page, pageSize);
        }

        public GridData<UserDto> PagedList(List<int> roleList, string status, List<ExpressionFilter> filters, string sort, string order, int pageNumber, int pageSize)
        {
            var data = _user.Users.Select(x=>x);
            GridData<UserDto> result = _mapper.Map<GridData<User>, GridData<UserDto>>((GridData<User>)data);
            return result;
        }

        public IEnumerable<UserDto> GetAll(string employeeId, int branchId, int roleId, bool? status)
        {
            var userList = GetAllModel();//.Where(x => x.RoleId == (int)RoleEnum.UNIT_AGENT || x.RoleId == (int)RoleEnum.MASTER_AGENT);
            if (!string.IsNullOrEmpty(employeeId) && !string.IsNullOrWhiteSpace(employeeId))
            {
                userList = userList.Where(u => u.EmployeeId == employeeId);
            }
            else
            {
                if (status != null && (status.Value && branchId != 0 && roleId != 0))
                {
                    userList =
                        userList.Where(u => u.BranchId == branchId && u.RoleId == roleId && u.ActiveStatus == status);
                }
                else if (status != null && status.Value == false && branchId != 0 && roleId != 0)
                {
                    userList =
                        userList.Where(u => u.BranchId == branchId && u.RoleId == roleId && u.ActiveStatus == status);
                }
                else if (status != null && (status.Value && branchId == 0 && roleId != 0))
                {
                    userList = userList.Where(u => u.RoleId == roleId && u.ActiveStatus == status);
                }
                else if (status != null && (!status.Value == false && branchId == 0 && roleId != 0))
                {
                    userList = userList.Where(u => u.RoleId == roleId && u.ActiveStatus == status);
                }
                else if (status != null && (status.Value && branchId != 0 && roleId == 0))
                {
                    userList = userList.Where(u => u.BranchId == branchId && u.ActiveStatus == status);
                }
                else if (status != null && (status.Value == false && branchId != 0 && roleId == 0))
                {
                    userList = userList.Where(u => u.BranchId == branchId && u.ActiveStatus == status);
                }
                else if (status == null && branchId != 0 && roleId != 0)
                {
                    userList = userList.Where(u => u.BranchId == branchId && u.RoleId == roleId);
                }
                else if (status != null && branchId == 0 && roleId == 0)
                {
                    userList = userList.Where(u => u.ActiveStatus == status);
                }
                else if (status == null && branchId != 0 && roleId == 0)
                {
                    userList = userList.Where(u => u.BranchId == branchId);
                }
                else if (status == null && branchId == 0 && roleId != 0)
                {
                    userList = userList.Where(u => u.RoleId == roleId);
                }
            }
            return userList.OrderByDescending(a => a.UserId);
        }

        public IEnumerable<UserDto> GetAllModel()
        {
            IEnumerable<UserDto> model = new List<UserDto>();
            try
            {

            }
            catch (Exception e)
            {
                Log.Error(e, "Data loading failed!");
            }

            return model;
        }

        public virtual async Task<User> FindByIdAsync(string key)
        {
            return await _user.FindByIdAsync(key).ConfigureAwait(false);
        }

        public bool IsUserExist(string userName, int roleId, int userId)
        {
            if (userId == 0)
            {
                return _user.Users.Any(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase) && u.RoleId.Equals(roleId));
            }

            return _user.Users.Any(u => string.Equals(u.UserName, userName, StringComparison.OrdinalIgnoreCase) && u.RoleId.Equals(roleId) && u.Id.Equals(userId));
        }

        public bool IsEmployeeIdExist(string employeeId, int roleId, int userId)
        {
            if (userId == 0)
            {
                return _user.Users.Any(u => string.Equals(u.EmployeeId, employeeId, StringComparison.OrdinalIgnoreCase) && u.RoleId.Equals(roleId));
            }

            return _user.Users.Any(u => string.Equals(u.EmployeeId, employeeId, StringComparison.OrdinalIgnoreCase) && u.RoleId.Equals(roleId) && u.Id.Equals(userId));
        }

        public async Task<User> Get(int id)
        {
            return await _user.FindByIdAsync(Convert.ToString(id));
        }

        public IEnumerable<User> GetUserwiseAll(User user)
        {
            return _user.Users.Where(x => x.UserName == user.UserName && x.EmployeeId == user.EmployeeId);
        }

        public async Task<string> GetRoleName(int roleId)
        {
            var user = await _roleManager.FindByIdAsync(Convert.ToString(roleId));
            return user.Name;
        }

        public async Task<bool> UpdateAsync(UserDto model)
        {
            //var previousData = _auditTrail.GetPreviousData("Users", "", model.UserId.ToString());
            User user = await _user.FindByIdAsync(model.UserId.ToString());
            user.UserName = model.UserName;
            user.ActiveStatus = model.ActiveStatus;
            user.BranchId = model.BranchId;
            user.EntryDate = model.CreateDate.GetValueOrDefault();
            user.Email = model.Email;
            user.EmployeeId = model.EmployeeId;
            user.ExpiryDate = model.ExpiryDate;
            user.FullName = model.FullName;
            if (model.CurrentPassword != model.UserPassword)
            {
                user.Password = model.ConfirmPassword;
            }
            user.PhoneNumber = model.PhoneNumber;
            user.RoleId = model.RoleId;
            user.DesignationId = model.DesignationId;
            user.UpdateDate = DateTime.Now;
            user.UpdatedBy = model.UpdatedBy;
            user.Vacation = model.Vacation;
            user.IsAllBranchPermitted = model.IsAllBranchPermitted;
            user.FingerPrint = model.FingerPrint;
            user.ParentAgentUserId = model.ParentAgentId;
            user.PhoneNumber = model.PhoneNumber;
            user.SignatureImage = model.SignatureImageFile?.FileId;
            user.ProfilePicture = model.ProfilePictureFile?.FileId;
            user.IsSelectedBranchAllAgentPermitted = model.IsBranchAllAgentPermitted;

            IdentityResult result = await _user.UpdateAsync(user);
            if (result.Succeeded && (model.CurrentPassword != model.UserPassword))
            {
                await _user.ChangePasswordAsync(user, model.CurrentPassword, model.UserPassword);
            }
            
            await Task.CompletedTask;
            return result.Succeeded;
        }


        public async Task<bool> UpdateUser(User model)
        {
            IdentityResult result = await _user.UpdateAsync(model);
            return result.Succeeded;
        }


        public IEnumerable<DropdownItemList> GetRoleDropDown()
        {
            return _roleManager.Roles.Where(a => a.ActiveStatus).Select(c => new DropdownItemList
            {
                Id = c.Id.ToString(),
                Name = c.Name
            });
        }

        public IEnumerable<DropdownItemList> GetAllAgent()
        {
            return _user.Users
                //.Where(a => a.ActiveStatus && (a.RoleId == (int)RoleEnum.MASTER_AGENT || a.RoleId == (int)RoleEnum.UNIT_AGENT))
                .Select(c => new DropdownItemList
            {
                Id = c.Id.ToString(),
                Name = c.FullName
            });
        }

        public IEnumerable<DropdownItemList> GetAllAgentByUser()
        {
            return _user.Users
                //.Where(a => a.ActiveStatus && (a.RoleId == (int)RoleEnum.UNIT_AGENT || a.RoleId == (int)RoleEnum.MASTER_AGENT))
                .Select(c => new DropdownItemList
            {
                Id = c.BranchId.ToString(),
                //Name = c.Branch.Name
            });
        }




        public IEnumerable<UserDto> GetUserByRole(int roleId)
        {
            var model = from user in _user.Users.Where(u => u.RoleId == roleId)
                        select new UserDto
                        {
                            UserId = user.Id,
                            FullName = user.FullName,
                        };
            return model;
        }


        public List<User> GetLockedUsers()
        {
            var list = _user.Users.Where(u => u.LockoutEnd >= DateTime.Now).ToList();
            return list;
        }

        public async Task<bool> UnlockUser(int id)
        {
            //var previousData = _auditTrail.GetPreviousData("Users", "", model.UserId.ToString());
            User user = await _user.FindByIdAsync(id.ToString());
            user.LockoutEnd = null;
            user.AccessFailedCount = 0;

            IdentityResult result = await _user.UpdateAsync(user).ConfigureAwait(false);

            return result.Succeeded;
        }

        public IEnumerable<DropdownItemList> GetUserDropdown()
        {
            return from u in _user.Users
                   select new DropdownItemList
                   {
                       Id = u.Id.ToString(),
                       Name = u.FullName + " (" + u.Role.Name + ")"
                   };
        }

        public User GetUserById(int id)
        {
            return _user.Users.FirstOrDefault(u => u.Id >= id);
        }

        public User GetUserByBranchIdAndRoleId(int branchId, int roleId)
        {
            return _user.Users.FirstOrDefault(u => u.BranchId == branchId && u.RoleId == roleId);
        }

        public UserDto GetUserByUserId(int userId)
        {
            var model = from user in _user.Users
                        where user.Id.Equals(userId)
                        join role in _roleManager.Roles.Where(x => x.ActiveStatus) on user.RoleId equals role.Id
                        //join branch in _usersAuthorizationRepository.GetAll() on user.BranchId equals branch.BranchId
                        //join branch in _usersAuthorizationRepository.GetAll() on user.Id equals branch.MainTableId
                        //join center in _centerRepository.GetAll() on user.BranchId equals center.Id
                        //join d in _designationRepository.GetAll().AsEnumerable() on user.DesignationId equals d.Id into ud
                        //from dg in ud.DefaultIfEmpty()
                        //join access in _userAccess.GetAllUserAccess().Result.ToList() on user.Id equals access.UserId into u
                        //from access in u.DefaultIfEmpty()
                        select new UserDto
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            RoleId = user.RoleId,
                            DesignationId = user.DesignationId ?? 0,
                            //Designation = dg == null ? "N/A" : dg.DesignationName,
                            CreateDate = user.EntryDate,
                            ExpiryDate = user.ExpiryDate,
                            UpdateDate = user.UpdateDate,
                            UpdatedBy = user.UpdatedBy,
                            Vacation = user.Vacation,
                            FullName = user.FullName,
                            UserPassword = user.Password,
                            ActiveStatus = user.ActiveStatus,
                            Email = user.Email,
                            BranchId = user.BranchId,
                            RoleName = role.Name,
                            //UserBranch = branch.Branch.Name,
                            //BranchName = access != null ? access.BranchNameList : branch.Branch.Name, // Should change the property name as PermittedBranch
                            //BranchIdList = access != null ? access.BranchIdList : "0", // Should change the property name as PermittedBranchId
                            EmployeeId = user.EmployeeId,
                            PhoneNumber = user.PhoneNumber,
                            IsAllBranchPermitted = user.IsAllBranchPermitted,
                            ProfilePictureFile = user.ProfilePicture.GetFile(),
                            FingerPrint = user.FingerPrint,
                            ParentAgentId = user.ParentAgentUserId,
                            IsBranchAllAgentPermitted = user.IsSelectedBranchAllAgentPermitted
                        };
            return model.FirstOrDefault();
        }

        public List<UserDto> GetUserByBranchAndRole(int branchId, int role)
        {
            var model = from user in _user.Users.Where(u => u.BranchId == branchId && u.ActiveStatus == true && u.RoleId == role && u.NormalizedEmail != null)
                        select new UserDto
                        {
                            Email = user.NormalizedEmail
                        };
            return model.ToList();

        }

        public List<User> GetUsersByAgent(int agentId)
        {
            var model = _user.Users.Where(x => x.BranchId == agentId).ToList();
            //from user in _user.Users.Where(u => u.BranchId == agentId && u.ActiveStatus == true).Select(x=>x.UserName && x.Id);
            return model;
        }

        IEnumerable<DropdownItemList> IUserService.GetRoleDropDown()
        {
            throw new NotImplementedException();
        }

        IEnumerable<DropdownItemList> IUserService.GetAllAgent()
        {
            throw new NotImplementedException();
        }

        IEnumerable<DropdownItemList> IUserService.GetAllAgentByUser()
        {
            throw new NotImplementedException();
        }

        IEnumerable<DropdownItemList> IUserService.GetUserDropdown()
        {
            throw new NotImplementedException();
        }
    }
}
