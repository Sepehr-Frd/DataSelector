using AmazonMockup.DataAccess;
using AmazonMockup.Model.Models;

namespace AmazonMockup.Business.Businesses;

public class QuestionBusiness : BaseBusiness<QuestionDocument>
{
    public QuestionBusiness(IBaseRepository<QuestionDocument> repository) : base(repository)
    {
    }
}

