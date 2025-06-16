using FakeItEasy;
using FakeItEasy.Creation;

namespace RecipeShareTest.Helpers;

public static class MockHelper
{
    public static T Mock<T>(params object[] args) where T : class
    {
        var mock = A.Fake<T>(options =>
        {
            options.WithArgumentsForConstructor(args);
        });
        return mock;
    }

    public static T Mock<T>() where T : class
    {
        var mock = A.Fake<T>();
        return mock;
    }

    public static T Mock<T>(Action<IFakeOptions<T>> optionsBuilder) where T : class
    {
        var mock = A.Fake<T>(optionsBuilder);
        return mock;
    }
}