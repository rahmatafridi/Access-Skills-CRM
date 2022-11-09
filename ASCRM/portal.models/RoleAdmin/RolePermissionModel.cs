using System;
using System.Collections.Generic;
using System.Text;

namespace portal.models.RoleAdmin
{
    public sealed class RolePermissionModel
    {
        public long Id { get; set; }
        public string Permission_Id { get; set; }
        public string Permission_Desc { get; set; }
        public long Parent_Feature_Id { get; set; }
        public bool is_checked { get; set; }
    }
}
