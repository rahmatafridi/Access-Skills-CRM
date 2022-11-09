using ds.portal.ui.Models;
namespace portal.models.RoleAdmin
{
    public class Role
    {
        public int security_role_id { get; set; }
        public bool is_active { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public string _encodeRoleId { get; private set; }
        public string identifier { get; set; }
        public string encodeRoleId
        {
            get
            {
                return Encryption.Encrypt(this.security_role_id.ToString());
            }
            set
            {
                _encodeRoleId = value;
            }
        }
    }
}
