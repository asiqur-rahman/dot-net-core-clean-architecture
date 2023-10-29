using Project.Common.Support;
using Project.Core.Entities.Common.User;
using Project.Core.Entities.Common.User.Dtos;
using Project.Core.Entities.Helper;
using X.PagedList;

namespace Project.Core.Interfaces.Services.Common
{
    public interface IUserService
    {
        IPagedList<UserDto> PagedList(int page, int pageSize, List<int> roleList);
        IEnumerable<UserDto> GetAllModel();
        Task<bool> UpdateAsync(UserDto model);
        IEnumerable<UserDto> GetAll(string employeeId, int branchId, int roleId, bool? status);
        GridData<UserDto> PagedList(List<int> roleList, string status, List<ExpressionFilter> filters, string sort, string order, int pageNumber, int pageSize);
        Task<User> FindByIdAsync(string key);
        bool IsUserExist(string userName, int roleId, int userId);
        bool IsEmployeeIdExist(string employeeId, int roleId, int userId);
        Task<User> Get(int id);
        Task<string> GetRoleName(int roleId);
        IEnumerable<User> GetUserwiseAll(User user);
        IEnumerable<DropdownItemList> GetRoleDropDown();
        IEnumerable<DropdownItemList> GetAllAgent();
        IEnumerable<DropdownItemList> GetAllAgentByUser();
        IEnumerable<UserDto> GetUserByRole(int roleId);
        List<User> GetLockedUsers();
        Task<bool> UnlockUser(int id);
        IEnumerable<DropdownItemList> GetUserDropdown();
        User GetUserById(int Id);
        User GetUserByBranchIdAndRoleId(int branchId, int roleId);
        UserDto GetUserByUserId(int userId);
        List<UserDto> GetUserByBranchAndRole(int branchId, int role);
        Task<bool> UpdateUser(User model);
    }
}
