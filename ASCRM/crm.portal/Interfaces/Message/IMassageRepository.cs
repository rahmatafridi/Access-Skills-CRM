using crm.portal.CrmModel;
using crm.portal.OscaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.Message
{
  public   interface IMassageRepository
    {
        IList<CourseTopic> courseTopics(int learnerId);
        IList<CourseTopic> courseTopicsByAdmin(int? learnerId);
       
        bool InsertMassage(Messages massages);
        Messages GetMessagesById(int id);
        IList<Messages> GetMessages(int learnerId);
        IList<Messages> GetMessagesAdmin();
        IList<MessageDetail> messageDetails(int id);
    }
}
