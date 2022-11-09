using ds.portal.leads.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using ds.portal.leads.Models;

namespace ds.portal.leads
{
    public interface ILead_ListRepository
    {
        IEnumerable<DropdownOptionsHeader> GetDropdownOptionsHeaders();
        IEnumerable<DropdownOption> GetDropdownOptionsByHeaderName(string headerName);
        IEnumerable<DropdownOption> GetDropdownOptionsByHeaderId(int headerId);        
        bool InsertUpdateOptionRecord(DropdownOption dropdownOption);
        bool DeleteDropdownOptionByOptionId(int id, int userId, string userName);
        Tuple<bool, bool> CheckTitleAndValueExists(DropdownOption dropdownOption);
        IEnumerable<CompanyGroup> GetAllCompanyGroup();
        IEnumerable<Company> GetAllCompany();
        DropdownOption GetDropdownOptionsByHeaderNameAndValue(string headerName, string opt_value);
        DropdownOption GetLastResultBySa(int value, string title);

    }
}
