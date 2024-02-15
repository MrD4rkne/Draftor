using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Draftor.Core.ViewModels;

namespace Draftor.Core.Interfaces;
public interface IGroupService
{
    Task<bool> DeleteGroupAsync(GroupVM group);

    Task<GroupVM> GetGroupAsync(int groupId);

    Task<IEnumerable<GroupVM>> GetGroupsListAsync();
}
