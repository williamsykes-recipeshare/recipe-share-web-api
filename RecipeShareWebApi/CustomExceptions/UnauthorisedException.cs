namespace RecipeShareWebApi.CustomExceptions;

public class UnauthorisedException(string message) : Exception(message);