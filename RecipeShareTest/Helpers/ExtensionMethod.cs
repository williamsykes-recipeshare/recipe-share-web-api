using FluentAssertions;
using FluentAssertions.Extensions;
using FluentAssertions.Primitives;
using RecipeShareLibrary.Model;
using RecipeShareLibrary.Model.Rights;

namespace RecipeShareTest.Helpers;

public static class ExtensionMethod
{
    public static AndConstraint<ObjectAssertions> BeValidAudit(this IBaseModel baseModel, IBaseModel other, IUser user)
    {
        if (other.Id == 0)
        {
            baseModel.Id.Should().BeGreaterThan(0);
            baseModel.CreatedOn.BeValidDefaultNow();
        }
        else
        {
            baseModel.Id.Should().Be(other.Id);
        }

        baseModel.UpdatedOn.BeValidDefaultNow();

        return baseModel.Should()
            .Match<IBaseModel>(x => other.IsActive != null || x.IsActive == true)
            .And.Match<IBaseModel>(x => x.Id > 0)
            .And.Match<IBaseModel>(x => other.Id != 0 || x.CreatedById == user.Id)
            .And.Match<IBaseModel>(x => other.Id != 0 || x.CreatedByName == user.Name)
            .And.Match<IBaseModel>(x => x.UpdatedById == user.Id)
            .And.Match<IBaseModel>(x => x.UpdatedByName == user.Name);
    }

    private static AndConstraint<NullableDateTimeAssertions> BeValidDefaultNow(this DateTime? dateTime, bool utc = false)
    {
        return dateTime.Should()
            .BeIn(utc ? DateTimeKind.Utc : DateTimeKind.Unspecified)
            .And.BeCloseTo(DateTime.UtcNow, 1.Minutes());
    }
}