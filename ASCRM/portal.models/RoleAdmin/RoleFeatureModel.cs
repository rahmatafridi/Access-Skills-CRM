using System;
using System.Collections.Generic;
using System.Text;

namespace portal.models.RoleAdmin
{
    public sealed class RoleFeatureModel
    {
        public long Id { get; set; }
        public long Feature_Id { get; set; }
        public string Feature_Name { get; set; }
        public List<RolePermissionModel> Permissions { get; set; }        
    }
}
