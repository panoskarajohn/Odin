using Ardalis.GuardClauses;
using IdGen;

namespace Shared.IdGenerator;

public static class SnowFlakIdGenerator
{
    private static IdGen.IdGenerator? _generator;

    public static void Configure(int generatorId)
    {
        Guard.Against.NegativeOrZero(generatorId, nameof(generatorId));

        // Let's say we take jan 17st 2022 as our epoch
        var epoch = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Local);

        // Create an ID with 45 bits for timestamp, 2 for generator-id
        // and 16 for sequence
        var structure = new IdStructure(45, 2, 16);

        // Prepare options
        var options = new IdGeneratorOptions(structure, new DefaultTimeSource(epoch));

        // Create an IdGenerator with it's generator-id set to 0, our custom epoch
        // and id-structure
        _generator = new IdGen.IdGenerator(0, options);
    }

    /// <summary>
    ///     Make sure the <see cref="SnowFlakIdGenerator.Configure" /> method has been called.
    /// </summary>
    /// <returns></returns>
    public static long NewId()
    {
        return _generator!.CreateId();
    }
}