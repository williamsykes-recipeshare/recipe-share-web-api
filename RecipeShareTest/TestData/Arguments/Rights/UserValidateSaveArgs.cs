using System.Collections;
using RecipeShareLibrary.Model.CustomExceptions;
using RecipeShareLibrary.Model.Rights.Implementation;
using RecipeShareTest.TestData.Rights;

namespace RecipeShareTest.TestData.Arguments.Rights;

public class UserValidateSaveArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var user = UserData.CreateNewSave();
        user.Id = -1;
        yield return new object[] { 1, user, "Invalid user.", typeof(BadRequestException)  };

        user = UserData.CreateNewSave();
        user.UserRights = new List<UserRight>()
        {
            new ()
            {
                RightId = 1,
                CreatedById = 1,
                CreatedByName = "Tester",
                UpdatedById = 1,
                UpdatedByName = "Tester",
            },
            new ()
            {
                RightId = -2,
                CreatedById = 1,
                CreatedByName = "Tester",
                UpdatedById = 1,
                UpdatedByName = "Tester",
            },
        };
        yield return new object[] { 1, user, "Invalid right ID(s).", typeof(BadRequestException) };

        user = UserData.CreateNewSave();
        user.UserRights = new List<UserRight>()
        {
            new ()
            {
                RightId = 1,
                CreatedById = 1,
                CreatedByName = "Tester",
                UpdatedById = 1,
                UpdatedByName = "Tester",
            },
            new ()
            {
                RightId = long.MaxValue,
                CreatedById = 1,
                CreatedByName = "Tester",
                UpdatedById = 1,
                UpdatedByName = "Tester",
            },
        };
        yield return new object[] { 1, user, "Invalid right.", typeof(NotFoundException) };

        user = UserData.CreateNewSave();
        user.Id = long.MaxValue;
        yield return new object[] { 1, user, "Invalid user.", typeof(NotFoundException) };

        user = UserData.CreateNewSave();
        user.Email = "user1@mail.com";
        yield return new object[] { 1, user, "A user with the specified email already exists.", typeof(BadRequestException) };

        user = UserData.CreateNewSave();
        user.Guid = new Guid();
        yield return new object[] { 1, user, "Invalid guid.", typeof(BadRequestException)  };

        user = UserData.CreateNewSave();
        user.Guid = Guid.Empty;
        yield return new object[] { 1, user, "Invalid guid.", typeof(BadRequestException)  };

        using var validatorTest = new UserValidationArgs().GetEnumerator();

        while (validatorTest.MoveNext())
        {
            yield return new object[] { 1 }.Concat(validatorTest.Current).ToArray();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class UserValidationArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var user = UserData.CreateNewSave();
        user.Id = -1;
        yield return new object[] { user, "Invalid user.", typeof(BadRequestException)  };

        user = UserData.CreateNewSave();
        user.Name = "";
        yield return new object[] { user, "Invalid username.", typeof(BadRequestException)  };

        user = UserData.CreateNewSave();
        user.Name = "      ";
        yield return new object[] { user, "Invalid username.", typeof(BadRequestException)  };

        user = UserData.CreateNewSave();
        user.Email = "      ";
        yield return new object[] { user, "Invalid email.", typeof(BadRequestException)  };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class UserPasswordChangeArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var user = UserData.CreateNewSave();
        yield return new object[] { 1, user.Id, "Onetwo3", "Invalid password length, should be 8 or more characters.", typeof(BadRequestException) };

        user = UserData.CreateNewSave();
        yield return new object[] { 1, user.Id, "onetwo34", "Invalid password, should contain at least 1 uppercase character.", typeof(BadRequestException) };

        user = UserData.CreateNewSave();
        yield return new object[] { 1, user.Id, "ONETWO34", "Invalid password, should contain at least 1 lowercase character.", typeof(BadRequestException) };

        user = UserData.CreateNewSave();
        yield return new object[] { 1, user.Id, "Onetwothree", "Invalid password, should contain at least 1 number.", typeof(BadRequestException) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}