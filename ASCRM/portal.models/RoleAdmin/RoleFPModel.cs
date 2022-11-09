using System;
using System.Collections.Generic;
using System.Text;

namespace portal.models.RoleAdmin
{
    public class RoleFPModel
    {
        public IEnumerable<RoleFeatureModel> features;
        public IEnumerable<RolePermissionModel> permissions;
        public long Id { get; set; }
    }
}
