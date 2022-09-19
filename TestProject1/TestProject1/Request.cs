namespace TestProject1;

public record Request(DateOnly EmploymentStartDate, DateOnly EmploymentEndDate, string State);

public class Response
{
    
    public void SetDaysOwing(int i)
    {
        DaysOwing = i;
    }

    public int DaysOwing { get; private set; }
}
