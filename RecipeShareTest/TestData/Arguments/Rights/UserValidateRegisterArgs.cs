using System.Collections;
using RecipeShareLibrary.Model.CustomExceptions;

namespace RecipeShareTest.TestData.Arguments.Rights;

public class UserValidateRegisterArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "", "mail6@mail.com", "#P@ssw0rd12", "Invalid name.", typeof(BadRequestException)  };
        yield return new object[] { "                ", "mail6@mail.com", "#P@ssw0rd12", "Invalid name.", typeof(BadRequestException)  };
        yield return new object[] { "Test User", "", "#P@ssw0rd12", "Invalid email.", typeof(BadRequestException)  };
        yield return new object[] { "Test User", "          ", "#P@ssw0rd12", "Invalid email.", typeof(BadRequestException)  };
        yield return new object[] { "Test User", "L", "#P@ssw0rd12", "Invalid email.", typeof(BadRequestException)  };
        yield return new object[] { "Test User", "Llfjdsol", "#P@ssw0rd12", "Invalid email.", typeof(BadRequestException)  };
        yield return new object[] { "Test User", "mail6@mail.com", "", "Invalid password.", typeof(BadRequestException)  };
        yield return new object[] { "Test User", "mail6@mail.com", "Onetwo3", "Invalid password length, should be 8 or more characters.", typeof(BadRequestException) };
        yield return new object[] { "Test User", "mail6@mail.com", "onetwo34", "Invalid password, should contain at least 1 uppercase character.", typeof(BadRequestException) };
        yield return new object[] { "Test User", "mail6@mail.com", "ONETWO34", "Invalid password, should contain at least 1 lowercase character.", typeof(BadRequestException) };
        yield return new object[] { "Test User", "mail6@mail.com", "Onetwothree", "Invalid password, should contain at least 1 number.", typeof(BadRequestException) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class UserValidRegisterArgs : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        using var validatorTest = new UserValidateRegisterArgs().GetEnumerator();

        while (validatorTest.MoveNext())
        {
            yield return validatorTest.Current.ToArray();
        }

        yield return new object[] { "Test User", "user1@mail.com", "#P@ssw0rd12", "Email address already in use.", typeof(BadRequestException) };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}