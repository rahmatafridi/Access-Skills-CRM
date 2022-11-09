using crm.portal.OscaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.MyDocument
{
   public interface IMyDocumentRepository
    {
        IEnumerable<LearnerDoc> LoadLearnerDoc(int Id);

    }
}
