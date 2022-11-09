using ds.portal.applications.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications
{
   public interface IQuestionValidationRepository
    {
        IEnumerable<QuestionValidationModel> GetAllQuestionValidation();
        QuestionValidationModel AddUpdateQuestionValidation(QuestionValidationModel model);
        QuestionValidationModel GetQuestionValidationById(int id);
        bool DeleteValidationById(int id);
    }
}
