using DataSelector.Business.Businesses;
using DataSelector.Model.Models;

namespace DataSelector.Api.Controllers;

public class QuestionController : BaseController<QuestionDocument>
{
    public QuestionController(QuestionBusiness questionBusiness) : base(questionBusiness)
    {
    }
}