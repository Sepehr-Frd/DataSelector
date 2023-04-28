using System;
using AmazonMockup.Business.Businesses;
using AmazonMockup.Model.Models;

namespace AmazonMockup.Api.Controllers;

public class PersonController : BaseController<Person>
{
    public PersonController(BaseBusiness<Person> personBusiness) : base(personBusiness)
    {
    }
}

