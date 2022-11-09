using ds.portal.applications.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications
{
   public interface IQuestionsRepository
    {
        IEnumerable<ApplicationSection> GetApplicationSection(int id);
        IEnumerable<ApplicationSteps> GetApplicationStep();
        IEnumerable<ApplicationQuestions> GetApplicationDepend(int setpId, int sectionId,int courseleveId);
        ApplicationQuestions InsertUpdateQuestionRecord(ApplicationQuestions question);
        IEnumerable<ApplicationQuestions> GetAllQuestions();
        IEnumerable<QuestionModel> GetListQuestions();
        IEnumerable<QuestionModel> QuestionsSearch(string name);
        IEnumerable<QuestionModel> QuestionsByCourseLevel(int id);
        IEnumerable<QuestionModel> GetQuestionsByType(int id);
        int AssignBulk(int levelFromId, int levelToId);
        int AssignSingleBulk(int levelId, int number);
        ApplicationQuestions GetQuestionById(int id);
        ApplicationQuestions AddCopyQuestion(ApplicationQuestions applicationQuestions);
        bool DeleteQuestionById(int id);
        bool DeleteHardQuestionById(int id);
        bool CheckQuestionExist(int levelId, int number);
        ApplicationVM CheckAppQuestionExist(int levelId);

        bool RemoveQuestionFromCourse(int id, int levelId);
    }
}
