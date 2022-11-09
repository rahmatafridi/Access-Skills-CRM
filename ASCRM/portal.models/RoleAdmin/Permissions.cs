using System;
using System.Collections.Generic;
using System.Text;

namespace portal.models.RoleAdmin
{
    public class Permissions : Entity
    {
        private readonly Dictionary<string, bool> values;

        public Permissions(IDictionary<string, bool> values)
            : base()
        {
            this.values = new Dictionary<string, bool>(values ?? new Dictionary<string, bool>());
        }

        public bool this[string name]
        {
            get
            {
                if (this.values.ContainsKey(name))
                {
                    return this.values[name];
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (this.values.ContainsKey(name))
                {
                    this.values[name] = value;
                }
                else
                {
                    this.values.Add(name, value);
                }
            }
        }
    }
}
