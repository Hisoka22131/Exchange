namespace Exchange.Domain.Entities;

public class Course
{
    public decimal CourseFrom { get; init; }
    public decimal MinAmountFrom { get; init; }
    public decimal MaxAmountFrom { get; init; }
    public decimal? AmountFrom { get; init; }

    public decimal CourseTo { get; init; }
    public decimal MinAmountTo { get; init; }
    public decimal MaxAmountTo { get; init; }
    public decimal? AmountTo { get; init; }
    
    public decimal FeeInUsdt { get; init; }
    public decimal FeeInCurrency { get; init; }
    public decimal FeePercent { get; init; }
}