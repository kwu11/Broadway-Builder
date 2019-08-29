using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Enums
{
    public enum PermissionsEnum
    {
        UpgradeGeneralUserToTheaterAdmin = 1,
        DowngradeTheaterAdminToGeneralUser = 2,
        DisableTheaterAdmin = 3,
        EnableTheaterAdmin = 4,
        ActivateAbusiveAccount = 5,
        ActivateNonAbusiveAccount = 6,
        DisableGeneralUser = 7,
    }
}
