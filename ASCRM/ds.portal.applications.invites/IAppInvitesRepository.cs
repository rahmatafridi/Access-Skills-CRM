using ds.core.configuration.Models;
using ds.portal.applications.invites.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.invites
{
    public interface IAppInvitesRepository
    {
        IEnumerable<AppInvites> GetAllApplicationInvites();
        IEnumerable<AppInvites> GetAllApplicationInvitesForLead(int lead_id);

        AppInvites GetApplicationInviteById(int API_Id);

        ConfigurationModel LoadConfigData();
        bool InsertUpdateApplicationInvite(AppInvites appInvite);
        bool DeleteApplicationInviteById(int API_Id, int userId, string userName);
    }
}
