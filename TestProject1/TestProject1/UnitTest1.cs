using NRules;
using NRules.Fluent;
using NRules.Fluent.Dsl;
using NRules.RuleModel;

namespace TestProject1;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        //Load rules
        var repository = new RuleRepository();
        repository.Load(x => x.From(typeof(RuleOfDoom).Assembly));

        //Compile rules
        var factory = repository.Compile();

        //Create a working session
        var session = factory.CreateSession();

        //Load domain model
        var request1 = new Request(DateOnly.Parse("01/01/2000"),DateOnly.Parse("01/01/2005"),  "NSW" );
        
        //Insert facts into rules engine's memory
        session.Insert(request1);

        //Start match/resolve/act cycle
        session.Fire();
    }

}


public class RuleOfDoom : Rule
{
    public override void Define()
    {

        IEnumerable<Request> requests = new List<Request>();
        IEnumerable<Response> responses = new List<Response>();
        
        When()
            .Match<Request>(r => r.State == "NSW")
            .Query(() => requests, query => 
                query.Match<Request>(
                    r => IsLessThanFiveYears(r))
                    .Collect().Where(r => r.Any()));

        Then().Do(ctx => NoLeave(ctx, responses));
    }

    private void NoLeave(IContext context, IEnumerable<Response> responses)
    {
        foreach (var response in responses)
        {
            response.SetDaysOwing(0);
        }
    }

    private bool IsLessThanFiveYears(Request request)
        => request.EmploymentEndDate.DayNumber - request.EmploymentStartDate.DayNumber < 5 * 365;  // good enough for now
}