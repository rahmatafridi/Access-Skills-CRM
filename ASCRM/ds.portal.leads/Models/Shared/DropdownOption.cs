using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.leads.Models.Shared
{
    public sealed class DropdownOption
    {
        public int id { get; set; }
        public string description { get; set; }
        public int Opt_Id { get; set; }
        public string Opt_Value { get; set; }
        public string Opt_Title { get; set; }
        public int Opt_Id_OptHeader { get; set; }
        public bool Opt_IsDeleted { get; set; }
        public int Opt_SortOrder { get; set; }
    }
}
