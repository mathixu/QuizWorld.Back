using QuizWorld.Infrastructure.Services;

namespace QuizWorld.Infrastructure.Tests.Identity;

[TestClass]
public class HashServiceTests
{
    private HashService _hashService;

    [TestInitialize]
    public void SetUp()
    {
        _hashService = new HashService();
    }

    #region Hash Method Tests

    [TestMethod]
    public void Hash_NonEmptyString_ReturnsValidHash()
    {
        // Arrange
        var input = "testString";

        // Act
        var hash = _hashService.Hash(input);

        // Assert
        Assert.IsNotNull(hash);
        Assert.AreNotEqual(input, hash);
    }

    [TestMethod]
    public void Hash_DifferentStrings_ReturnDifferentHashes()
    {
        // Arrange
        var input1 = "stringOne";
        var input2 = "stringTwo";

        // Act
        var hash1 = _hashService.Hash(input1);
        var hash2 = _hashService.Hash(input2);

        // Assert
        Assert.AreNotEqual(hash1, hash2);
    }

    [TestMethod]
    public void Hash_EmptyString_ReturnsValidHash()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var hash = _hashService.Hash(input);

        // Assert
        Assert.IsNotNull(hash);
        Assert.AreNotEqual(input, hash);
    }

    [TestMethod]
    public void Hash_SameStrings_ReturnDifferentHashes()
    {
        // Arrange
        var input = "repeatString";

        // Act
        var hash1 = _hashService.Hash(input);
        var hash2 = _hashService.Hash(input);

        // Assert
        Assert.AreNotEqual(hash1, hash2, "Hashes should differ due to salting.");
    }
    #endregion

    #region Verify Method Tests
    [TestMethod]
    public void Verify_CorrectInput_ReturnsTrue()
    {
        // Arrange
        var input = "testPassword";
        var hash = _hashService.Hash(input);

        // Act
        var isValid = _hashService.Verify(input, hash);

        // Assert
        Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void Verify_IncorrectInput_ReturnsFalse()
    {
        // Arrange
        var input = "testPassword";
        var wrongInput = "wrongPassword";
        var hash = _hashService.Hash(input);

        // Act
        var isValid = _hashService.Verify(wrongInput, hash);

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void Verify_EmptyInput_ReturnsFalse()
    {
        // Arrange
        var input = "testPassword";
        var emptyInput = string.Empty;
        var hash = _hashService.Hash(input);

        // Act
        var isValid = _hashService.Verify(emptyInput, hash);

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void Verify_InputWithAlteredHash_ReturnsFalse()
    {
        // Arrange
        var input = "testPassword";
        var hash = _hashService.Hash(input);
        var alteredHash = hash.Remove(hash.Length - 1) + 'x';

        // Act
        var isValid = _hashService.Verify(input, alteredHash);

        // Assert
        Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void Verify_NullInput_ThrowsArgumentNullException()
    {
        // Arrange
        var input = "testPassword";
        var hash = _hashService.Hash(input);

        // Act & Assert
        Assert.ThrowsException<ArgumentNullException>(() => _hashService.Verify(null, hash));
    }

    #endregion
}
