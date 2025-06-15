using System.Security.Cryptography;
using System.Text;
using RecipeShareLibrary.Model.Rights;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RecipeShareLibrary.Helper;

public static class PasswordHelper
{
    private static readonly char[] Chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_@!#$%^&*()<>,.?/':;[]{}/|".ToCharArray();

    /// <summary>
    /// Private class for holding hashing information
    /// </summary>
    private class HashVersion
    {
        public short Version { get; init; }
        public int SaltSize { get; init; }
        public int HashSize { get; init; }
        public KeyDerivationPrf KeyDerivation { get; init; }
        public int Iterations { get; init; }
    }

    /// <summary>
    /// Password hashing versions
    /// </summary>
    private static readonly Dictionary<short, HashVersion> Versions = new()
    {
        {
            1, new HashVersion
            {
                Version = 1,
                KeyDerivation = KeyDerivationPrf.HMACSHA512,
                HashSize = 512 / 8,
                SaltSize = 128 / 8,
                Iterations = 100000
            }
        }
    };

    /// <summary>
    /// Current version
    /// </summary>
    private const short CurrentVersion = 1;

    private const int SizeOfVersion = sizeof(short);
    private const int SizeOfIterationCount = sizeof(int);

    /// <summary>
    /// Hash a text password and returns byte array
    /// containing version, iteration and salted hash
    /// </summary>
    /// <param name="textPassword"></param>
    /// <param name="version"></param>
    /// <param name="password"></param>
    private static void HashPassword(string textPassword, short version, out byte[] password)
    {
        var currentVersion = Versions[version];

        byte[] salt;
        RandomNumberGenerator.Create().GetBytes(salt = new byte[currentVersion.SaltSize]);
        var versionBytes = BitConverter.GetBytes(currentVersion.Version);
        var iterationBytes = BitConverter.GetBytes(currentVersion.Iterations);
        var hashBytes = KeyDerivation.Pbkdf2(textPassword, salt, currentVersion.KeyDerivation, currentVersion.Iterations, currentVersion.HashSize);

        //calculate the indexes for the combined hash
        var indexIteration = SizeOfVersion;
        var indexSalt = indexIteration + SizeOfIterationCount;
        var indexHash = indexSalt + currentVersion.SaltSize;

        //combine all data to one result hash
        var resultBytes = new byte[SizeOfVersion + SizeOfIterationCount + currentVersion.SaltSize + currentVersion.HashSize];
        Array.Copy(versionBytes, 0, resultBytes, 0, SizeOfVersion);
        Array.Copy(iterationBytes, 0, resultBytes, indexIteration, SizeOfIterationCount);
        Array.Copy(salt, 0, resultBytes, indexSalt, currentVersion.SaltSize);
        Array.Copy(hashBytes, 0, resultBytes, indexHash, currentVersion.HashSize);
        password = resultBytes;
    }

    /// <summary>
    /// Hash a text password based on current hashing version,
    /// and returns byte array
    /// containing version, iteration and salted hash
    /// </summary>
    /// <param name="textPassword"></param>
    /// <param name="password"></param>
    public static void HashPassword(string textPassword, out byte[] password)
    {
        HashPassword(textPassword, CurrentVersion, out password);
    }

    /// <summary>
    /// Slow Equals two byte arrays, mainly used for comparing to
    /// hashed passwords
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private static bool SlowEquals(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
    {
        var diff = (uint)a.Count ^ (uint)b.Count;
        for (var i = 0; i < a.Count && i < b.Count; i++)
            diff |= (uint)(a[i] ^ b[i]);
        return diff == 0;
    }

    private static bool VerifyHashedPassword(byte[] password, string textPassword)
    {
        //Get the current version and number of iterations
        var currentVersion = Versions[BitConverter.ToInt16(password, 0)];
        var iteration = BitConverter.ToInt32(password, 2);

        //Create the byte arrays for the salt and hash
        var saltBytes = new byte[currentVersion.SaltSize];
        var hashBytes = new byte[currentVersion.HashSize];

        //Calculate the indexes of the salt and the hash
        var indexSalt = SizeOfVersion + SizeOfIterationCount; // Int16 (Version) and Int32 (Iteration)
        var indexHash = indexSalt + currentVersion.SaltSize;

        //Fill the byte arrays with salt and hash
        Array.Copy(password, indexSalt, saltBytes, 0, currentVersion.SaltSize);
        Array.Copy(password, indexHash, hashBytes, 0, currentVersion.HashSize);

        //Hash the current clearText with the parameters given via the data
        var verificationHashBytes = KeyDerivation.Pbkdf2(textPassword, saltBytes, currentVersion.KeyDerivation, iteration, currentVersion.HashSize);

        //Check if generated hashes are equal
        return SlowEquals(hashBytes, verificationHashBytes);
    }

    public static bool VerifyHashedPassword(IUserPassword userPassword, string textPassword)
    {
        return VerifyHashedPassword(userPassword.Password!, textPassword);
    }

    public static string RandomPasswordGenerator(int length)
    {
        byte[] data = new byte[4 * length];
        using RandomNumberGenerator crypto = RandomNumberGenerator.Create();

        crypto.GetBytes(data);

        StringBuilder result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % Chars.Length;

            result.Append(Chars[idx]);
        }

        return result.ToString();
    }

    public static byte[] RandomByteGenerator(int length)
    {
        byte[] data = new byte[length];
        using RandomNumberGenerator crypto = RandomNumberGenerator.Create();

        crypto.GetBytes(data);

        return data;
    }
    
    public static IEnumerable<IUser> WithoutPasswords(this IEnumerable<IUser> users)
    {
        return users.Select(x => WithoutPassword(x));
    }

    public static IUser WithoutPassword(this IUser user)
    {
        if (user == null) return null;

        user.UserPassword = null;
        return user;
    }
}